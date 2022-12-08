using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stats
{
    public int totalWins;
    public int totalLosses;
    public int gamesPlayed;
    public float winRatio;
    public int fastestTime = 9999; // in seconds
    public int checker;
    public float motivationLevel;
    public float actualML;
    public float totalML;
    public float centralTend;
    public int points; // for feedback version only

    public int currentBg; // for feedback version only
    public bool[] bgUnlocked = new bool[11] { true, false, false, false, false, false, false, false, false, false, false}; // for feedback version only
    public bool[] isApplied = new bool[11]  { true, false, false, false, false, false, false, false, false, false, false }; // for feedback version only



    public void SaveStats(bool hasWonGame, bool hasplayedGame, float calculatedML, int data, int playtime)
    {
        StatsData statsList = SaveSystem.LoadStats();

        statsList.points += (hasWonGame) ? 5 : 0;
        statsList.totalWins += (hasWonGame) ? 1 : 0;
        statsList.totalLosses += (!hasWonGame) ? 1 : 0;
        statsList.gamesPlayed = statsList.totalLosses + statsList.totalWins;
        statsList.winRatio = (float)Math.Round(((float)statsList.totalWins / statsList.gamesPlayed) * 100, 2);

        if (hasplayedGame)
        {
            statsList.totalML += 3f;
            statsList.actualML += calculatedML;
            statsList.centralTend = (float)(statsList.actualML / (statsList.gamesPlayed - 1));
        }
        if (hasWonGame)
        {
            statsList.fastestTime = (playtime >= fastestTime) ? fastestTime : playtime;
        }

        statsList.motivationLevel = calculatedML;

        statsList.checker = data;

        SaveSystem.SaveStats(statsList);

        /*EditorUtility.SetDirty(this); // 43
        AssetDatabase.SaveAssets(); // 43*/
    }

    public void InitStats()
    {
        SaveSystem.InitSave(this);
    }
    public void LoadStats()
    {
        StatsData data = SaveSystem.LoadStats();
    }
}
