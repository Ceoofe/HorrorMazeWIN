using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject message;
    GameObject transition;
    GameObject plr;

    float timer;

    bool isTrigger;

    public enum KeyType { YellowKey, RedKey, BlueKey, GreenKey, PurpleKey , None};
    public KeyType currentKey;

    public Vector3 plrPosition;

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

        if (isTrigger && PlayerController.isPressed && PlayerController.item[0] == currentKey.ToString())
        {
            Debug.Log("Unlocked!");
            PlayerController.isPressed = false;
            isTrigger = false;
            currentKey = KeyType.None;
            PlayerController.item[0] = "None";
            StartCoroutine(transition.GetComponent<Transition>().LoadingScreen(3f, plr, plrPosition));
        }
        else if (isTrigger && PlayerController.isPressed)
        {
            message.SetActive(true);
            PlayerController.isPressed = false;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerController.item[0] == currentKey.ToString())
        {
            isTrigger = true;
        }
        else if (other.CompareTag("Player"))
        {
            isTrigger = true;
            // Message door is locked
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerController.item[0] == currentKey.ToString())
        {
            isTrigger = true;
        }
        if (other.CompareTag("Player"))
        {
            isTrigger = true;
            // Message door is locked
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && PlayerController.item[0] == currentKey.ToString())
        {
            isTrigger = false;
        }
        else if (other.CompareTag("Player"))
        {
            isTrigger = false;
            // Message door is locked
        }
    }
}
