using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    // Find a way to save the data from one scene to another
    public float speed;
    float highSpeed;
    float stamina = 100f;
    float battery = 100f;
    float timer;
    float flashTimer;
    readonly float waitTime = 0.05f;

    GameObject cinema;
    GameObject flashLight;
    GameObject barrier;
    GameObject plrUI;
    GameObject interactionUI;
    Transform mainCam;
    Slider staminaBar;
    Slider flashlightBar;
    TMP_Text batteryUI;
    TMP_Text objectives;

    AudioSource[] audioSources;
    public AudioClip[] clips;
    public static bool isCinemaMode; // False to skip cutescene
    bool lowFuel = false;
    bool noFuel = false;
    bool rotateCam = false;
    bool isOn = false;
    bool isSprinting = false;
    bool isNearMainDoor = false;

    public enum GameState { Highway, Mansion }
    public GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");

        audioSources = GetComponents<AudioSource>();
        plrUI = canvas.transform.Find("PlayerUI").gameObject;
        staminaBar = plrUI.transform.Find("StaminaBar").GetComponent<Slider>();
        flashlightBar = plrUI.transform.Find("FlashlightBar").GetComponent<Slider>();
        batteryUI = flashlightBar.transform.Find("BatteryLevel").GetComponent<TMP_Text>();
        objectives = plrUI.transform.Find("ObjectiveUI/Objectives").GetComponent<TMP_Text>();
        interactionUI = plrUI.transform.Find("Indicator").gameObject;

        cinema = canvas.transform.Find("Cinema").gameObject;
        mainCam = transform.Find("Main Camera");
        flashLight = mainCam.Find("Flashlight").gameObject;
        barrier = GameObject.Find("Barrier"); // Barrier for the player to not fall off the map or roaming around somewhere else
        
        highSpeed = Mathf.Pow(speed, 3);

        if (currentState == GameState.Highway)
        {
            StartCoroutine(NoFuel()); // Starts cutscene

            // Reset
            isCinemaMode = true;
            lowFuel = false;
            noFuel = false;
            rotateCam = false;
            isOn = false;
            isSprinting = false;
            stamina = 100f;
            battery = 100f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SprintLogic();
        FlashlightLogic();
        InteractionLogic();
        SkipCutScene();
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

    void OnTriggerEnter(Collider other) 
    {
        if (other.name == "Trigger")
        {
            StartCoroutine(NewObjective(objectives, "• Enter the House"));
        }
        if (other.name == "SecondTrigger")
        {
            interactionUI.SetActive(true);
            isNearMainDoor = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "SecondTrigger")
        {
            interactionUI.SetActive(false);
            isNearMainDoor = false;
        }
    }

    IEnumerator NewObjective(TMP_Text oldText, string newText) // Replace text
    {
        oldText.text = "<s>" + oldText.text + "</s>";

        yield return new WaitForSeconds(2f);

        oldText.text = newText;
    }

    IEnumerator NoFuel() // Cutscene
    {
        audioSources[1].PlayOneShot(clips[3]);
        yield return new WaitForSeconds(5f);
        barrier.SetActive(false);
        lowFuel = true;
        yield return new WaitForSeconds(18f);
        noFuel = true;
        yield return new WaitForSeconds(1f);
        rotateCam = true;
        yield return new WaitForSeconds(.5f);
        audioSources[1].Stop();
        barrier.SetActive(true);
        isCinemaMode = false;
        cinema.SetActive(false);
        plrUI.SetActive(true);
    }

    void IsCinemaMode()
    {
        if (noFuel)
        {
            transform.Translate(10 * Time.deltaTime * Vector3.forward); // Forward
            transform.Translate(speed * Time.deltaTime * Vector3.right); // Right
        }
        else
        {
            transform.Translate(highSpeed * Time.deltaTime * Vector3.forward); // Forward

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
    void SkipCutScene()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isCinemaMode)
        {
            transform.position = new Vector3(4.444809f, 1f, -11.10724f);
            audioSources[1].Stop();
            isCinemaMode = false;
            cinema.SetActive(false);
            plrUI.SetActive(true);
        }
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
            if (!audioSources[1].isPlaying)
            {
                if (isSprinting)
                {
                    // replace the audio with running clip
                }
                audioSources[1].PlayOneShot(clips[1]);
            }
        }
    }
    void SprintLogic()
    {
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

                if (!audioSources[0].isPlaying)
                {
                    audioSources[0].PlayOneShot(clips[2]);
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
    void InteractionLogic()
    {
        if (Input.GetKey(KeyCode.E) && interactionUI.activeSelf && isNearMainDoor) // Teleports player inside the mansion
        {
            SceneManager.LoadScene("Mansion");
        }
    }
}
