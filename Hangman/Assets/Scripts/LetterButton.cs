using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 26

public class LetterButton : MonoBehaviour
{
    string letter;
    [SerializeField]
    public AudioSource audioSource;

    // Set button to its correspondiong letter
    public void SetButton(string _letter)
    {
        letter = _letter;
    }

    // Check if a button clicked is hint or letter button
    public void Sendletter(bool isThatAHint)// button input or hint
    {

        GameManager.instance.InputFromButton(letter, isThatAHint); 
        ButtonCreator.instance.RemoveLetter(this); 
        GetComponent<Button>().interactable = false;
    }

    // Play audio clip every time the button is clicked
    public void click()
    {
        audioSource.GetComponent<AudioSource>();
        audioSource.Play();
    }
}
