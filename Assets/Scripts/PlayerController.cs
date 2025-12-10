using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static int health = 100;

    public float speed;
    float highSpeed;
    float stamina = 100;

    GameObject cinema;
    GameObject flashLight;
    GameObject barrier;
    Transform mainCam;
    Rigidbody rb;

    AudioSource audioSource;
    public AudioClip[] clips; // WIP need to change clips to different ones 
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
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("NoFuel");
        cinema = GameObject.Find("Canvas/Cinema");
        mainCam = transform.Find("Main Camera");
        flashLight = transform.Find("Main Camera/Flashlight").gameObject;
        barrier = GameObject.Find("Barrier"); // Barrier for the player to not fall off the map or roaming around somewhere else
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

                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(clips[2]);
                }
            }
        }

        if (isSprinting) // Stamina
        {
            stamina -= 20 * Time.deltaTime;
            if(stamina <= 0)
            {
                stamina = 0;
                speed = 3;
                isSprinting = false;
            }
        }
        else
        {
            stamina += 10 * Time.deltaTime;
            if (stamina >= 100)
            {
                stamina = 100;
            }
        }
    }

    void FixedUpdate() // Movement
    {
        IsCinemaMode();
        Movement();
    }

    IEnumerator NoFuel()
    {
        yield return new WaitForSeconds(5f);
        barrier.gameObject.SetActive(false);
        lowFuel = true;
        yield return new WaitForSeconds(18f);
        noFuel = true;
        yield return new WaitForSeconds(1f);
        rotateCam = true;
        yield return new WaitForSeconds(.5f);
        barrier.gameObject.SetActive(true);
        isCinemaMode = false;
        cinema.SetActive(false);
    }

    void IsCinemaMode()
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
                mainCam.Rotate(0, 0, 0.1f, Space.World); // Right slant camera
            }
            return;
        }
    }
    void Movement()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * hor * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * ver * speed * Time.deltaTime);

        // Walking Sound
        if (hor != 0 || ver != 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(clips[1]);
            }
        }
    }
}
