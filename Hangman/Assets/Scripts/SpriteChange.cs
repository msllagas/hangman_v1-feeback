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

    //public static Sprite savedSprite = null; //added
    public bool KeepAspectRatio;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //load the sprite
        StatsData stats = SaveSystem.LoadStats();
        for (int i = 0; i < stats.isApplied.Length; i++)
        {
            if (stats.isApplied[i] == true)
            {
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
        int cur = BackgroundSelection.instance.currentBg;
        for (int i = 0; i < stats.isApplied.Length; i++)
        {      
                stats.isApplied[i] = false;  
        }
        stats.isApplied[cur] = true;
        SaveSystem.SaveStats(stats);
        spriteRenderer.sprite = newSprite[cur];

    }
    public void SpriteFill()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);
    }
}
