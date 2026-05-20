using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioController : MonoBehaviour
{
    GameObject soundsPanel;
    [SerializeField] AudioMixer audioMixer;
    Slider musicSlider;
    Slider sfxSlider;
    Slider masterSlider;

    // Start is called before the first frame update
    void Start()
    {
        soundsPanel = gameObject.transform.Find("SettingsMenu/Panels/SoundsPanel").gameObject; 
        musicSlider = soundsPanel.transform.Find("Music").GetComponent<Slider>();
        sfxSlider = soundsPanel.transform.Find("SFX").GetComponent<Slider>();
        masterSlider = soundsPanel.transform.Find("Master").GetComponent<Slider>();

        LoadMaster();
    }

    public void SetMaster()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetMusic()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SetSFX()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    void LoadMaster() //WIP
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }
        SetMusic();
        SetSFX();
        SetMaster();
    }
    public void PercentageToText(TMP_Text texts)
    {
        Slider slider;
        switch (texts.name)
        {
            case ("MasterPercent"):
                slider = masterSlider;
                float MasPercent = (slider.value * 100);
                texts.text = ((int)MasPercent).ToString() + "%";
                break;
            case ("MusicPercent"):
                slider = musicSlider;
                float musicPercent = (slider.value * 100);
                texts.text = ((int)musicPercent).ToString() + "%";
                break;
            case ("SFXPercent"):
                slider = sfxSlider;
                float sFXPercent = (slider.value * 100);
                texts.text = ((int)sFXPercent).ToString() + "%";
                break;
        }
    }
}
