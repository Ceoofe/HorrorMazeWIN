using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMenu : MonoBehaviour
{
    PlayerController plrController;
    GameObject menu;
    bool isOn;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        plrController = GameObject.Find("Player").GetComponent<PlayerController>();

        menu = canvas.Find("Menu").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOn == false)
        {
            Time.timeScale = 0f;
            menu.SetActive(true);
            isOn = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Opened");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isOn == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            menu.SetActive(false);
            isOn = false;
            Debug.Log("Closed");
        }

    }

    public void Resume()
    {
        Debug.Log("Closed");
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        menu.SetActive(false);
        isOn = false;
        if (!PlayerController.audioSources[0].isPlaying)
        {
            PlayerController.audioSources[0].PlayOneShot(plrController.clips[0]); // WIP
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
