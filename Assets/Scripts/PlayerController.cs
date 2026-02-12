using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    // Find a way to save the data from one scene to another
    public float speed;
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
    bool isOn = false;
    bool isSprinting = false;
    bool isNearMainDoor = false;

    Animator animator;

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
        
        animator = GetComponent<Animator>();

        if (currentState == GameState.Highway)
        {
            animator.enabled = true;
            StartCoroutine(CarDriving()); // Starts cutscene
            // Reset values
            audioSources[1].Play();
            barrier.SetActive(false);
            isCinemaMode = true;
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
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.name == "Trigger")
        {
            StartCoroutine(NewObjective(objectives, "• Enter the House", 1));
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

    IEnumerator NewObjective(TMP_Text oldText, string newObj, int objNum) // Replace text
    {
        int index = oldText.text.LastIndexOf("•");

        if (objNum == 1)
        {
            string oldObj;
            string remain = oldText.text.Substring(index);
            if (index == 0)
            {
                oldObj = oldText.text.Substring(index);
                oldObj = "<s>" + oldObj + "</s>";
                oldText.text = oldObj;
                yield return new WaitForSeconds(2f);
                oldText.text = newObj;
                yield break;
            }
            else
            {
                oldObj = oldText.text.Substring(0, index);

                oldObj = "<s>" + oldObj + "</s>";
                oldText.text = oldObj + remain;
            }

            yield return new WaitForSeconds(2f);

            oldText.text = newObj + "\n" + remain;
        }
        else if (objNum == 2)
        {
            string remain;
            string oldObj;
            if (index == 0)
            {
                remain = oldText.text.Substring(index);
                oldText.text = remain + newObj;
            }
            else
            {
                remain = oldText.text.Substring(0, index);
                oldObj = oldText.text.Substring(index);
                oldObj = "<s>" + oldObj + "</s>";
                oldText.text = remain + oldObj;
            }

            yield return new WaitForSeconds(2f);

            oldText.text = remain + newObj;
        }
    }

    IEnumerator CarDriving() // Cutscene
    {
        audioSources[1].PlayOneShot(clips[3]); // Play car sound
        yield return new WaitForSeconds(25f); // Wait until the cutscene is over
        animator.enabled = false; // disable cutscene animation
        audioSources[1].Stop(); // no car sound
        barrier.SetActive(true);
        isCinemaMode = false;
        cinema.SetActive(false);
        plrUI.SetActive(true);
    }
    void SkipCutScene()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isCinemaMode)
        {
            animator.enabled = false;
            transform.position = new Vector3(4.44f, 1f, -.77f);
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
            StartCoroutine(NewObjective(objectives, "", 2));
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
