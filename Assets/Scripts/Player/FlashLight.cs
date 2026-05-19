using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    PlayerController plrController;

    bool isOn = false;
    bool isDone = false;

    float flashTimer;
    readonly float waitTime = 0.05f;
    public static float battery = 100f;

    TMP_Text batteryUI;
    TMP_Text objectives;
    Slider flashlightBar;

    GameObject flashLight;
    // Start is called before the first frame update
    void Start()
    {
        plrController = GetComponent<PlayerController>();

        flashLight = transform.Find("Main Camera/Flashlight").gameObject;

        Transform plrUI = GameObject.Find("Canvas").transform.Find("PlayerUI").transform;
        flashlightBar = plrUI.Find("FlashlightBar").GetComponent<Slider>();
        batteryUI = flashlightBar.transform.Find("BatteryLevel").GetComponent<TMP_Text>();
        objectives = plrUI.Find("ObjectiveUI/Objectives").GetComponent<TMP_Text>();


        // Reload values
        flashlightBar.value = battery;
        batteryUI.text = battery.ToString() + "%";

        if (SceneManager.GetActiveScene().name == "Game")
        {
            isOn = false;
            battery = 100f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !PlayerController.isCinemaMode)
        {
            if (!isDone) // The first Objective
            {
                StartCoroutine(plrController.NewObjective(objectives, "", 2));
                isDone = true;
            }

            if (isOn)
            {
                flashLight.SetActive(false);
                isOn = false;
            }
            else if (isOn == false)
            {
                flashLight.SetActive(true);
                isOn = true;

                if (!PlayerController.audioSources[0].isPlaying)
                {
                    PlayerController.audioSources[0].PlayOneShot(plrController.clips[2]);
                }
            }
        }

        if (isOn == true)
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= (waitTime * 100)) // every 5 seconds
            {
                if (battery >= 1)
                {
                    battery--; // Decrease flashlight battery
                }
                batteryUI.text = battery.ToString() + "%";
                flashlightBar.value = battery;
                flashTimer = 0;
            }
        }
    }
}
