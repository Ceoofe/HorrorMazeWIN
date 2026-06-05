using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool isGrabbable = false;
    public enum KeyType { YellowKey, RedKey, BlueKey, GreenKey, PurpleKey };
    public KeyType keyType;

    GameObject indicator;

    Transform plrUI;
    TMP_Text objectives;

    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        indicator = canvas.Find("PlayerUI/Indicator").gameObject;
        plrUI = canvas.Find("PlayerUI");
        objectives = plrUI.Find("ObjectiveUI/Objectives").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbable && Input.GetKeyDown(KeyCode.E))
        {
            PlayerController.item[0] = keyType.ToString();
            Debug.Log("Obtained " + keyType.ToString() + "!");
            indicator.SetActive(false);
            isGrabbable = false;
            Destroy(gameObject);
        }

        if (PlayerController.item[0] == "BlueKey" && PlayerController.isPressed)
        {
            StartCoroutine(plrUI.transform.Find("ObjectiveUI").GetComponent<Objectives>().NewObjective(objectives, "", 1));
            StartCoroutine(plrUI.transform.Find("ObjectiveUI").GetComponent<Objectives>().NewObjective(objectives, "", 2));
            StartCoroutine(plrUI.transform.Find("ObjectiveUI").GetComponent<Objectives>().NewObjective(objectives, "•Escape the mansion", 2));
        }
    }
}
