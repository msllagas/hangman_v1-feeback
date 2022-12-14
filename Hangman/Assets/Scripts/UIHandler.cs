using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 39
using UnityEngine.UI;
using TMPro; // 44
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using Firebase.Database;

// 35
public class UIHandler : MonoBehaviour
{
    public static UIHandler instance; // 38
    private string userID;
    private DatabaseReference dbReference;

    //public Animator firstCloud;
    //public Animator secondCloud;
    public Animator gameOverPanel; // id 1
    public Animator statsPanel; // id 2
    public Animator winPanel; // id 3
    public Animator settingsPanel; // id 4
    public Animator hintPanel;
    public Animator shop; // for feedback version only
    [Space]
    public Animator[] earnPoints; // for feedback version only
    [Header("STATS")] // 44
    public TMP_Text statsText; // 44
/*    public TMP_Text TotalWins;
    public TMP_Text TotalLosses;
    public TMP_Text GamesPlayed;
    public TMP_Text WinRatio;
    public TMP_Text FastestTime;
    [Header("Data Placeholder")]
    public TMP_Text MotivationLevel;
    public TMP_Text AverageMotivationLevel;*/
    public Stats saveFile; // 44
    [Header("POINTS")]
    public TMP_Text pointsText; // for feedback version only
    [Header("AUDIO")]
    public AudioClip winnerSound;
    public AudioClip backgroundSound;
    public AudioClip gameOverSound;
    public AudioClip clickSound;
    public AudioSource audioSource;

    //fuzzy
    [Header("FuzzyLogic")]
    public AnimationCurve TimeonTaskshort;
    public AnimationCurve TimeonTaskmedium;
    public AnimationCurve TimeonTasklong;
    public AnimationCurve NumRepeatTaskLow;
    public AnimationCurve NumRepeatTaskAve;
    public AnimationCurve NumRepeatTaskHigh;
    public AnimationCurve NumHelpRequestlow;
    public AnimationCurve NumHelpRequestmed;
    public AnimationCurve NumHelpRequesthigh;
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

    //motivation level
    float motivationLevel;

    [Header("SLIDER")]
    [SerializeField] Slider bgmSlider;

    public Image victory_losePanel;
    public Image settings_StatsPanel;

    
    public void reducer()
    {
        StatsData statsList = SaveSystem.LoadStats();
        statsList.points -= 5;
        SaveSystem.SaveStats(statsList);
        UpdatePoints(); // for feedback version only
    }

