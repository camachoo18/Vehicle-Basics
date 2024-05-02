using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speedPlayer = 10f;
    Rigidbody rb;
    float maxVelocity = 10f;
    Vector3 rotate = new Vector3(0, 0.1f, 0);

    [Header("Friction y break")]
    [SerializeField] float breakPower;
    [SerializeField] float friction;

    [Header("Particulas")]
    [SerializeField] ParticleSystem particulasAC;
    [SerializeField] ParticleSystem particulasleft;
    [SerializeField] ParticleSystem particulasRight;

    void Start()
    {
        particulasAC.Stop();
        particulasleft.Stop();
        particulasRight.Stop();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

       
        if (Input.GetKey(KeyCode.W))
        {
            movement.z += 1;
            print("a");
        }

        if (Input.GetKeyDown(KeyCode.W))
            particulasAC.Play();


//break and backward

        if (Input.GetKeyDown(KeyCode.D))
            particulasleft.Play();

        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            particulasleft.Stop();
            particulasRight.Stop();
        }

        if (Input.GetKeyDown(KeyCode.A))
            particulasRight.Play();



        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    if(movement != Vector3.zero)
        //    {
        //        rb.velocity = new Vector3(
        //                rb.velocity.x / ( 1 + frenado * Time.deltaTime),
        //                rb.velocity.y,
        //                (rb.velocity.z / (1 + frenado * Time.deltaTime)));
        //        print("s");
        //    }

        //    else
        //    {
        //        rb.AddForce(-transform.forward * -speedPlayer * Time.deltaTime);
        //    }
        //    print(movement);
        //}

        if (Input.GetKey(KeyCode.D))
        {
            rb.angularVelocity += rotate;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.angularVelocity -= rotate;

        }


        if (movement != Vector3.zero)
        {
            rb.velocity += transform.forward * speedPlayer * Time.deltaTime;

            if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = new Vector3(
                    rb.velocity.x / (1 + breakPower * Time.deltaTime),
                    rb.velocity.y,
                    (rb.velocity.z / (1 + breakPower * Time.deltaTime)));
                print("Estoy Frenando");
            }
        }

        else
        {
            rb.velocity = new Vector3(
                rb.velocity.x / (1 + friction * Time.deltaTime),
                rb.velocity.y,
                (rb.velocity.z / (1 + friction * Time.deltaTime)));
            print(friction);
            particulasAC.Stop();
            if (Input.GetKey(KeyCode.S))
            {
                rb.velocity -= transform.forward * speedPlayer * Time.deltaTime;
                print("patras");
            }
        }


        if (rb.velocity.x >= maxVelocity || rb.velocity.z >= maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }



    }
}
