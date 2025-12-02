using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static int health = 100;

    public float speed;
    float highSpeed;

    GameObject cinema;
    GameObject flashLight;
    Transform mainCam;
    Rigidbody rb;

    public static bool isCinemaMode = true;
    bool lowFuel = false;
    bool noFuel = false;
    bool rotateCam = false;
    bool isOn = false;
    bool isSprinting = false;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine("NoFuel");
        cinema = GameObject.Find("Canvas/Cinema");
        mainCam = transform.Find("Main Camera");
        flashLight = transform.Find("Main Camera/Spot Light").gameObject;
        highSpeed = Mathf.Pow(speed, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCinemaMode) // Sprint
        {
            isSprinting = true;
            speed = 6;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && !isCinemaMode)
        {
            isSprinting = false;
            speed = 3;
        }
        if (Input.GetKeyDown(KeyCode.F) && !isCinemaMode)
        {
            if (isOn)
            {
                flashLight.SetActive(false);
                isOn = false;
            }
            else if (isOn == false)
            {
                flashLight.SetActive(true);
                isOn = true;
            }
        }
    }

    void FixedUpdate() // Movement
    {
        if (isCinemaMode) // No movement
        {
            if (noFuel)
            {
                transform.Translate(Vector3.forward * 10 * Time.deltaTime); // Forward
                transform.Translate(Vector3.right * speed * Time.deltaTime); // Right
            }
            else
            {
                transform.Translate(Vector3.forward * highSpeed * Time.deltaTime); // Forward

                if (lowFuel)
                {
                    highSpeed -= .9f * Time.deltaTime;
                }
                Debug.Log(highSpeed); // Remove speed by .5 every second
            }
            if (rotateCam)
            {
                mainCam.Rotate(0,0,0.1f, Space.World); // Right slant camera
            }
            return;
        }

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * hor * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * ver * speed * Time.deltaTime);
    }

    IEnumerator NoFuel()
    {
        yield return new WaitForSeconds(5f);
        lowFuel = true;
        yield return new WaitForSeconds(18f);
        noFuel = true;
        yield return new WaitForSeconds(1f);
        rotateCam = true;
        yield return new WaitForSeconds(.5f);
        isCinemaMode = false;
        cinema.SetActive(false);
    }
}
