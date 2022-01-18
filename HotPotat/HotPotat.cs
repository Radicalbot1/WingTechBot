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
    private System.Timers.Timer _timer;
    private ulong _potatWielder;
    private Random _random = new();

    List<ulong> _alive = new();
    List<ulong> _dead = new();
    protected override void Start()
    {
        _timerDuration = Prompt<int>(GamemasterID, AllowedChannels, true, "How long til detonation (in seconds)?");
    }

    public override void RunGame()
    {
        _alive.AddRange(PlayerIDs);

        _potatWielder = _alive[_random.Next(_alive.Count)];
        WriteLine("Start Game!");

        while(_alive.Count > 1)
        {
            RunTimer();
            PlayRound();
            _potatWielder = GetPlayer(_alive[_random.Next(_alive.Count)]).Id;
            WriteLine($"New round everyone!");
        }

        GameOver();
    }

    private void PlayRound()
    {
        while (true)
        {
            WriteLine($"{GetPlayer(_potatWielder)} has the potat!");
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
    private void RunTimer()
    {
        // Create a timer with a two second interval.
        _timer = new(_timerDuration * 1000);
        // Hook up the Elapsed event for the timer. 
        _timer.Elapsed += OnDetonation;
        _timer.AutoReset = false;
        _timer.Enabled = true;
        _timer.Start();
    }

    private void OnDetonation(object source, ElapsedEventArgs e)
    {
        Console.WriteLine("reeee");
        WriteLine($"The Potat has detonated, ending the career of {GetPlayer(_potatWielder).Username}");
        _alive.Remove(_potatWielder);
        _dead.Add(_potatWielder);
        
        Interrupt();
    }

    private void GameOver()
    {
        WriteLine($"{GetPlayer(_alive.First())} has won Hot Potat");
        bool yn = PromptAnyYN(PromptMode.Any, message: "would you like to play again?");
        if (yn) RunGame();
        else Shutdown();
    }
}


