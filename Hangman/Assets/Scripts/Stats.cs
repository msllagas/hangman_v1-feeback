using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stats
{
    // Stats to be passed on StatsData class constructor
    public int totalWins;
    public int totalLosses;
    public int gamesPlayed;
    public float winRatio;
    public int fastestTime = 9999; 
    public int checker;
    public float motivationLevel;
    public float actualML;
    public float totalML;
    public float centralTend;
    public int points; 

    public int currentBg; 
    public bool[] bgUnlocked = new bool[11] { true, false, false, false, false, false, false, false, false, false, false}; 
    public bool[] isApplied = new bool[11]  { true, false, false, false, false, false, false, false, false, false, false }; 
    public bool isNewPlayer = true;
    public string firstName;
    public string lastName;
    public string fullname;

    // Load the save file, edit it based on the passed arguments, and save all the data back to the file.
    public void SaveStats(bool hasWonGame, float calculatedML, int data, int playtime)
    {
        StatsData statsList = SaveSystem.LoadStats();

        statsList.points += (hasWonGame) ? 5 : 0;
        statsList.totalWins += (hasWonGame) ? 1 : 0;
        statsList.totalLosses += (!hasWonGame) ? 1 : 0;
        statsList.gamesPlayed = statsList.totalLosses + statsList.totalWins;
        statsList.winRatio = (float)Math.Round(((float)statsList.totalWins / statsList.gamesPlayed) * 100, 2);

        statsList.totalML += 3f;
        statsList.actualML += calculatedML;
        statsList.centralTend = (statsList.actualML / statsList.gamesPlayed);

        if (hasWonGame)
        {
            statsList.fastestTime = (playtime >= statsList.fastestTime) ? statsList.fastestTime : playtime;
        }


        statsList.motivationLevel = calculatedML;

        statsList.checker = data;

        SaveSystem.SaveStats(statsList);

    }

    // initially save this class to the file
    public void InitStats()
    {
        SaveSystem.InitSave(this);
    }
}
