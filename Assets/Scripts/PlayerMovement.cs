using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speedPlayer = 10f;
    [SerializeField] float friction = 5f;
    Rigidbody rb;
    float maxVelocity = 10f;
    Vector3 rotate = new Vector3(0, 0.1f, 0);
    [SerializeField] ParticleSystem particulasAC;
    [SerializeField] ParticleSystem left;
    [SerializeField] ParticleSystem right;

    void Start()
    {
        particulasAC.Stop();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement.z -= 1;
            //movement += transform.forward;
            
        }

        if (Input.GetKeyDown(KeyCode.W))
            particulasAC.Play();


        if (Input.GetKey(KeyCode.S))
        {
            // movement.x -= 1;

        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.angularVelocity += rotate;
        }
        if (Input.GetKeyDown(KeyCode.D))
            right.Play();
        
        
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            left.Stop();
            right.Stop();
        }


        if (Input.GetKey(KeyCode.A))
        {
            rb.angularVelocity -= rotate;

        }
        if (Input.GetKeyDown(KeyCode.A))
            left.Play();


        if (movement != Vector3.zero)
            rb.velocity += transform.forward * speedPlayer * Time.deltaTime;

        else
        {
            //rb.velocity /= rb.velocity.z + friccion * Time.deltaTime;
            rb.velocity = new Vector3(rb.velocity.x / (1 + friction * Time.deltaTime),
                rb.velocity.y,
                (rb.velocity.z / (1 + friction * Time.deltaTime)));
           //print(friccion);
            particulasAC.Stop();
        }


        if (rb.velocity.x >= maxVelocity || rb.velocity.z >= maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }



    }
}
