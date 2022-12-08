using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
  
    public float motivationLevel;
    public float averageMotivationLevel;
    public float motLevPerc;
    public float aveMLPerc;

    /*    public Player(int totalWins, int totalLosses, int gamesPlayed, float winRatio)
        {
            this.totalWins = totalWins;
            this.totalLosses = totalLosses;
            this.gamesPlayed = gamesPlayed;
            this.winRatio = winRatio;
        }*/

    public Player(float motivationLevel, float averageMotivationLevel, float motLevPerc, float aveMLPerc)
    {
        this.motivationLevel = motivationLevel;
        this.averageMotivationLevel = averageMotivationLevel;
        this.motLevPerc = motLevPerc;
        this.aveMLPerc = aveMLPerc;
    }


}
