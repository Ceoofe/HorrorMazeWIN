using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool isGrabbable = false;
    public enum KeyType { YellowKey, RedKey, BlueKey, GreenKey, PurpleKey };
    public KeyType keyType;

    GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        indicator = canvas.Find("PlayerUI/Indicator").gameObject;
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
    }
}