    void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }// 38

    void Start()
    {
        BackGroundMusic();
        InitialSaveFile();
        UpdateStatsText();
        //Load();
        LoadBGMSession();
        UpdatePoints(); // for feedback version only
        //userID = "Mandy => " +  SystemInfo.deviceUniqueIdentifier;
        userID =  SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        CreateUser();

    } // 45
    public void CreateUser()
    {
        StatsData statsList = SaveSystem.LoadStats();
        float motLevPerc = (statsList.motivationLevel / 3) * 100;
        float aveMLPerc = (statsList.centralTend / 3) * 100;
        Player newPlayer = new Player(statsList.fullname, statsList.motivationLevel, statsList.centralTend, motLevPerc, aveMLPerc); // subject to change
        string json = JsonUtility.ToJson(newPlayer);

        dbReference.Child("players").Child(userID).SetRawJsonValueAsync(json);
    }

    public void SettingsButton() // top-left corner button
    {
        settingsPanel.SetTrigger("open");
        //VLPanelEnabler();
        SSPanelEnabler();
    }
    public void StatsButton() // top-left corner button
    {

        UpdateStatsText();
        statsPanel.SetTrigger("open");
        //VLPanelEnabler();
        SSPanelEnabler();
       /* victory_losePanel.GetComponent<Image>();
        victory_losePanel.gameObject.SetActive(true);*/

        // 45
    }

    void InitialSaveFile()
    {
        Stats statFile = new Stats();

        string path = Application.persistentDataPath + "/stats.save";
        if (!File.Exists(path))
        {
            statFile.InitStats();
        }  
    }

    void UpdateStatsText()
    {
        StatsData statsList = SaveSystem.LoadStats();
        /*        statsText.text =
                    "" + statsList.totalWins + "\n" +
                    "" + statsList.totalLosses + "\n" +
                    "" + statsList.gamesPlayed + "\n" +
                    "" + statsList.winRatio + "%\n" +
                    "" + statsList.motivationLevel + "s\n" +
                    "" + statsList.centralTend + "s\n";*/

        statsText.text =
            "" + statsList.totalWins + "\n" +
            "" + statsList.totalLosses + "\n" +
            "" + statsList.gamesPlayed + "\n" +
            "" + statsList.winRatio + "%\n" +
            "" + statsList.fastestTime + "s\n";

/*        MotivationLevel.text = statsList.motivationLevel.ToString();
        AverageMotivationLevel.text = statsList.centralTend.ToString();
        TotalWins.text = statsList.totalWins.ToString();
        TotalLosses.text = statsList.totalLosses.ToString();
        GamesPlayed.text = statsList.gamesPlayed.ToString();
        WinRatio.text = statsList.winRatio.ToString();*/
        //FastestTime.text = statsList.fastestTime.ToString();
    } // 45
    void UpdatePoints()
    {
        StatsData statsList = SaveSystem.LoadStats();
        //pointsText.text = "" + statsList.points;
        if (statsList.points >= 1000)
        {
            pointsText.text = "" + System.Math.Round(statsList.points / 1000f, 2) + "K";
        }
        else
        {
            pointsText.text = "" + statsList.points;
        }

    } // for feedback version only

    void BackGroundMusic()
    {
        audioSource.GetComponent<AudioSource>();
        audioSource.clip = backgroundSound;
        audioSource.loop = true;
        audioSource.Play();
        //audioSource.PlayOneShot(backgroundSound, 0.7f);
        
    }

    public void ClosePanelButton(int buttonId)
    {
        switch (buttonId)
        {
            case 1:
                gameOverPanelTrigger();
                break;
            case 2:
                //statsPanel.SetTrigger("close");
                statsPanelTrigger();
                break;
            case 3:
                winPanelTrigger();
                break;
            case 4:
                settingsPanelTrigger();
                //settingsPanel.SetTrigger("close");
                break;
        }
    }
    public void winPanelTrigger()
    {
        winPanel.SetTrigger("close");
        audioSource.clip = winnerSound;
        audioSource.Stop();
        BackGroundMusic();

    }
    public void gameOverPanelTrigger()
    {
        gameOverPanel.SetTrigger("close");
        audioSource.clip = gameOverSound;
        audioSource.Stop();
        BackGroundMusic();
    }
    public void statsPanelTrigger()
    {
        //victory_losePanel.GetComponent<Image>();
        //victory_losePanel.gameObject.SetActive(false);
        //VLPanelDisabler();
        SSPanelDisabler();
        statsPanel.SetTrigger("close");

    }
    public void settingsPanelTrigger()
    {

        //VLPanelDisabler();
        SSPanelDisabler();
        settingsPanel.SetTrigger("close");
        Save();
    }

    public void WinCondition(int playTime, int remHints, int curMistakes) // could pass in mistakes used and time used
    {
        Stats statsFile = new Stats();

        int usedHints = 3 - remHints;
        int numRepeatTask = nrtEvaluateValue(curMistakes);
        int timeonTask = totEvaluateValue(playTime);
        int numHelpRequest = nhrEvaluateValue(usedHints);

        motivationLevel = rulesEvaluator(timeonTask, numRepeatTask, numHelpRequest);


        statsFile.SaveStats(true, motivationLevel, timeonTask, playTime); // 44

        winPanel.SetTrigger("open");
        VLPanelEnabler();
        audioSource.Stop();
        if (winnerSound != null)
        {
            audioSource.PlayOneShot(winnerSound, 0.7f);
        }
        StartCoroutine(NextLevelAfterWait());
        //earnPoints.SetTrigger("open");
        StartCoroutine(PointsUpdateDelay());
    }
    public void LoseCondition(int playTime, int remHints, int curMistakes) // could pass in mistakes used and time used
    {
        Stats statsFile = new Stats();


        int usedHints = 3 - remHints;
        int numRepeatTask = nrtEvaluateValue(curMistakes);
        int timeonTask = totEvaluateValue(playTime);
        int numHelpRequest = nhrEvaluateValue(usedHints);

        motivationLevel = rulesEvaluator(timeonTask, numRepeatTask, numHelpRequest);
        statsFile.SaveStats(false, motivationLevel, timeonTask, playTime); // 44

        gameOverPanel.SetTrigger("open");
        VLPanelEnabler();
        audioSource.Stop();
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound, 0.7f);


        }
    }

    public void BackToMenu(string levelToLoad)
    {
        
        SceneManager.LoadScene(levelToLoad);

    } // 39

    public IEnumerator NextLevelAfterWait()
    {
        yield return new WaitForSeconds(.5f);

        //SceneManager.LoadScene("Game");
        
        for (int i = 0; i < earnPoints.Length; i++)
        {
            earnPoints[i].SetTrigger("open");
        }
    }
    public IEnumerator PointsUpdateDelay()
    {
        yield return new WaitForSeconds(1.5f);

        //SceneManager.LoadScene("Game");
        UpdatePoints();
    }
    public void Menu()
    {
        SceneManager.LoadScene("Game");
        StatsData statsList = SaveSystem.LoadStats();
        statsList.isNewPlayer = false;
        SaveSystem.SaveStats(statsList);
        //StartCoroutine(NextLevelAfterWait());
    }

    public void ResetGame()
    {
        // load the currentBg open scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } // 39

    public void ExitGame()
    {
        Application.Quit();
    }// 40

    public void VLPanelEnabler()
    {
        victory_losePanel.GetComponent<Image>();
        victory_losePanel.gameObject.SetActive(true);
    }
    public void VLPanelDisabler()
    {

        victory_losePanel.GetComponent<Image>();
        victory_losePanel.gameObject.SetActive(false);
    }
    public void SSPanelEnabler()
    {
        settings_StatsPanel.GetComponent<Image>();
        settings_StatsPanel.gameObject.SetActive(true);
    }
    public void SSPanelDisabler()
    {

        settings_StatsPanel.GetComponent<Image>();
        settings_StatsPanel.gameObject.SetActive(false);
    }
    public void ClickSound()
    {
        audioSource.clip = clickSound;
        audioSource.Play();
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", bgmSlider.value);
        //Load();
    }
    public void Load()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("musicVolume");
        //AudioListener.volume = bgmSlider.value;
    }
    public void LoadBGMSession()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = bgmSlider.value;
        Save();
    }
    public void DefinitionHint()
    {
        hintPanel.SetTrigger("open");
    }
/*    public void Pause()
    {
        GameManager.instance.pause = true;
    }
    public void Play()
    {
        GameManager.instance.pause = false;
    }*/
    public void OpenShop()
    {
        shop.SetTrigger("open");
    } // for feedback version only
    public void CloseShop()
    {
        shop.SetTrigger("close");
    } // for feedback version only


    //TimeonTask
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

    //Mistakes
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

    //sWins
    /*public int perfEvaluateValue(float winratio)
    {
        perfLowValue = NumRepeatTaskLow.Evaluate(winratio);
        perfMedValue = NumRepeatTaskAve.Evaluate(winratio);
        perfHighValue = NumRepeatTaskHigh.Evaluate(winratio);

        if (perfLowValue > perfMedValue && perfLowValue > perfHighValue)
        {
            perfNumber = 1;
        }
        else if (perfMedValue > perfLowValue && perfMedValue > perfHighValue)
        {
            perfNumber = 2;
        }
        else if (perfHighValue > perfLowValue && perfHighValue > perfMedValue)
        {
            perfNumber = 3;
        }
        return perfNumber;
    }*/

    //NumHelpRequest
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
