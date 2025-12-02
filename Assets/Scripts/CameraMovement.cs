using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    // [SerializeField] Slider mouseSlider;
    [SerializeField] Transform player;

    float xRotation = 0f;

    bool isOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //LoadMouseSensitivity();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.timeScale == 0) return;
        if (PlayerController.isCinemaMode) return;

        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    // public void SetMouseSensitivity()
    // {
    //     rotateSpeed = mouseSlider.value;
    //     PlayerPrefs.SetFloat("MouseSensitivity", rotateSpeed);
    // }
   
    // void LoadMouseSensitivity()
    // {
    //     if (PlayerPrefs.HasKey("MouseSensitivity"))
    //     {
    //         mouseSlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
    //     }
    //     SetMouseSensitivity();
    // }
}
