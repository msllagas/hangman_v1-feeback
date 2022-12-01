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
    public TMP_Text TotalWins;
    public TMP_Text TotalLosses;
    public TMP_Text GamesPlayed;
    public TMP_Text WinRatio;
    public TMP_Text FastestTime;
    public Stats saveFile; // 44
    [Header("POINTS")]
    public TMP_Text pointsText; // for feedback version only
    [Header("AUDIO")]
    public AudioClip winnerSound;
    public AudioClip backgroundSound;
    public AudioClip gameOverSound;
    public AudioClip clickSound;
    public AudioSource audioSource;

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
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        CreateUser();

    } // 45
    public void CreateUser()
    {
        Player newPlayer = new Player(int.Parse(TotalWins.text), int.Parse(TotalLosses.text), int.Parse(GamesPlayed.text), float.Parse(WinRatio.text), int.Parse(FastestTime.text));
        //User newUser = new User(1, 2, 3, 4f, 5);
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
        statsText.text =
            "" + statsList.totalWins + "\n" +
            "" + statsList.totalLosses + "\n" +
            "" + statsList.gamesPlayed + "\n" +
            "" + statsList.winRatio + "%\n" +
            "" + statsList.fastestTime + "s\n";

        TotalWins.text = statsList.totalWins.ToString();
        TotalLosses.text = statsList.totalLosses.ToString();
        GamesPlayed.text = statsList.gamesPlayed.ToString();
        WinRatio.text = statsList.winRatio.ToString();
        FastestTime.text = statsList.fastestTime.ToString();
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

    public void WinCondition(int playTime) // could pass in mistakes used and time used
    {
        Stats statsFile = new Stats();
        statsFile.SaveStats(true, playTime); // 44
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
    public void LoseCondition(int playTime) // could pass in mistakes used and time used
    {
        Stats statsFile = new Stats();
        statsFile.SaveStats(false, playTime); // 44
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
}
