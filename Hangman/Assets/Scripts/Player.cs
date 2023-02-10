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
    public string fullName;

    //Constructor that sets the values for the player
    public Player(string fullName, float motivationLevel, float averageMotivationLevel, float motLevPerc, float aveMLPerc)
    {
        // Assigns the passed arguments to its corresponding fields
        this.motivationLevel = motivationLevel;
        this.averageMotivationLevel = averageMotivationLevel;
        this.motLevPerc = motLevPerc;
        this.aveMLPerc = aveMLPerc;
        this.fullName = fullName;
    }


}
