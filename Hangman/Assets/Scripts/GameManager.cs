using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq; 

public class GameManager : MonoBehaviour
{

    public static GameManager instance; 
    
    List<string> solvedList = new List<string>();
    string[] unsolvedWord;

    [Header("LETTERS")]
    [Space]
    public GameObject letterPrefab;
    public Transform letterHolder;
    List<TMP_Text> letterHolderList = new List<TMP_Text>(); 

    [Header("CATEGORIES")]
    [Space]
    public Category[] categories; 
    public TMP_Text categoryText; 
    public TMP_Text definitionText;
    public TMP_Text correctWord;

    [Header("TIMER")]
    [Space]
    public TMP_Text timerText; 
    int playTime; 
    bool gameOver; 

    [Header("HINTS")] 
    [Space]
    public int maxHints = 3; 

    [Header("PETALS")] 
    [Space]
    public Animator[] petalList;
    [SerializeField]

    [Header("MISTAKES")]
    int maxMistakes;
    int currentMistakes;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxMistakes = petalList.Length;
        Initialize();
        StartCoroutine(Timer()); 
    }

    // Initializes the letters to be solved
    void Initialize()
    {
        // Pick a category first
        int cIndex = Random.Range(0, categories.Length); 
        categoryText.text = categories[cIndex].name;
        int wIndex = Random.Range(0, categories[cIndex].wordList.Length);

        // Pick a word from list or category
        string pickedWord = categories[cIndex].wordList[wIndex]; 

        definitionText.text = categories[cIndex].definition[wIndex];
        correctWord.text = pickedWord;
        // Split the word into single letters
        string[] splittedWord = pickedWord.Select(l => l.ToString()).ToArray(); 
        unsolvedWord = new string[splittedWord.Length]; 
        foreach (string letter in splittedWord) 
        {
            solvedList.Add(letter);
        }
        // Map each letter to the letterHolder game object
        for (int i = 0; i < solvedList.Count; i++) 
        {
            GameObject tempLetter = Instantiate(letterPrefab, letterHolder, false);
            letterHolderList.Add(tempLetter.GetComponent<TMP_Text>());
        }
    }

    // Check if the letter that is clicked exists in the letter to be solved
    public void InputFromButton(string requestedLetter, bool isThatAHint)
    {
        // Search mechanic for solved list
        CheckLetter(requestedLetter, isThatAHint); // 21
    }

    // Check the letter input from the button component
    void CheckLetter(string requestedLetter, bool isThatAHint)
    {
        if (gameOver) 
        {
            return;
        }

        bool letterFound = false; 
        // Find the letter in the solved list
        for (int i = 0; i < solvedList.Count; i++) 
        {
            if (solvedList[i] == requestedLetter)
            {
                letterHolderList[i].text = requestedLetter;
                unsolvedWord[i] = requestedLetter;
                letterFound = true;
            }
        }

        if (!letterFound && !isThatAHint) 
        {
            // If mistake was commited, trigger the "miss" animation on a petal
            petalList[currentMistakes].SetTrigger("miss");
            currentMistakes++;
            // Check if current mistakes is equal to the max mistakes limit
            if (currentMistakes == maxMistakes) 
            {
                UIHandler.instance.LoseCondition(playTime, maxHints, currentMistakes); 
                gameOver = true;
                
                return;
            }

        }

        // Check if game won
        gameOver = CheckIfWon();
        if (gameOver)
        {
            // Get and show the UI associated
            UIHandler.instance.WinCondition(playTime, maxHints, currentMistakes);
        }
    }

    // Returns if a player won or not
    bool CheckIfWon()
    {
        // Check if the word is solved
        for (int i = 0; i < unsolvedWord.Length; i++) 
        {
            if (unsolvedWord[i] != solvedList[i])
            {
                return false;
            }
        }
        return true;
    }

    // Function for the timer for the game
    IEnumerator Timer()
    {
        int seconds = 0;
        int minutes = 0;
        timerText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        while (!gameOver )
        {
            yield return new WaitForSeconds(1);
            playTime++;
            
            seconds = playTime % 60;
            minutes = (playTime / 60) % 60;

            timerText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");

        }
    }

    // Returns true if the game is over, otherwise returns false
    public bool GameOver()
    {
        return gameOver;
    } 

}
