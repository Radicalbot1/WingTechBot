using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;

namespace WingTechBot;
using System.Diagnostics;
using System.Timers;
using System.Threading;

public class HotPotat : Game
{
    private int _timerDuration;
    private System.Timers.Timer _tooHotTimer;
    private ulong _potatWielder;
    private Random _random = new();

    List<ulong> _alive = new();
    List<ulong> _dead = new();

    Dictionary<ulong, int> _scores = new();

    public string[] collectText = { "has the potat!",  "is now a potat-er", "is no longer potat-less", "loves potats", "doesn't have the potat... lol jk", "has a high potat diet", "never skips potat day",
            "leads the change in the potat eating challeng", "could eat potats fater", "joins the potat war", "plants the seeds for more potats", "hates tomats", "knows that potats are the best fruit",
            "found the golden potat", "slides into their potats", "eats the most healthy fruit", "sets up their children's future with potats", "steals the potat from their enemy's hands",
            "couldn't live without a potat", "slays demons with the help of the potat", "knows that the market cap of the potat is infinite", "has created the potat NFT", "knows the power of the potat side",
            "knows their parents love potats more then them", "would sell their soul for potats", "sails the seven seas for potats", "is the Dream Minecraft of potats", "has replaced their kidney with potats",
            "never skips potat day", "becomes potat-sexual", "married their favorite potat", "has discovered a new breed of potat!"};

    public string[] roundText = { "Potat time is starting!", "Watch out! That potat's hot!", "Get your mouths ready for... Hot Potat!", "Warning! Hot Potat inbound!", "Time for a new round of... Hot Potat" };

    public string[] detonationText = { "had their career ended by the potat detonation", "has perished to the potat", "could not handle the raw power of a potat", "has accended due to the potat", 
        "has left the mortal plane due to the potat", "has been consumed by the potat", "has failed to overcome the power of the potat", "ceased exiting because of the potat detonation", 
        "had their life cut short due to a potat", "has been eviserated due to a potat", "can not eat another potat again", "never stood a chance against the potat", "potat-ed their last potat", 
        "had an allergic reaction to the potat", "thought the potat was a vegetable", "thought eating another fruit would save them from death", "never stood a chance and is now dead", "is dead",
        "is now a potat ghost", "has lost their taste buds due to the heat", "couldn't handle the heat of a potat"};

    public string[] deadText = { "You can't pass to a dead potat-er", "That person is nothing but bones", "Dust can't play Hot Potat", "That person already bit the potat", "No messing with the dead",
        "Such actions against the dead are unnaceptable in this game", "Only bones remain...", "Their tombstone haunts you", "Let loved ones rest", "They are already full of potat", 
        "They have no stomach to put the potat in", "Their fate was already sealed", "They already summited to the potat", "Their memory lives on but not their potat", "Try someone alive next time",
        "They are healthy enough", "Don't give food to a full person", "I would say nobody can have enough but...", "Could you pay attention to who is alive?", "It stuck to your fingers",
        "Imagine not knowing who is still playing Hot Potat", "Cringe, Pass to someone who hasen't eaten a potat", "We're not graverobbers, pass it to someone alive"};
    protected override void Start()
    {
        _timerDuration = Prompt<int>(GamemasterID, AllowedChannels, true, "How long til potat detonation (in seconds)?");
        while(_timerDuration <= 1) _timerDuration = Prompt<int>(GamemasterID, AllowedChannels, true, "The potat can't be detonated *that* fast, try again");
    }

    public override void RunGame()
    {
        _alive.AddRange(PlayerIDs);
        foreach (ulong id in _alive) if(!_scores.ContainsKey(id))_scores.Add(id, 0);
        _potatWielder = _alive[_random.Next(_alive.Count)];
        WriteLine("Welcome to HOT POTAT comrades, let us Start Game!__\n\n__");

        while(_alive.Count > 1)
        {
            PlayRound();

            foreach(ulong id in _alive) _scores[id] += 1;
            if (_alive.Count == 1) break;
            WriteLine(roundText[_random.Next(roundText.Length)]);
        }

        GameOver();
    }

    private void PlayRound()
    {
        while (true)
        {
            WriteLine($"{GetPlayer(_potatWielder)} {collectText[_random.Next(collectText.Length)]}");
            System.Timers.Timer _timer = new();
            RunTimer(_timer, _timerDuration);
            try
            {
                while (true)
                {
                    IMessage message = Prompt(_potatWielder, PromptMode.Any);
                    if (message.MentionedUserIds.Count == 1 && _alive.Contains(message.MentionedUserIds.First()))
                    {
                        _potatWielder = message.MentionedUserIds.First();
                        break;
                    }
                    else if(message.MentionedUserIds.Count == 1 && _dead.Contains(message.MentionedUserIds.First()))
                    {
                        WriteLine(deadText[_random.Next(deadText.Length)]);
                    }
                    else if (message.MentionedUserIds.Count == 1 && message.MentionedUserIds.First() == Program.BotID)
                    {
                        WriteLine("You can't give me the hot potat fool!");
                    }
                    else if (message.MentionedUserIds.Count == 1 && message.MentionedUserIds.First() == _potatWielder)
                    {
                        WriteLine("You can't give yourself the hot potat fool! Thats cheating!");
                    }
                    else if(message.MentionedUserIds.Count == 1)
                    {
                        WriteLine("They are not playing Hot Potat! Now both of you are cringe");
                    }
                    else if (message.MentionedUserIds.Count > 1)
                    {
                        WriteLine("There is only one potat comrade! Try again, better this time");
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                return;
            }
        }
    }
    private void RunTimer(System.Timers.Timer timer, int duration)
    {
        timer = new();
        // Create a timer with a two second interval.
        timer = new(duration * 1000);
        // Hook up the Elapsed event for the timer. 
        timer.Elapsed += OnDetonation;
        timer.AutoReset = false;
        timer.Enabled = true;
        timer.Start();
    }

    private void OnDetonation(object source, ElapsedEventArgs e)
    {
        WriteLine($"{GetPlayer(_potatWielder).Username} {detonationText[_random.Next(detonationText.Length)]}");
        //if (_tooHotTimer != null) _tooHotTimer.Enabled = false;
        //if (_timer != null) _timer.Enabled = false;
        _alive.Remove(_potatWielder);
        _dead.Add(_potatWielder);
        
        Interrupt();
    }

    private void GameOver()
    {
        WriteLine($"{GetPlayer(_alive.First())} has won Hot Potat!");
        WriteLine("\n\n Here's the scores:\n\n");

        foreach(ulong id in _alive)
        {
            WriteLine($"{GetPlayer(id).Username}: {_scores[id]}");
        }

        //WriteLine("__\n\n__");
        bool yn = PromptAnyYN(PromptMode.Any, message: "__\nWould you like to play again?");
        if (yn) RunGame();
        else
        {
            WriteLine("The Game has concluded, thank you for playing comrads.");
            Shutdown();
        }
        
    }
}


