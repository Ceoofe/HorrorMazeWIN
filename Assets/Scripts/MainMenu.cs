using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    TMP_Dropdown dropDownFPS;
    TMP_Dropdown dropDownQuality;
    Toggle fullScreen;
    GameObject display;
    GameObject graphics;
    GameObject sounds;
    GameObject controls;
    GameObject transition;
    // Start is called before the first frame update
    void Start()
    {
        Transform panel = transform.Find("SettingsMenu/Panels");

        display = panel.Find("DisplayPanel").gameObject;
        graphics = panel.Find("GraphicsPanel").gameObject;
        sounds = panel.Find("SoundsPanel").gameObject;
        controls = panel.Find("ControlsPanel").gameObject;

        transition = transform.Find("Transition").gameObject;

        dropDownFPS = graphics.transform.Find("LimitFPS").GetComponent<TMP_Dropdown>();
        dropDownQuality = graphics.transform.Find("Graphics").GetComponent<TMP_Dropdown>();
        fullScreen = display.transform.Find("FullScreenToggle").GetComponent<Toggle>();

        LoadLimitFPS();
        LoadQuality();
        LoadIsFullScreen();
    }

    IEnumerator WaitTime(float seconds)
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(1);
    }

    public void Play()
    {
        transition.SetActive(true);
        StartCoroutine(WaitTime(1.55f));
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Credits()
    {

    }

    public void SetLimitFPS() // Sets up the limit fps
    {
        int valueNum = dropDownFPS.value;
        switch (dropDownFPS.value)
        {
            case 0:
            Application.targetFrameRate = 30;
            PlayerPrefs.SetInt("LimitFPS", valueNum);
            break;
            case 1:
            Application.targetFrameRate = 60;
            PlayerPrefs.SetInt("LimitFPS", valueNum);
            break;
            case 2:
            Application.targetFrameRate = 120;
            PlayerPrefs.SetInt("LimitFPS", valueNum);
            break;
            case 3:
            Application.targetFrameRate = 240;
            PlayerPrefs.SetInt("LimitFPS", valueNum);
            break;
            case 4:
            Application.targetFrameRate = -1;
            PlayerPrefs.SetInt("LimitFPS", valueNum);
            break;
        }
        
    }
    void LoadLimitFPS() // loads in limit fps
    {
        if (PlayerPrefs.HasKey("LimitFPS"))
        {
           dropDownFPS.value = PlayerPrefs.GetInt("LimitFPS");
        }
        SetLimitFPS();
    }

    public void SetQuality()
    {
        int valueNum = dropDownQuality.value;
        switch (dropDownQuality.value)
        {
            case 0:
            QualitySettings.SetQualityLevel(0, true);
            PlayerPrefs.SetInt("Quality", valueNum);
            break;
            case 1:
            QualitySettings.SetQualityLevel(1, true);
            PlayerPrefs.SetInt("Quality", valueNum);
            break;
            case 2:
            QualitySettings.SetQualityLevel(2, true);
            PlayerPrefs.SetInt("Quality", valueNum);
            break;
        }
    }
    void LoadQuality()
    {
        if (PlayerPrefs.HasKey("Quality"))
        {
           dropDownQuality.value = PlayerPrefs.GetInt("Quality");
        }
        SetQuality();
    }

    public void SetIsFullScreen()
    {
        Screen.fullScreen = fullScreen.isOn;
        PlayerPrefs.SetInt("FullScreen", fullScreen.isOn ? 1 : 0);
    }
    void LoadIsFullScreen()
    {
        if (PlayerPrefs.HasKey("FullScreen"))
        {
           fullScreen.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        }
        SetIsFullScreen();
    }

    public void ShowPanel(GameObject panel) // Choose one panel to show
    {
        display.SetActive(false);
        graphics.SetActive(false);
        sounds.SetActive(false);
        controls.SetActive(false);

        panel.SetActive(true);
    }


}
