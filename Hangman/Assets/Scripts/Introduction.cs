using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Introduction : MonoBehaviour
{

    public string[] Lines;
    public float TextSpeed;

    [Header("PLAYER NAME PANEL")]
    public Animator PlayerNamePanel;
    public Animator ConfirmationPanel;

    [Header("GAME OBJECT")]
    public GameObject[] GameObjects;

    [Header("IMAGES")]
    public Image SecondDialog;
    public Image Preventer;

    [Header("INPUT FIELD")]
    public TMP_InputField PlayerFirstName;
    public TMP_InputField PlayerLastName;

    [Header("TEXT FIELD")]
    public TMP_Text DialogHolder;
    public TMP_Text ErrorText;
    public TMP_Text ConfirmationText;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        DialogHolder.text = string.Empty;
        StartDialogue();
    }

    // Function for getting the next dialog in the Lines array
    public void NextDialog()
    {
        // Check if the text already contains the corresponding text in the Lines array
        if (DialogHolder.text == Lines[index])
        {
            // If it is then call the NextLine() function
            NextLine();
        }
        else
        {
            // If it is not then set the DialogHolder to the last element of Line array
            StopAllCoroutines();
            DialogHolder.text = Lines[index];
        }
    }

    // Initialize the dialog to be appeared in the component
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in Lines[index].ToCharArray())
        {
            DialogHolder.text += c;
            yield return new WaitForSeconds(TextSpeed);
        }
    }

    // Add a typing effect to each letters in the Lines array
    void NextLine()
    {
        if (index < Lines.Length - 1)
        {
            index++;
            DialogHolder.text = string.Empty;
            StartCoroutine(TypeLine());
            if (index == Lines.Length - 1)
            {
            }
        }
        else
        {
            foreach (GameObject item in GameObjects)
            {
                item.gameObject.SetActive(false);
            }
            PlayerNamePanel.gameObject.SetActive(true);
            PlayerNamePanel.SetTrigger("open");
        }
    }

    // Save the input first name and last name to the save file
    public void Save()
    {
        string firstName = PlayerFirstName.text;
        string lastName = PlayerLastName.text;

        // Check if the textfield is empty
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            // If it is, display error message
            ErrorText.gameObject.SetActive(true);
        }
        else
        {     
            ErrorText.gameObject.SetActive(false);
            Preventer.gameObject.SetActive(true);
            ConfirmationPanel.gameObject.SetActive(true);
            ConfirmationPanel.SetTrigger("open");
            ConfirmationText.text = "Confirm your name " + "\"" + firstName.Trim() + " " + lastName.Trim() + "\"" + "?";
        }

    }

    // Event listener if the player confirms the input first name and last name
    public void Confirm()
    {
        string firstName = PlayerFirstName.text.Trim();
        string lastName = PlayerLastName.text.Trim();
        StatsData statsList = SaveSystem.LoadStats();
        statsList.firstName = firstName;
        statsList.lastName = lastName;
        statsList.fullname = firstName + "_" + lastName;
        SaveSystem.SaveStats(statsList);
        SecondDialog.GetComponent<Image>();
        SecondDialog.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    // Function for editing the input first name and last name
    public void Edit()
    {
        Preventer.gameObject.SetActive(false);
        ConfirmationPanel.SetTrigger("close");
        ConfirmationPanel.gameObject.SetActive(false);
    }
}
