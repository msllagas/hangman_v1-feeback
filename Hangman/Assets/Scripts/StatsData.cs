using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsData
{

    public int totalWins;
    public int totalLosses;

    public int gamesPlayed;
    public float winRatio;
    public int fastestTime;

    public int points; // for feedback version only

    public int currentBg; // for feedback version only
    public bool[] bgUnlocked; // for feedback version only
    public bool[] isApplied; // for feedback version only
    public StatsData ( Stats statsdata )
    {
        totalWins = statsdata.totalWins;
        totalLosses = statsdata.totalLosses;
        gamesPlayed = statsdata.gamesPlayed;
        winRatio = statsdata.winRatio;
        fastestTime = statsdata.fastestTime;
        points = statsdata.points; // for feedback version only
        bgUnlocked = statsdata.bgUnlocked; // for feedback version only
        currentBg = statsdata.currentBg; // for feedback version only
        isApplied = statsdata.isApplied; // for feedback version only
    }

}
