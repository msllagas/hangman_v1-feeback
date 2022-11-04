using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 39
using UnityEngine.UI;
using TMPro; // 44
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SpriteChange : MonoBehaviour
{
    [Header("Sprite Change")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] newSprite;

    public static Sprite savedSprite = null; //added
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (savedSprite != null)
        {
            spriteRenderer.sprite = savedSprite;
        } // added

        //load the sprite
        spriteRenderer.sprite = newSprite[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSprite()
    {
        savedSprite = newSprite[0]; //added
        spriteRenderer.sprite = newSprite[0];
    }
    public void ChangeSpriteAgain()
    {
        savedSprite = newSprite[1]; //added
        spriteRenderer.sprite = newSprite[1];
    }
}
