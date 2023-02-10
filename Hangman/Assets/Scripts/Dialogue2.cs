using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue2 : MonoBehaviour
{
   
    public TMP_Text DialogHolder;
    public string[] Lines;
    public float TextSpeed;
    public Button PlayButton;
    public GameObject GuidesChild;

    [Header("Animator")]
    public Animator PointDown;
    public Animator PointRight;

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
        if (GuideNavigate.initChild == GuidesChild.transform.childCount - 1)
        {
            NextLine();
        }
    }

    // Function for getting the next dialog in the Lines array
    public void NextDialog()
    {
        // Check if the text already contains the corresponding text in the Lines array
        if (DialogHolder.text == Lines[index])
        {  
            if (GuideNavigate.initChild == GuidesChild.transform.childCount - 1)
            {
                // If it is then call the NextLine() function
                NextLine();
            }
        }
        else
        {
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
        if (index < Lines.Length - 1)
        {
            index++;
            DialogHolder.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        // If there are no more text in the Lines array, the PlayButton will now set to true and tutorial pointer will be disabled
        else
        {
            PlayButton.gameObject.SetActive(true);
            PointDown.gameObject.SetActive(false);
            PointRight.gameObject.SetActive(true);
        }
    }
}
