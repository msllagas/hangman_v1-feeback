using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

// Script for selecting background images in the shop panel
public class BackgroundSelection : MonoBehaviour 
{
    [Header("NAVIGATION BUTTONS")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [Header("PLAY/BUY BUTTONS")]
    [SerializeField] private Button apply;
    [SerializeField] private Button buy;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text bgName;

    [Header("BACKGROUND ATTRIBUTES")]
    [SerializeField] private int[] bgPrices;
    public int currentBg;
    public static BackgroundSelection instance;

    [Header("TEXTFIELD")]
    [SerializeField] private TMP_Text pointsText;

    [Header("BACKGROUND NAMES ARRAY")]
    public string[] bgNames;

    // Start is called before the first frame update
    private void Start()
    {
        // Load the saved stats
        StatsData stats = SaveSystem.LoadStats();
        // Set the current background to the value of the loaded current background from the stats. 
        currentBg = stats.currentBg;
        // Call the SelectBg method with the current background as an argument. 
        SelectBg(currentBg);
        UpdateUI(); 
        UpdatePoints();
    }
    public void Awake()
    {
        instance = this;
    }

    // Activates the game object corresponding to the background selected by the player
    private void SelectBg(int _index)
    {
        // Loops through all the children of the transform component, and sets the active state of the child at the specified index to "true"
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == _index);
            
        UpdateUI();
    }

    // Updates the UI for the backgrounds
    private void UpdateUI()
    {
        // Load the stats data from the save file
        StatsData stats = SaveSystem.LoadStats();
        //If currentBg background unlocked show the apply button
        if (stats.bgUnlocked[currentBg])
        {
            apply.gameObject.SetActive(true);
            buy.gameObject.SetActive(false);     
        }
        //If not unlocked, show the "Buy" button and display the price in the text component
        else
        {
            apply.gameObject.SetActive(false);
            buy.gameObject.SetActive(true);
            priceText.text = bgPrices[currentBg] + "";
        }
        //If the current background car is already applied, hide the "Apply" button
        if (stats.isApplied[currentBg])
        {
            apply.gameObject.SetActive(false);
        }

    }

    // Updates the "Buy" button according to the points left
    private void Update()
    {
        StatsData stats = SaveSystem.LoadStats();
        //Check if we have enough plants
        if (buy.gameObject.activeInHierarchy)
            buy.interactable = (stats.points >= bgPrices[currentBg]);     

        bgName.text = bgNames[currentBg] + "";
        UpdateUI();
    }

    // Changes the background when the user clicks the left or right buttons. 
    public void ChangeBg(int _change)
    {
        currentBg += _change;
        StatsData stats = SaveSystem.LoadStats();

        //Check if the current background is outside the bounds of the component's child count
        if (currentBg > transform.childCount - 1)
            // If it is, set the current background to the first background in the component's child
            currentBg = 0;
        else if (currentBg < 0)
            // If it is, set the current background to the last background in the component's child
            currentBg = transform.childCount - 1;

        // Update the current background in the saved stats data
        stats.currentBg = currentBg;
        SaveSystem.SaveStats(stats);

        SelectBg(currentBg);
    }

    // Function to buy a background
    public void BuyBg()
    {
        StatsData stats = SaveSystem.LoadStats();
        // Subtract the price of the current background from the total points
        stats.points -= bgPrices[currentBg];

        // Set the current background to be unlocked
        stats.bgUnlocked[currentBg] = true;
        SaveSystem.SaveStats(stats);

        // Update the UI to reflect the changes
        UpdateUI();

        // Update the points displayed in the UI
        UpdatePoints();
    }

    // Load the points from the save file and update the text that holds the it
    void UpdatePoints()
    {
        StatsData statsList = SaveSystem.LoadStats();
        if (statsList.points >= 1000)
        {
            pointsText.text = "" + System.Math.Round(statsList.points / 1000f, 2) + "K";
        }
        else
        {
            pointsText.text = "" + statsList.points;
        }

    }
}