using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public bool isClicked = true;

    [Header("SLIDER")]
    [SerializeField] Slider bgmSlider;

    [Header("Animators")]
    public Animator VolumeButton;

    [Header("Buttons")]
    public Button openButton;
    public Button closeButton;


    // Start is called before the first frame update
    void Start()
    {

        LoadBGMSession();
    }

    public void OpenVolumeClick()
    {

        VolumeButton.SetTrigger("open");
        closeButton.GetComponent<Button>();
        closeButton.gameObject.SetActive(true);

        openButton.gameObject.SetActive(false);


    }
    public void CloseVolumeClick()
    {

        closeButton.gameObject.SetActive(false);

        VolumeButton.SetTrigger("close");
        openButton.gameObject.SetActive(true);


    }

    public void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", bgmSlider.value);
        //Load();
    }
    public void Load()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("musicVolume");
        //AudioListener.volume = bgmSlider.value;
    }
    public void LoadBGMSession()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = bgmSlider.value;
        Save();
    }
}
