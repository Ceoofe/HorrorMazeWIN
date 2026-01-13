using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static int health = 100;

    public float speed;
    float highSpeed;
    float stamina = 100f;
    float battery = 100f;
    float timer;
    float flashTimer;
    readonly float waitTime = 0.05f;

    GameObject really;
    GameObject cinema;
    GameObject flashLight;
    GameObject barrier;
    GameObject plrUI;
    Transform mainCam;
    Slider staminaBar;
    Slider flashlightBar;
    TMP_Text batteryUI;

    AudioSource audioSource;
    public AudioClip[] clips; // WIP need to change clips to different ones 
    public static bool isCinemaMode = true; // False to skip cutescene
    bool lowFuel = false;
    bool noFuel = false;
    bool rotateCam = false;
    bool isOn = false;
    bool isSprinting = false;
    

    // Start is called before the first frame update
    void Start()
    {
        // Reset 
        isCinemaMode = true;
        lowFuel = false;
        noFuel = false;
        rotateCam = false;
        isOn = false;
        isSprinting = false;
        stamina = 100f;
        battery = 100f;

        GameObject canvas = GameObject.Find("Canvas");

        audioSource = GetComponent<AudioSource>();
        plrUI = canvas.transform.Find("PlayerUI").gameObject;
        staminaBar = plrUI.transform.Find("StaminaBar").GetComponent<Slider>();
        flashlightBar = plrUI.transform.Find("FlashlightBar").GetComponent<Slider>();
        batteryUI = flashlightBar.transform.Find("BatteryLevel").GetComponent<TMP_Text>();

        cinema = canvas.transform.Find("Cinema").gameObject;
        mainCam = transform.Find("Main Camera");
        flashLight = mainCam.Find("Flashlight").gameObject;
        barrier = GameObject.Find("Barrier"); // Barrier for the player to not fall off the map or roaming around somewhere else
        
        highSpeed = Mathf.Pow(speed, 3);

        StartCoroutine("NoFuel"); // Starts cutscene
    }

    // Update is called once per frame
    void Update()
    {
        SprintLogic();
        FlashlightLogic();
    }

    void FixedUpdate()
    {
        if (!isCinemaMode)
        {
            Movement(); // Movement
            return;
        }
        IsCinemaMode();
    }

    IEnumerator NoFuel() // Cutscene
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
        plrUI.SetActive(true);
    }

    void IsCinemaMode()
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
            //Debug.Log(highSpeed); Remove speed by .5 every second
        }
        if (rotateCam)
        {
            mainCam.Rotate(0, 0, 0.1f, Space.World); // Right slant camera
        }
        return;
    }
    void Movement()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * (hor * speed * Time.deltaTime));
        transform.Translate(Vector3.forward * (ver * speed * Time.deltaTime));

        // Walking Sound
        if (hor != 0 || ver != 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(clips[1]);
            }
        }
    }
    void SprintLogic()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift) && !isCinemaMode) // Sprint WIP sprints when player isnt moving
        {
            isSprinting = true;
            speed = 6;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && !isCinemaMode)
        {
            isSprinting = false;
            speed = 3;
        }

        // STAMINA LOGIC
        if (isSprinting) // Decrease stamina when sprinting
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                stamina--;
                staminaBar.gameObject.SetActive(true);
                timer = 0;
            }
            if (stamina <= 1)
            {
                stamina = 0;
                speed = 3;
                isSprinting = false;
            }
        }
        else // Increase stamina when not sprinting
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                stamina++;
                timer = 0;
            }
            if (stamina >= 100)
            {
                stamina = 100;
                staminaBar.gameObject.SetActive(false);
            }
        }
        staminaBar.value = stamina; // Updates Stamina Bar UI
    }
    void FlashlightLogic()
    {
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
        if (isOn == true)
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= (waitTime*100)) // every 5 seconds
            {
                if (battery >= 1)
                {
                    battery--; // Decrease flashlight battery
                }
                batteryUI.text = battery.ToString() + "%";
                flashlightBar.value = battery;
                flashTimer = 0;
            }
        }
    }
}
