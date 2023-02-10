using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    
    public TMP_Text DialogHolder;
    public string[] Lines;
    public float TextSpeed;
    public Image Arrow;
    public Button GuideButton;

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
        StatsData statsList = SaveSystem.LoadStats();
        index = 0;

        // Initializes the first line in the Lines array
        Lines[0] = "Hey there, " +  statsList.firstName.Trim() + "! Nice to meet you!";
        StartCoroutine(TypeLine());
    }

    // Add a typing effect to each letters in the Lines array
    IEnumerator TypeLine()
    {
        foreach (char c in Lines[index].ToCharArray())
        {
            DialogHolder.text += c;
            yield return new WaitForSeconds(TextSpeed);
        }
    }

    // Get the next text in the Lines array
    void NextLine()
    {
        if(index < Lines.Length - 1)
        {
            index++;
            DialogHolder.text = string.Empty;
            StartCoroutine(TypeLine());

            // If the index matches the length of the Lines array, display the arrow that points to the next tutorial phase
            if(index == Lines.Length - 1)
            {
                Arrow.GetComponent<Image>();
                Arrow.gameObject.SetActive(true);
                GuideButton.GetComponent<Button>();
                GuideButton.gameObject.SetActive(true);
            }
        }
    }

}
