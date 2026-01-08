using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMenu : MonoBehaviour
{
    GameObject menu;
    bool isOn;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        
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

    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
