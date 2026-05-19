using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = true;

    GameObject message;
    GameObject transition;
    GameObject plr;

    float timer;

    bool isTrigger;

    public enum KeyType { YellowKey, RedKey, BlueKey, GreenKey, PurpleKey , None};
    public KeyType currentKey;

    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        message = canvas.Find("PlayerUI/Message").gameObject;
        transition = canvas.Find("Transition").gameObject;
        plr = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (message.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer >= 1f) // 1 Second delay
            {
                timer = 0;
                message.SetActive(false);
            }
        }

        if (isTrigger && Input.GetKeyDown(KeyCode.E) && !isLocked && PlayerController.item[0] == currentKey.ToString())
        {
            Debug.Log("Unlocked!");
            isTrigger = false;
            StartCoroutine(transition.GetComponent<Transition>().LoadingScreen(1.55f, transition, plr, new Vector3(15f, 1.2f, -8f)));
            PlayerController.item[0] = null;
        }
        else if (isTrigger && Input.GetKeyDown(KeyCode.E))
        {
            message.SetActive(true);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isLocked && PlayerController.item[0] == currentKey.ToString())
        {
            isLocked = false;
            isTrigger = true;
        }
        else if (other.CompareTag("Player") && isLocked)
        {
            isTrigger = true;
            // Message door is locked
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isLocked && PlayerController.item[0] == currentKey.ToString())
        {
            isTrigger = true;
        }
        if (other.CompareTag("Player") && isLocked)
        {
            isTrigger = true;
            // Message door is locked
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isLocked && PlayerController.item[0] == currentKey.ToString())
        {
            isTrigger = false;
        }
        else if (other.CompareTag("Player") && isLocked)
        {
            isTrigger = false;
            // Message door is locked
        }
    }
}
