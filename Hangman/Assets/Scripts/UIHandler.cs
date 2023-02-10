using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using TMPro;
using System.IO;
using Firebase.Database;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [Header("ANIMATOR")]
    public Animator gameOverPanel; 
    public Animator statsPanel; 
    public Animator winPanel; 
    public Animator settingsPanel; 
    public Animator hintPanel;
    public Animator shop;

    [Space]
    [Header("ANIMATOR ARRAY")]
    public Animator[] earnPoints; 

    [Header("STATS")] 
    public TMP_Text statsText;
    public Stats saveFile; 

    [Header("POINTS")]
    public TMP_Text pointsText; 

    [Header("AUDIO")]
    public AudioClip winnerSound;
    public AudioClip backgroundSound;
    public AudioClip gameOverSound;
    public AudioClip clickSound;
    public AudioSource audioSource;

    [Header("FUZZY LOGIC")]
    public AnimationCurve TimeonTaskshort;
    public AnimationCurve TimeonTaskmedium;
    public AnimationCurve TimeonTasklong;
    public AnimationCurve NumRepeatTaskLow;
    public AnimationCurve NumRepeatTaskAve;
    public AnimationCurve NumRepeatTaskHigh;
    public AnimationCurve NumHelpRequestlow;
    public AnimationCurve NumHelpRequestmed;
    public AnimationCurve NumHelpRequesthigh;

    // Initialize variables for fuzzy logic inputs
    int totNumber;
    float totshortValue = 0f;
    float totmedValue = 0f;
    float totlongValue = 0f;
    int nrtNumber;
    float nrtLowValue = 0f;
    float nrtMedValue = 0f;
    float nrtHighValue = 0f;
    int nhrNumber;
    float nhrlowValue = 0f;
    float nhrmedValue = 0f;
    float nhrhighValue = 0f;

    float motivationLevel;

    [Header("SLIDER")]
    [SerializeField] Slider bgmSlider;

    [Header("PANEL")]
    public Image victory_losePanel;
    public Image settings_StatsPanel;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        BackGroundMusic();
        InitialSaveFile();
        UpdateStatsText();
        LoadBGMSession();
        UpdatePoints();
    } 

    // Trigger the "open" animation of settings panel and enable the settings image panel
    public void SettingsButton()
    {
        settingsPanel.SetTrigger("open");
        SSPanelEnabler();
    }

    // Trigger the "open" animation of stats panel and enable the settings image panel
    public void StatsButton()
    {
        statsPanel.SetTrigger("open");
        SSPanelEnabler();
    }

    // Check if a save file exist, if not create a save file instead
    void InitialSaveFile()
    {
        Stats statFile = new Stats();

        string path = Application.persistentDataPath + "/stats.save";
        if (!File.Exists(path))
        {
            statFile.InitStats();
        }  
    }

    // Loads the save file and update the stats on the textfield component
    void UpdateStatsText()
    {
        StatsData statsList = SaveSystem.LoadStats();
        statsText.text =
            "" + statsList.totalWins + "\n" +
            "" + statsList.totalLosses + "\n" +
            "" + statsList.gamesPlayed + "\n" +
            "" + statsList.winRatio + "%\n" +
            "" + statsList.fastestTime + "s\n";
    } 

    // Load the save file and update the points on its corresponding component
    void UpdatePoints()
    {
        StatsData statsList = SaveSystem.LoadStats();
        // Check of the points is greater than or equal to 1000
        if (statsList.points >= 1000)
        {
            // If it is, round the points into 2 decimals places and add "K"
            pointsText.text = "" + System.Math.Round(statsList.points / 1000f, 2) + "K";
        }
        else
        {
            pointsText.text = "" + statsList.points;
        }

    }

    // Get the AudioSource component and play its clip on loop
    void BackGroundMusic()
    {
        audioSource.GetComponent<AudioSource>();
        audioSource.clip = backgroundSound;
        audioSource.loop = true;
        audioSource.Play();    
    }

    // Closes the panel according to the passed buttonId
    public void ClosePanelButton(int buttonId)
    {
        switch (buttonId)
        {
            case 1:
                gameOverPanelTrigger();
                break;
            case 2:
                statsPanelTrigger();
                break;
            case 3:
                winPanelTrigger();
                break;
            case 4:
                settingsPanelTrigger();
                break;
        }
    }

    // Trigger the "close" winPanel animation and stop the winnerSound audio clip
    public void winPanelTrigger()
    {
        winPanel.SetTrigger("close");
        audioSource.clip = winnerSound;
        audioSource.Stop();
        BackGroundMusic();
    }

    // Trigger the "close" gameOverPanel animation and stop the gameOverSound audio clip
    public void gameOverPanelTrigger()
    {
        gameOverPanel.SetTrigger("close");
        audioSource.clip = gameOverSound;
        audioSource.Stop();
        BackGroundMusic();
    }

    // Trigger the "close" animation of statsPanel
    public void statsPanelTrigger()
    {
        SSPanelDisabler();
        statsPanel.SetTrigger("close");
    }

    // Trigger the "close" animation of settingsPanel
    public void settingsPanelTrigger()
    {
        SSPanelDisabler();
        settingsPanel.SetTrigger("close");
        Save();
    }

    // Evaluate the player's stats after winning
    public void WinCondition(int playTime, int remHints, int curMistakes) // Could pass in mistakes used and time used
    {
        Stats statsFile = new Stats();

        int usedHints = 3 - remHints;
        int numRepeatTask = nrtEvaluateValue(curMistakes);
        int timeonTask = totEvaluateValue(playTime);
        int numHelpRequest = nhrEvaluateValue(usedHints);

        motivationLevel = rulesEvaluator(timeonTask, numRepeatTask, numHelpRequest);

        // Save the stats into the save file
        statsFile.SaveStats(true, motivationLevel, timeonTask, playTime);

        // Trigger the "open" animation of winPanel
        winPanel.SetTrigger("open");
        VLPanelEnabler();
        audioSource.Stop();
        if (winnerSound != null)
        {
            // Play the winnerSound audio clip once
            audioSource.PlayOneShot(winnerSound, 0.7f);
        }
        StartCoroutine(NextLevelAfterWait());
        StartCoroutine(PointsUpdateDelay());
    }

    // Evaluate the player's stats after lossing
    public void LoseCondition(int playTime, int remHints, int curMistakes) // could pass in mistakes used and time used
    {
        Stats statsFile = new Stats();

        int usedHints = 3 - remHints;
        int numRepeatTask = nrtEvaluateValue(curMistakes);
        int timeonTask = totEvaluateValue(playTime);
        int numHelpRequest = nhrEvaluateValue(usedHints);

        motivationLevel = rulesEvaluator(timeonTask, numRepeatTask, numHelpRequest);

        // Save the stats into the save file
        statsFile.SaveStats(false, motivationLevel, timeonTask, playTime);

        // Trigger the "open" animation of gameOverPanel
        gameOverPanel.SetTrigger("open");
        VLPanelEnabler();
        audioSource.Stop();
        if (gameOverSound != null)
        {
            // Play the gameOverSound audio clip once
            audioSource.PlayOneShot(gameOverSound, 0.7f);
        }
    }

    // Load the Main Menu game scene
    public void BackToMenu(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    } 

    // Delay the animation for earning points
    public IEnumerator NextLevelAfterWait()
    {
        yield return new WaitForSeconds(.5f);
        
        for (int i = 0; i < earnPoints.Length; i++)
        {
            earnPoints[i].SetTrigger("open");
        }
    }

    // Delay the animation for updating points
    public IEnumerator PointsUpdateDelay()
    {
        yield return new WaitForSeconds(1.5f);
        UpdatePoints();
    }

    // Load the Game Scene and set the isNewPlayer stats to false
    public void Menu()
    {
        SceneManager.LoadScene("Game");
        StatsData statsList = SaveSystem.LoadStats();
        statsList.isNewPlayer = false;
        SaveSystem.SaveStats(statsList);
    }

    // Reload and resets the game scene
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 

    // Quit the game itself
    public void ExitGame()
    {
        Application.Quit();
    }

    // Activate a panel (panel that pop up with winning or lossing the game) that disables
    // user interaction to other components on the background
    public void VLPanelEnabler()
    {
        victory_losePanel.GetComponent<Image>();
        victory_losePanel.gameObject.SetActive(true);
    }

    // Deactivate a panel (panel that pop up with winning or lossing the game) that disables
    // user interaction to other components on the background
    public void VLPanelDisabler()
    {

        victory_losePanel.GetComponent<Image>();
        victory_losePanel.gameObject.SetActive(false);
    }

    // Activate a panel (panel that pop up with opening stats or settings panel) that disables
    // user interaction to other components on the background
    public void SSPanelEnabler()
    {
        settings_StatsPanel.GetComponent<Image>();
        settings_StatsPanel.gameObject.SetActive(true);
    }

    // Deactivate a panel (panel that pop up with opening stats or settings panel) that disables
    // user interaction to other components on the background
    public void SSPanelDisabler()
    {
        settings_StatsPanel.GetComponent<Image>();
        settings_StatsPanel.gameObject.SetActive(false);
    }

    // Play the clickSound every time a corresponding component is clicked
    public void ClickSound()
    {
        audioSource.clip = clickSound;
        audioSource.Play();
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

    // Trigger the "open" shop panel animation
    public void OpenShop()
    {
        shop.SetTrigger("open");
    }

    // Update points text and trigger the "close" shop panel animatin
    public void CloseShop()
    {
        UpdatePoints();
        shop.SetTrigger("close");
    } 

    // Function that calculates TimeonTask for Fuzzy Logic
    public int totEvaluateValue(int time)
    {
        totshortValue = TimeonTaskshort.Evaluate(time);
        totmedValue = TimeonTaskmedium.Evaluate(time);
        totlongValue = TimeonTasklong.Evaluate(time);

        if (totshortValue > totmedValue && totshortValue > totlongValue)
        {
            totNumber = 3;
        }
        else if (totmedValue > totshortValue && totmedValue > totlongValue)
        {
            totNumber = 2;
        }
        else if (totlongValue > totshortValue && totlongValue > totmedValue)
        {
            totNumber = 1;
        }
        return totNumber;
    }

    // Function that calculates Mistakes for Fuzzy Logix
    public int nrtEvaluateValue(int mistakes)
    {
        nrtLowValue = NumRepeatTaskLow.Evaluate(mistakes);
        nrtMedValue = NumRepeatTaskAve.Evaluate(mistakes);
        nrtHighValue = NumRepeatTaskHigh.Evaluate(mistakes);

        if (nrtLowValue > nrtMedValue && nrtLowValue > nrtHighValue)
        {
            nrtNumber = 3;
        }
        else if (nrtMedValue > nrtLowValue && nrtMedValue > nrtHighValue)
        {
            nrtNumber = 2;
        }
        else if (nrtHighValue > nrtLowValue && nrtHighValue > nrtMedValue)
        {
            nrtNumber = 1;
        }
        return nrtNumber;
    }

    //Function that calculates NumHelpRequest for Fuzzy Logic
    public int nhrEvaluateValue(int usedHints)
    {
        nhrlowValue = NumHelpRequestlow.Evaluate(usedHints);
        nhrmedValue = NumHelpRequestmed.Evaluate(usedHints);
        nhrhighValue = NumHelpRequesthigh.Evaluate(usedHints);

        if (nhrlowValue > nhrmedValue && nhrlowValue > nhrhighValue)
        {
            nhrNumber = 3;
        }
        else if (nhrmedValue > nhrlowValue && nhrmedValue > nhrhighValue)
        {
            nhrNumber = 2;
        }
        else if (nhrhighValue > nhrlowValue && nhrhighValue > nhrmedValue)
        {
            nhrNumber = 1;
        }
        return nhrNumber;
    }

    // Function to get motivation level according to the passed arguments
    public float rulesEvaluator(int TimeonTask, int NumRepeatTask, int NumHelpRequest)
    {
        //1
        if (TimeonTask == 3 && NumRepeatTask == 1 && NumHelpRequest == 1)
        {
            motivationLevel = 1.66f;
        }
        //2
        else if (TimeonTask == 3 && NumRepeatTask == 1 && NumHelpRequest == 2)
        {
            motivationLevel = 2f;
        }
        //3
        else if (TimeonTask == 3 && NumRepeatTask == 1 && NumHelpRequest == 3)
        {
            motivationLevel = 2.33f;
        }
        //4
        else if (TimeonTask == 3 && NumRepeatTask == 2 && NumHelpRequest == 1)
        {
            motivationLevel = 2f;
        }
        //5
        else if (TimeonTask == 3 && NumRepeatTask == 2 && NumHelpRequest == 2)
        {
            motivationLevel = 2.33f;
        }
        //6
        else if (TimeonTask == 3 && NumRepeatTask == 2 && NumHelpRequest == 3)
        {
            motivationLevel = 2.66f;
        }
        //7
        else if (TimeonTask == 3 && NumRepeatTask == 3 && NumHelpRequest == 1)
        {
            motivationLevel = 2.33f;
        }
        //8
        else if (TimeonTask == 3 && NumRepeatTask == 3 && NumHelpRequest == 2)
        {
            motivationLevel = 2.66f;
        }
        //9
        else if (TimeonTask == 3 && NumRepeatTask == 3 && NumHelpRequest == 3)
        {
            motivationLevel = 3f;
        }
        //10
        else if (TimeonTask == 2 && NumRepeatTask == 1 && NumHelpRequest == 1)
        {
            motivationLevel = 1.33f;
        }
        //11
        else if (TimeonTask == 2 && NumRepeatTask == 1 && NumHelpRequest == 2)
        {
            motivationLevel = 1.66f;
        }
        //12
        else if (TimeonTask == 2 && NumRepeatTask == 1 && NumHelpRequest == 3)
        {
            motivationLevel = 2f;
        }
        //13
        else if (TimeonTask == 2 && NumRepeatTask == 2 && NumHelpRequest == 1)
        {
            motivationLevel = 1.66f;
        }
        //14
        else if (TimeonTask == 2 && NumRepeatTask == 2 && NumHelpRequest == 2)
        {
            motivationLevel = 2f;
        }
        //15
        else if (TimeonTask == 2 && NumRepeatTask == 2 && NumHelpRequest == 3)
        {
            motivationLevel = 2.33f;
        }
        //16
        else if (TimeonTask == 2 && NumRepeatTask == 3 && NumHelpRequest == 1)
        {
            motivationLevel = 2f;
        }
        //17
        else if (TimeonTask == 2 && NumRepeatTask == 3 && NumHelpRequest == 2)
        {
            motivationLevel = 2.33f;
        }
        //18
        else if (TimeonTask == 2 && NumRepeatTask == 3 && NumHelpRequest == 3)
        {
            motivationLevel = 2.66f;
        }
        //19
        else if (TimeonTask == 1 && NumRepeatTask == 1 && NumHelpRequest == 1)
        {
            motivationLevel = 1f;
        }
        //20
        else if (TimeonTask == 1 && NumRepeatTask == 1 && NumHelpRequest == 2)
        {
            motivationLevel = 1.33f;
        }
        //21
        else if (TimeonTask == 1 && NumRepeatTask == 1 && NumHelpRequest == 3)
        {
            motivationLevel = 1.66f;
        }
        //22
        else if (TimeonTask == 1 && NumRepeatTask == 2 && NumHelpRequest == 1)
        {
            motivationLevel = 1.33f;
        }
        //23
        else if (TimeonTask == 1 && NumRepeatTask == 2 && NumHelpRequest == 2)
        {
            motivationLevel = 1.66f;
        }
        //24
        else if (TimeonTask == 1 && NumRepeatTask == 2 && NumHelpRequest == 3)
        {
            motivationLevel = 2f;
        }
        //25
        else if (TimeonTask == 1 && NumRepeatTask == 3 && NumHelpRequest == 1)
        {
            motivationLevel = 1.66f;
        }
        //26
        else if (TimeonTask == 1 && NumRepeatTask == 3 && NumHelpRequest == 2)
        {
            motivationLevel = 2f;
        }
        //27
        else if (TimeonTask == 1 && NumRepeatTask == 3 && NumHelpRequest == 3)
        {
            motivationLevel = 2.33f;
        }



        return motivationLevel;
    }
}
