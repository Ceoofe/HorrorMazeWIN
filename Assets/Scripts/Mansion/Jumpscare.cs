using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    PlayerController plrController;
    GameObject zombie;
    GameObject zombieTwo;
    GameObject plr;
    bool isTrigger;
    GameObject interactionUI;

    float timer;
    Vector3 plrforward;
    bool didOnce;
    bool didOnceAnother;
    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;

        zombie = GameObject.Find("MainRoom").transform.Find("skinless zombie").gameObject;
        zombieTwo = GameObject.Find("Basement").transform.Find("skinless zombie").gameObject;
        Transform plrUI = canvas.Find("PlayerUI");
        interactionUI = plrUI.Find("Indicator").gameObject;
        plr = GameObject.Find("Player");
        plrController = GameObject.Find("Player").GetComponent<PlayerController>();

        plrforward = -plr.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger && PlayerController.isPressed)
        {
            zombie.SetActive(true);
            didOnce = true;
            if (!PlayerController.audioSources[0].isPlaying)
            {
                PlayerController.audioSources[0].PlayOneShot(plrController.clips[4]); // change clip to jumpscare sound
            }
        }

        if(zombie.activeSelf)
        {

            timer += Time.deltaTime;
            if (timer >= 1f) // 1 Second delay
            {
                timer = 0;
                zombie.SetActive(false);
                zombie.transform.position = new Vector3(0f, 0.04f, -14f);
            }
        }

        if (PlayerController.item[0] == "BlueKey" && Vector3.Angle(plrforward, plr.transform.forward) > 120f && !didOnceAnother)
        {
            zombieTwo.SetActive(true);
            didOnceAnother = true;
            if (!PlayerController.audioSources[0].isPlaying)
            {
                PlayerController.audioSources[0].PlayOneShot(plrController.clips[4]); // change clip to jumpscare sound
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!didOnce)
        {
            isTrigger = true;
            interactionUI.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        isTrigger = false;
        interactionUI.SetActive(false);
    }
}
