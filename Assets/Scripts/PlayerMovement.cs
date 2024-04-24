using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speedPlayer = 10f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement.x += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement.z += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement.z -= 1;
        }



        rb.velocity += movement * speedPlayer * Time.deltaTime;




    }
}
