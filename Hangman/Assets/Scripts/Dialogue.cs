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

    // Update is called once per frame
    void Update()
    {
    }
    public void NextDialog()
    {
        if (DialogHolder.text == Lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            DialogHolder.text = Lines[index];
        }
    }
    void StartDialogue()
    {
        StatsData statsList = SaveSystem.LoadStats();
        index = 0;
        Lines[0] = "Hey there, " +  statsList.firstName.Trim() + "! Nice to meet you!";
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
    void NextLine()
    {
        if(index < Lines.Length - 1)
        {
            index++;
            DialogHolder.text = string.Empty;
            StartCoroutine(TypeLine());
            if(index == Lines.Length - 1)
            {
                Arrow.GetComponent<Image>();
                Arrow.gameObject.SetActive(true);
                GuideButton.GetComponent<Button>();
                GuideButton.gameObject.SetActive(true);
            }
        }
        else
        {
            /* gameObject.SetActive(false);*/
            /*Arrow.GetComponent<Image>();
            Arrow.gameObject.SetActive(true);
            GuideButton.GetComponent<Button>();
            GuideButton.gameObject.SetActive(true);*/
        }
    }
    /*private string GetFirstName(string fullname)
    {
        string firstName = "";
        
        if (GetWordsCount(fullname) > 2){
            string[] parts = fullname.Trim().Split(' ');
            for (int i = 0; i < parts.Length - 1; i++)
            {
                firstName += parts[i] + ' ';
            }
            return firstName;
        }
        else
            return Regex.Replace(fullname.Trim().Split()[0], @"[^0-9a-zA-Z\ ]+", "");
    }*/
    /*private int GetWordsCount(string wordString)
    {
        string[] parts = wordString.Trim().Split(' ');
        return parts.Length;
    }*/
}
