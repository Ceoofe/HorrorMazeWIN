using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    PlayerController plrController;
    GameObject image;
    bool isTrigger;
    GameObject interactionUI;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;

        image = GameObject.Find("Canvas").transform.Find("Jumpscare").gameObject;
        Transform plrUI = canvas.Find("PlayerUI");
        interactionUI = plrUI.Find("Indicator").gameObject;
        plrController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger && PlayerController.isPressed)
        {
            image.SetActive(true);
            if (!PlayerController.audioSources[0].isPlaying)
            {
                PlayerController.audioSources[0].PlayOneShot(plrController.clips[0]); // change clip to jumpscare sound
            }
        }

        if(image.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer >= 2f) // 2 Second delay
            {
                timer = 0;
                image.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isTrigger = true;
        interactionUI.SetActive(true);
    }
    void OnTriggerExit(Collider other)
    {
        isTrigger = false;
        interactionUI.SetActive(false);
    }
}
