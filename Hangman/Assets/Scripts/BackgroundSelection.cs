using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BackgroundSelection : MonoBehaviour
{
    [Header("Navigation Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [Header("Play/Buy Buttons")]
    [SerializeField] private Button apply;
    [SerializeField] private Button buy;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text bgName;
    [Header("Car Attributes")]
    [SerializeField] private int[] bgPrices;
    public int currentBg;
    public static BackgroundSelection instance;
    /*    [Header("Sound")]
        [SerializeField] private AudioClip purchase;
        private AudioSource source;*/
    public string[] bgNames = new string[4] {"Default", "Background 1", "Background 2", "Background 4" };
    private void Start()
    {
        StatsData stats = SaveSystem.LoadStats();
        currentBg = stats.currentBg;
       /* for (int i = 0; i < stats.isApplied.Length; i++)
        {
            if (stats.isApplied[i])
            {
                SelectCar(i);
            }
        }*/
        SelectCar(currentBg);
        UpdateUI(); //added
    }
    public void Awake()
    {
        instance = this;
    }

    private void SelectCar(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == _index);
            
        UpdateUI();
    }
    private void UpdateUI()
    {
        StatsData stats = SaveSystem.LoadStats();
        //If currentBg car unlocked show the apply button
        if (stats.bgUnlocked[currentBg])
        {
            apply.gameObject.SetActive(true);
            buy.gameObject.SetActive(false);
            //priceText.text = bgPrices[currentBg] + "$"; // just added
        }
        //If not show the buy button and set the price
        else
        {
            apply.gameObject.SetActive(false);
            buy.gameObject.SetActive(true);
            priceText.text = bgPrices[currentBg] + "";

        }

    }

    private void Update()
    {
        //Check if we have enough money
        StatsData stats = SaveSystem.LoadStats();
        if (buy.gameObject.activeInHierarchy)
            buy.interactable = (stats.points >= bgPrices[currentBg]);     

        bgName.text = bgNames[currentBg] + "";
    }

    public void ChangeCar(int _change)
    {
        currentBg += _change;
        StatsData stats = SaveSystem.LoadStats();
        if (currentBg > transform.childCount - 1)
            currentBg = 0;
        else if (currentBg < 0)
            currentBg = transform.childCount - 1;

        stats.currentBg = currentBg;
        SaveSystem.SaveStats(stats);


        SelectCar(currentBg);
        Debug.Log(currentBg);
        Debug.Log(Screen.width + " and" + Screen.height);
    }
    public void BuyCar()
    {
        StatsData stats = SaveSystem.LoadStats();
        stats.points -= bgPrices[currentBg];
        
        stats.bgUnlocked[currentBg] = true;
        SaveSystem.SaveStats(stats);
        //transform.position = new Vector3(0, 0, 0);
        UpdateUI();
    }
}