using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsData
{
    // stats to be saved to the save file
    public int totalWins;
    public int totalLosses;

    public int gamesPlayed;
    public float winRatio;
    public float actualML;
    public float totalML;
    public float centralTend;
    public float motivationLevel;
    public int checker;
    public int fastestTime;
    public int points; 

    public int currentBg; 
    public bool[] bgUnlocked;
    public bool[] isApplied;
    public bool isNewPlayer;
    public string firstName;
    public string lastName;
    public string fullname;

    /*constructor of the StatsData class which take a type Stats parameter 
     * and assign its global variables to the global variables of the passed arguments */
    public StatsData ( Stats statsdata )
    {
        totalWins = statsdata.totalWins;
        totalLosses = statsdata.totalLosses;
        gamesPlayed = statsdata.gamesPlayed;
        winRatio = statsdata.winRatio;
        actualML = statsdata.actualML;
        totalML = statsdata.totalML;
        motivationLevel = statsdata.motivationLevel;
        checker = statsdata.checker;
        fastestTime = statsdata.fastestTime;
        points = statsdata.points; 
        bgUnlocked = statsdata.bgUnlocked; 
        currentBg = statsdata.currentBg; 
        isApplied = statsdata.isApplied; 
        isNewPlayer = statsdata.isNewPlayer;
        firstName = statsdata.firstName;
        lastName = statsdata.lastName;
        fullname = statsdata.fullname;
    }

}
