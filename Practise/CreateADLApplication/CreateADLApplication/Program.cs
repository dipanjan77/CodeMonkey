using System;
using System.Threading;

using Microsoft.Rest.Azure.Authentication;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.Azure.Management.DataLake.Store.Models;
using Microsoft.Azure.Management.DataLake.StoreUploader;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Security.Cryptography.X509Certificates; //Required only if you are using an Azure AD application created with certificates
using System.Collections.Generic;

namespace SdkSample
{
    class Program
    {
        private static DataLakeStoreAccountManagementClient _adlsClient;
        private static DataLakeStoreFileSystemManagementClient _adlsFileSystemClient;

        private static string _adlsAccountName;
        private static string _resourceGroupName;
        private static string _location;
        private static string _subId;

        private static void Main(string[] args)
        {
            _adlsAccountName = "dipanbappnexus"; // TODO: Replace this value with the name of your existing Data Lake Store account.
            _resourceGroupName = "dipanbappnexus"; // TODO: Replace this value with the name of the resource group containing your Data Lake Store account.
            _location = "East US 2";
            _subId = "66935119xxxxxxxxxxxx2-10d7e8e377cf";

            string localFolderPath = @"C:\WORKAREA\Enlist\GitHub\CodeMonkey\Practise\CreateADLApplication\CreateADLApplication\Data\"; // TODO: Make sure this exists and can be overwritten.
            string localFilePath = localFolderPath + "file.txt"; // TODO: Make sure this exists and can be overwritten.
            string remoteFolderPath = "/data_lake_path/";
            string remoteFilePath = remoteFolderPath + "file.txt";

            try
            {
                // Service principal / appplication authentication with client secret / key
                // Use the client ID of an existing AAD "Web App" application.
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                var domain = "72f988bf-86f1-xxxxxxxxxx-2d7cd011db47"; //This is the tenant ID
                var webApp_clientId = "76c6a67b-xxxxxxxxx-08b81c66c654"; //This is the Application ID
                var clientSecret = "oXW9O+R6Kxxxxxxxxxxxxxx8dC8yhWCg1mYafdGfE="; //This is the secret key
                var clientCredential = new ClientCredential(webApp_clientId, clientSecret);
                var creds = ApplicationTokenProvider.LoginSilentAsync(domain, clientCredential).Result;

                // Create client objects and set the subscription ID
                _adlsClient = new DataLakeStoreAccountManagementClient(creds);
                _adlsFileSystemClient = new DataLakeStoreFileSystemManagementClient(creds);
                _adlsClient.SubscriptionId = _subId;

                var accounts = ListAdlStoreAccounts();
                foreach (var account in accounts)
                    Console.WriteLine(account.Name.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }



            Console.ReadLine();

        }
        // List all ADLS accounts within the subscription
        public static List<DataLakeStoreAccount> ListAdlStoreAccounts()
        {
            var response = _adlsClient.Account.List();
            var accounts = new List<DataLakeStoreAccount>(response);

            while (response.NextPageLink != null)
            {
                response = _adlsClient.Account.ListNext(response.NextPageLink);
                accounts.AddRange(response);
            }

            return accounts;
        }
    }
}
