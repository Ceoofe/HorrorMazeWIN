using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float speed;
    public static float stamina = 100f;
    float timer;
    readonly float waitTime = 0.05f;

    GameObject cinema;
    GameObject interactionUI;
    GameObject transition;

    Transform plrUI;

    Slider staminaBar;

    TMP_Text objectives;

    public static AudioSource[] audioSources;
    public AudioClip[] clips;

    public static bool isCinemaMode; // False to skip cutescene
    public static bool isPressed = false;
    bool isSprinting = false;
    bool isNearMainDoor = false;
    bool check = false;

    public static string[] item = new string[1];

    // Start is called before the first frame update
    void Awake()
    {
        Transform canvas = GameObject.Find("Canvas").transform;

        audioSources = GetComponents<AudioSource>();
        plrUI = canvas.Find("PlayerUI");
        staminaBar = plrUI.Find("StaminaBar").GetComponent<Slider>();
        objectives = plrUI.Find("ObjectiveUI/Objectives").GetComponent<TMP_Text>();
        interactionUI = plrUI.Find("Indicator").gameObject;
        transition = canvas.Find("Transition").gameObject;

        cinema = canvas.Find("Cinema").gameObject;

        if (SceneManager.GetActiveScene().name == "Game")
        {
            // Reset values
            audioSources[1].Play();
            isCinemaMode = true;
            isSprinting = false;
            stamina = 100f;
        }
        item[0] = "None";
    }

    // Update is called once per frame
    void Update()
    {
        SprintLogic();
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

        if (other.name == "Trigger" && !check) // Changes objective
        {
            StartCoroutine(plrUI.transform.Find("ObjectiveUI").GetComponent<Objectives>().NewObjective(objectives, "• Enter the House", 1));
            check = true;
        }
        if (other.name == "SecondTrigger" && check) // Player at the front door
        {
            interactionUI.SetActive(true);
            isNearMainDoor = true;
        }

        if (other.CompareTag("Door"))
        {
            interactionUI.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "SecondTrigger")
        {
            interactionUI.SetActive(false);
            isNearMainDoor = false;
        }

        if (other.CompareTag("Door"))
        {
            interactionUI.SetActive(false);
        }
    }

    void SkipCutScene()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isCinemaMode)
        {
            transform.position = new Vector3(1.7f, 1.2f, -10.8f); // 4.44f 1.2f -7.7f 
            audioSources[1].Stop();
            isCinemaMode = false;
            cinema.SetActive(false);
            plrUI.gameObject.SetActive(true);
            Destroy(GameObject.Find("Cars"));
            gameObject.GetComponent<PlayerController>().enabled = false;
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
    void InteractionLogic()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isPressed = false;
        }


        if (isPressed && interactionUI.activeSelf && isNearMainDoor) // Teleports player inside the mansion
        {
            StartCoroutine(plrUI.transform.Find("ObjectiveUI").GetComponent<Objectives>().NewObjective(objectives, "", 1));
            StartCoroutine(transition.GetComponent<Transition>().LoadingScreen(3f, 2)); // Freeze player
        }
    }
}
