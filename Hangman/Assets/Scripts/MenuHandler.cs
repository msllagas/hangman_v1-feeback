using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class MenuHandler : MonoBehaviour
{
    // Creates a field in the unity engine with their names and corresponding UI component types
    public bool isClicked = true;

    [Header("SLIDER")]
    [SerializeField] Slider bgmSlider;

    [Header("ANIMATORS")]
    public Animator AnimateStats;
    public Animator AnimateSettings;
    public Animator AnimateGuide;

    [Header("GAMEOBJECT PANEL")]
    public GameObject GuideObj;
    public GameObject GuidePanel;

    [Header("BUTTONS")]
    public Button StatsButton;
    public Button SettingsButton;
    public Button GuideButton;
    public Button SaveButton;

    [Header("PANEL")]
    public Image OpenGuidePanel;
    public Image IntroDialog;
    public Image SecondDialog;

    [Header("STATS")]
    public TMP_Text statsText;
    public Stats saveFile;



    // Start is called before the first frame update
    void Start()
    {
        InitialSaveFile();
        UpdateStatsText();
        LoadBGMSession();
        IdentifyNewPlayer();
    }

    //  Initialize a save file for stats data
    void InitialSaveFile()
    {
        // Create an instance of the Stats class
        Stats statFile = new Stats();

        // Define the path to the stats save file
        string path = Application.persistentDataPath + "/stats.save";

        // Check if the stats save file already exists
        if (!File.Exists(path))
        {
            // If the file does not exist, initialize the stats data
            statFile.InitStats();
        }
    }

    // Update the text displayed for the stats data
    void UpdateStatsText()
    {
        // Load the stats data from the save file
        StatsData statsList = SaveSystem.LoadStats();

        // Set the text of the statsText object to display the stats data
        statsText.text =
            "" + statsList.totalWins + "\n" +
            "" + statsList.totalLosses + "\n" +
            "" + statsList.gamesPlayed + "\n" +
            "" + statsList.winRatio + "%\n" +
            "" + statsList.fastestTime + "s\n";
    }

    // Open the settings panel
    public void OpenSettings()
    {
        // Trigger the "open" animation for the settings panel
        AnimateSettings.SetTrigger("open");

        // Disable the settings button
        SettingsButton.GetComponent<Button>();
        SettingsButton.enabled = !SettingsButton.enabled;

        // Disable the stats button
        StatsButton.GetComponent<Button>();
        StatsButton.enabled = !StatsButton.enabled;
    }

    // Close the settings panel
    public void CloseSettings()
    {
        // Trigger the "close" animation for the settings panel
        AnimateSettings.SetTrigger("close");

        // Enable the settings button
        SettingsButton.GetComponent<Button>();
        SettingsButton.enabled = true;

        // Enable the stats button
        StatsButton.enabled = true;
    }

    // Open the stats panel
    public void OpenStats()
    {
        // Trigger the open animation of the stats panel
        AnimateStats.SetTrigger("open");

        // Disable the stats button to prevent multiple clicks
        StatsButton.GetComponent<Button>();
        StatsButton.enabled = !StatsButton.enabled;

        // Disable the settings button to prevent multiple clicks
        SettingsButton.GetComponent<Button>();
        SettingsButton.enabled = !SettingsButton.enabled;
    }

    // Close the stats panel
    public void CloseStats()
    {
        // Trigger the close animation of the stats panel
        AnimateStats.SetTrigger("close");

        // Re-enable the stats button
        StatsButton.GetComponent<Button>();
        StatsButton.enabled = true;

        // Re-enable the settings button
        SettingsButton.enabled = true;
    }

    // Open the guide panel and if the player is new, a dialogue will pop-up
    public void OpenGuide()
    {
        // Enable the guide panel game object
        GuidePanel.gameObject.SetActive(true);

        // Trigger the open animation of the guide panel
        AnimateGuide.SetTrigger("open");

        // Call the GuidePanelImgEnabler() to enable an image that prevents user from clicking in other elements
        GuidePanelImgEnabler();

        // Load the stats data from the save file
        StatsData statsList = SaveSystem.LoadStats();

        // Check if the player is a new player
        if (statsList.isNewPlayer)
        {
            SecondDialog.GetComponent<Image>();
            SecondDialog.gameObject.SetActive(false);
        }
    }

    // Closes the guide panel
    public void CloseGuide()
    {
        AnimateGuide.SetTrigger("close");
        GuidePanelImgDisabler();
    }

    // Opens a panel that prevents users from interacting with components other than the intended components to be interacted with
    public void GuidePanelImgEnabler()
    {
        OpenGuidePanel.GetComponent<Image>();
        OpenGuidePanel.gameObject.SetActive(true);
    }

    // Closes a panel that prevents users from interacting with components other than the intended components to be interacted with
    public void GuidePanelImgDisabler()
    {
        OpenGuidePanel.GetComponent<Image>();
        OpenGuidePanel.gameObject.SetActive(false);
    }

    // Check if the player is a new player or not and display the proper UI components.
    public void IdentifyNewPlayer()
    {
        StatsData statsList = SaveSystem.LoadStats();
        if (statsList.isNewPlayer)
        {
            IntroDialog.GetComponent<Image>();
            IntroDialog.gameObject.SetActive(true);
            GuideButton.GetComponent<Button>();
            GuideButton.gameObject.SetActive(false);
        }
    }

    // Saves the volume level of the background music
    public void Save()
    {
        // Set the "musicVolume" key in PlayerPrefs to the value of the bgmSlider
        PlayerPrefs.SetFloat("musicVolume", bgmSlider.value);
    }

    // Loads the saved volume value for background music
    public void Load()
    {
        // Retrieve the value of background music volume from PlayerPrefs
        bgmSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    // Checks if there is an existing playerprefs for background music
    public void LoadBGMSession()
    {
        // Check if "musicVolume" key exists in PlayerPrefs
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            // If "musicVolume" key does not exist, set its value to 1 and load it
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    // Changes the volume according to the value of bgmSlider
    public void ChangeVolume()
    {
        AudioListener.volume = bgmSlider.value;
        Save();
    }
}
