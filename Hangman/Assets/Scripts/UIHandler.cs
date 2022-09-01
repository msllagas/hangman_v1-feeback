using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 39
using TMPro; // 44
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// 35
public class UIHandler : MonoBehaviour
{
    public static UIHandler instance; // 38

    //public Animator firstCloud;
    //public Animator secondCloud;
    public Animator gameOverPanel; // id 1
    public Animator statsPanel; // id 2
    public Animator winPanel; // id 3
    public Animator settingsPanel; // id 4
    [Header("STATS")] // 44
    public TMP_Text statsText; // 44
    public Stats saveFile; // 44
    [Header("AUDIO")]
    public AudioClip winnerSound;
    public AudioClip backgroundSound;
    public AudioClip gameOverSound;
    public AudioClip clickSound;
    public AudioSource audioSource;
    void Awake()
    {
        instance = this;   
    }// 38

    void Start()
    {
        BackGroundMusic();
        InitialSaveFile();
        UpdateStatsText();
    } // 45

    public void SettingsButton() // top-left corner button
    {
        settingsPanel.SetTrigger("open");
    }
    public void StatsButton() // top-left corner button
    {
        statsPanel.SetTrigger("open");
        UpdateStatsText(); // 45
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
            "Total Wins: " + statsList.totalWins + "\n" +
            "Total Losses: " + statsList.totalLosses + "\n" +
            "Total Games Played: " + statsList.gamesPlayed + "\n" +
            "Win Rate: " + statsList.winRatio + "% \n" +
            "Fastest Time: " + statsList.fastestTime + " seconds \n"; 
    } // 45

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
                statsPanel.SetTrigger("close");
                break;
            case 3:
                winPanelTrigger();
                break;
            case 4:
                settingsPanel.SetTrigger("close");
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

    public void WinCondition(int playTime) // could pass in mistakes used and time used
    {
        Stats statwFile = new Stats();
        statwFile.SaveStats(true, playTime); // 44
        winPanel.SetTrigger("open");
        audioSource.Stop();
        if (winnerSound != null)
        {
            audioSource.PlayOneShot(winnerSound, 0.7f);


        }
    }
    public void HintSound()
    {
        audioSource.clip = clickSound;
        audioSource.Play();
    }

    public void LoseCondition(int playTime) // could pass in mistakes used and time used
    {
        Stats statlFile = new Stats();
        statlFile.SaveStats(false, playTime); // 44
        gameOverPanel.SetTrigger("open");
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
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Game");
    }
    public void Menu()
    {
         //firstCloud.SetTrigger("close");
         //secondCloud.SetTrigger("close");
        SceneManager.LoadScene("Game");
        //StartCoroutine(NextLevelAfterWait());
    }

    public void ResetGame()
    {
        // load the current open scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } // 39

    public void ExitGame()
    {
        Application.Quit();
    }// 40
}
