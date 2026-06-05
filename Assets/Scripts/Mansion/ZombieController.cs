using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position += 10 * Time.deltaTime * Vector3.forward;
    }
}
