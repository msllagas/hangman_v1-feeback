using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonCreator : MonoBehaviour
{
    public static ButtonCreator instance;

    public TMP_Text hintsLeft;

    public GameObject buttonPrefab;
    string[] letterToUse = new string[26] {"A", "B", "C", "D", "E",
                                            "F", "G", "H", "I", "J",
                                            "K", "L", "M", "N", "O",
                                            "P", "Q", "R", "S", "T",
                                            "U", "V", "W", "X", "Y",
                                            "Z"};
    public Transform buttonHolder;

    List<LetterButton> letterList = new List<LetterButton>();

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PopulateKeyboard();
        HintsLeft();
    }

    // Populate the components that acts as a keyboard with each letters in letterToUse array
    void PopulateKeyboard()
    {
        // Loops through the letterToUse array and create and assign each letter to the component
        for (int i = 0; i < letterToUse.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonHolder, false);
            newButton.GetComponentInChildren<TMP_Text>().text = letterToUse[i];
            LetterButton myLetter = newButton.GetComponent<LetterButton>();
            myLetter.SetButton(letterToUse[i]);

            letterList.Add(myLetter);
        }
    }

    // Remove the passed LetterButton type from the letterList List
    public void RemoveLetter(LetterButton theButton)
    {
        letterList.Remove(theButton);
    }

    // Event listener for the hint button
    public void UseHint()
    {
        if (GameManager.instance.GameOver() || GameManager.instance.maxHints <= 0)
        {
            return;
        } 
        GameManager.instance.maxHints--; 
        // Call the HinstLeft() function to get the remaining hints
        HintsLeft();
        int randomIndex = Random.Range(0, letterList.Count);
        letterList[randomIndex].Sendletter(true);
    } 

    // Update the textfield component with the hints left
    public void HintsLeft()
    {
        hintsLeft.text = "Hint: " + GameManager.instance.maxHints.ToString();
    }
}
