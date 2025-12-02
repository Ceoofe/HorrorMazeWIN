using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    float speed = 26;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.z > 88)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            speed -= 1.2f * Time.deltaTime;
        }
    }
}
