using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideNavigate : MonoBehaviour
{
    
    public static GuideNavigate instance;
    public static int initChild = 0;


    [Header("BUTTONS")]
    public Button PrevButton;
    public Button NextButton;
    public Button PlayButton;

    [Header("IMAGES")]
    public Image NewPlayerImage;

    // Start is called before the first frame update
    void Start()
    {
        SelectGuide(0);
        CheckNextPrev();
        IdentifyNewPlayer();
    }

    // Event listener for handling the navigation through guide panel
    public void Navigate(int _change)
    {
        initChild += _change;
        StatsData stats = SaveSystem.LoadStats();
        if (initChild > transform.childCount - 1)       
            initChild = 0;     
        else if (initChild < 0)      
            initChild = transform.childCount - 1;
        SaveSystem.SaveStats(stats);
        SelectGuide(initChild);
        CheckNextPrev();
    }

    // Set the current child as active on the UI
    private void SelectGuide(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == _index);
    }

    // Check if the active component is first child or last child of the parent component
    private void CheckNextPrev()
    {
        int children = transform.childCount;

        if (initChild == 0)
        {
            PrevButton.gameObject.SetActive(false);
        }
        else if(initChild == children-1) // Disables the NextButton if the active child component is the last child
        {
            NextButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
        }
        else // Disables the PrevButton if the active child component is the first child
        {
            PrevButton.gameObject.SetActive(true);
            NextButton.gameObject.SetActive(true);
        }
    }

    // Check if the player is a new player or not and display the proper UI components.
    public void IdentifyNewPlayer()
    {
        StatsData statsList = SaveSystem.LoadStats();
        if (statsList.isNewPlayer)
        {
            NewPlayerImage.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);
        }
    }
}
