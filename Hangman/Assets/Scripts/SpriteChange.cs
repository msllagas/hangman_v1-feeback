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

    [Header("Animator")]
    public Animator success;
    public bool KeepAspectRatio;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the SpriteRenderer component attached to the GameObject
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StatsData stats = SaveSystem.LoadStats();
        for (int i = 0; i < stats.isApplied.Length; i++)
        {
            if (stats.isApplied[i] == true)
            {
                // Set the spriteRenderer's sprite to the corresponding background
                spriteRenderer.sprite = newSprite[i];
            }
        }
        SpriteFill();
    }

    // Update is called once per frame
    void Update()
    {   
        SpriteFill();
    }
    public void Apply()
    {
        StatsData stats = SaveSystem.LoadStats();
        // Get the current selected background from the BackgroundSelection instance
        int cur = BackgroundSelection.instance.currentBg;
        // Loop through the isApplied array and set all values to false
        for (int i = 0; i < stats.isApplied.Length; i++)
        {      
                stats.isApplied[i] = false;  
        }
        // Set the current selected background to true in the isApplied array
        stats.isApplied[cur] = true;
        SaveSystem.SaveStats(stats);
        // Update the sprite renderer with the selected background
        spriteRenderer.sprite = newSprite[cur];
        // Trigger the "open" animation if the background has been applied
        success.SetTrigger("open");
    }

    // This method sets the size of the sprite renderer to fill the screen by adjusting its local scale based on the aspect ratio of the screen
    public void SpriteFill()
    {
        // Get the sprite renderer component
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        // Calculate the screen width based on the aspect ratio of the screen
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Set the local scale of the transform to fill the screen
        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);
    }
}
