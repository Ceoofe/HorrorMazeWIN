using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public IEnumerator LoadingScreen(float seconds, int scene, GameObject transition)
    {
        transition.SetActive(true);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(scene);
    }
    public IEnumerator LoadingScreen(float seconds, GameObject transition, GameObject plr, Vector3 pos)
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(seconds);
        transition.SetActive(false);
        plr.transform.position = pos;
    }


}
