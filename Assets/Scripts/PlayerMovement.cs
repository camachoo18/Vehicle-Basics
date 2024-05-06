using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speedPlayer = 10f;
    Rigidbody rb;
    float maxVelocity = 10f;
    [SerializeField] Vector3 rotate = new Vector3(0, 0.1f, 0);

    [Header("Friction and break")]
    [SerializeField] float powerBreak;
    [SerializeField] float friction;

    [Header("Particles")]
    [SerializeField] ParticleSystem particulasAC;
    [SerializeField] ParticleSystem particulasL;
    [SerializeField] ParticleSystem particulasR;
    Vector2 movementInput = Vector2.zero;


    void Start()
    {
        particulasAC.Stop();
        particulasL.Stop();
        particulasR.Stop();
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        MovementInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void MovementInput()
    {
        movementInput = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movementInput.x += 1;
        }



        if (Input.GetKey(KeyCode.S))
        {
            movementInput.x -= 1;
        }



        if (Input.GetKey(KeyCode.D))
        {
            movementInput.y += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementInput.y -= 1;
        }


        if (Input.GetKeyDown(KeyCode.W))
            particulasAC.Play();

        if (Input.GetKeyDown(KeyCode.D))
            particulasL.Play();

        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            particulasL.Stop();
            particulasR.Stop();
        }

        if (Input.GetKeyDown(KeyCode.A))
            particulasR.Play();

    }


    void Movement()
    {

        if (movementInput.x < 0)
        {

            if (transform.InverseTransformDirection(rb.velocity).z < 0)
            {
                rb.velocity -= transform.forward * speedPlayer * Time.fixedDeltaTime;

            }

            else
            {
                rb.velocity = new Vector3(
                        rb.velocity.x / (1 + powerBreak * Time.fixedDeltaTime),
                        rb.velocity.y,
                        rb.velocity.z / (1 + powerBreak * Time.fixedDeltaTime));


            }

        }



        if (movementInput.y != 0)
        {
            rb.angularVelocity += new Vector3(
                0,
                rotate.y * movementInput.y,
                0
            );

        }

        if (movementInput.x > 0)
        {
            rb.velocity += transform.forward * speedPlayer * Time.fixedDeltaTime;


        }

        else
        {
            rb.velocity = new Vector3(
                rb.velocity.x / (1 + friction * Time.deltaTime),
                rb.velocity.y,
                (rb.velocity.z / (1 + friction * Time.deltaTime)));

            particulasAC.Stop();

        }

        if (rb.velocity.x >= maxVelocity || rb.velocity.z >= maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}
