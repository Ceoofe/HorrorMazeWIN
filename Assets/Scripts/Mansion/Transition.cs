using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    PlayerController plrController;
    FlashLight flashLight;
    CameraMovement cam;
    MiniMenu mini;

    void Awake()
    {
        plrController = GameObject.Find("Player").GetComponent<PlayerController>();
        flashLight = GameObject.Find("Player").GetComponent<FlashLight>();
        cam = GameObject.Find("Player/Main Camera").GetComponent<CameraMovement>();
        mini = GameObject.Find("Canvas").GetComponent<MiniMenu>();
    }
    public IEnumerator LoadingScreen(float seconds, int scene, GameObject transition)
    {
        transition.SetActive(true);
        Time.timeScale = 1f;
        // Turn off player movement
        flashLight.enabled = false;
        cam.enabled = false;
        mini.enabled = false;
        plrController.enabled = false;
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(scene);
    }
    public IEnumerator LoadingScreen(float seconds, GameObject transition, GameObject plr, Vector3 pos)
    {
        transition.SetActive(true);
        // Turn off player movement
        flashLight.enabled = false;
        cam.enabled = false;
        mini.enabled = false;
        plrController.enabled = false;
        yield return new WaitForSeconds(seconds);
        transition.SetActive(false);
        flashLight.enabled = true;
        cam.enabled = true;
        mini.enabled = true;
        plrController.enabled = true;
        plr.transform.position = pos;
    }


}
