using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    FlashLight flashLight;
    CameraMovement cam;
    MiniMenu mini;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            flashLight = GameObject.Find("Player").GetComponent<FlashLight>();
            cam = GameObject.Find("Player/Main Camera").GetComponent<CameraMovement>();
            mini = GameObject.Find("Canvas").GetComponent<MiniMenu>();
        }
    }
    public IEnumerator LoadingScreen(float seconds, int scene)
    {
        gameObject.SetActive(true);
        Time.timeScale = 1f;
        // Turn off player movement\
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
        flashLight.enabled = false;
        cam.enabled = false;
        mini.enabled = false;
        PlayerController.isFreezed = true;
        }
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(scene);
    }
    public IEnumerator LoadingScreen(float seconds, GameObject plr, Vector3 pos)
    {
        gameObject.SetActive(true);
        // Turn off player movement
        flashLight.enabled = false;
        cam.enabled = false;
        mini.enabled = false;
        PlayerController.isFreezed = true;
        PlayerController.item[0] = "e"; 
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        flashLight.enabled = true;
        cam.enabled = true;
        mini.enabled = true;
        PlayerController.isFreezed = false;
        plr.transform.position = pos;
        PlayerController.item[0] = "None";
    }


}
