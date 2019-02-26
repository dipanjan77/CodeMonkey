using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Connector;

namespace HotelBot.Dialogs
{
    
    [Serializable]
    public class GreetingDialog : IDialog
    {
        
         async Task IDialog<object>.StartAsync(IDialogContext context)
        {
            await context.PostAsync("hi I am John Bot");
            context.Wait(MessageReceivedAsync);


        }

       public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var userName = String.Empty;
            var getName = false;


            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if(getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);

            }

            if(string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name?");
                context.UserData.SetValue<bool>("GetName", true);
                
            }
            else
            {
                await context.PostAsync(String.Format("Hi {0}. How can I help you today?", userName));

            }
            context.Wait(MessageReceivedAsync);


        }
    }
}