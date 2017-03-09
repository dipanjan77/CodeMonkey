import random

def get_random_word():
    words=["pizza","cheese","apples"]
    word=words[random.randint(0,len(words)-1)]
    return word
               


def play_word_game():
    strikes=0
    max_strikes=3
    playing = True
    
    word=get_random_word()

    while playing:
        strikes+=1

        if strikes>=max_strikes:
            playing=False

    if strikes>=max_strikes:
        print("Looser!")
    else:
        print("Winner!")


    
print("Game Started")
play_word_game()
print("Game Over!")
