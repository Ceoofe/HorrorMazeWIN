using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCarController : MonoBehaviour
{
    PlayerController plr;

    GameObject cinema;
    GameObject barrier;
    GameObject cam;
    GameObject leftLight;
    GameObject rightLight;

    Transform plrUI;

    Animator animator;

    MeshRenderer plrMesh;

    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        plrUI = canvas.Find("PlayerUI");
        cinema = canvas.Find("Cinema").gameObject;
        plr = GameObject.Find("Player").GetComponent<PlayerController>();
        plrMesh = GameObject.Find("Player").GetComponent<MeshRenderer>();

        barrier = GameObject.Find("Barrier"); // Barrier for the player to not fall off the map or roaming around somewhere else
        animator = GetComponent<Animator>();
        leftLight = transform.Find("1950sClassicCar#6/S_Main/LeftLight").gameObject;
        rightLight = transform.Find("1950sClassicCar#6/S_Main/RightLight").gameObject;

        cam = transform.Find("CarCamera").gameObject;

        if (SceneManager.GetActiveScene().name == "Game")
        {
            animator.enabled = true;
            StartCoroutine(CarDriving()); // Starts cutscene

            barrier.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PlayerController.isCinemaMode)
        {
            animator.enabled = false;
            leftLight.SetActive(false);
            rightLight.SetActive(false);
            transform.SetPositionAndRotation(new Vector3(0.0052f, 0.92f, -9.005f), Quaternion.Euler(0f, 225f, 0f));
            cam.SetActive(false);
        }
    }
    IEnumerator CarDriving() // Cutscene
    {
        PlayerController.audioSources[1].PlayOneShot(plr.clips[3]); // Play car sound
        plrMesh.enabled = false;
        yield return new WaitForSeconds(25f); // Wait until the cutscene is over
        cam.SetActive(false);
        leftLight.SetActive(false);
        rightLight.SetActive(false);
        plrMesh.enabled = true;
        animator.enabled = false; // disable cutscene animation
        PlayerController.audioSources[1].Stop(); // no car sound
        barrier.SetActive(true);
        PlayerController.isCinemaMode = false;
        cinema.SetActive(false);
        plrUI.gameObject.SetActive(true);
        Destroy(GameObject.Find("Cars"));
        if (!FlashLight.isDone)
        {
            plr.enabled = false;
        }
    }
}
