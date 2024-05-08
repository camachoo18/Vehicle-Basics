using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speedPlayer = 50f;
    Rigidbody rb;
    float maxVelocity = 10f;

    [Header("Giro")]
    [SerializeField] Vector3 rotate = new Vector3(0, 0.1f, 0);
    [SerializeField] Vector3 giroFrenoMano;

    [Header("Friction and break")]
    [SerializeField] float breakPower;
    [SerializeField] float friction;
    bool gearLever;
    [SerializeField] float handBreakPower;

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

        gearLever = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.W))
            particulasAC.Play();

        if (Input.GetKeyDown(KeyCode.D))
            particulasL.Play();

        if (Input.GetKeyDown(KeyCode.A))
            particulasR.Play();

        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            particulasL.Stop();
            particulasR.Stop();
        }

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
                        rb.velocity.x / (1 + breakPower * Time.fixedDeltaTime),
                        rb.velocity.y,
                        rb.velocity.z / (1 + breakPower * Time.fixedDeltaTime));
            }

        }

        if (gearLever)
        {
            rb.velocity = new Vector3(
                        rb.velocity.x / (1 + handBreakPower * Time.fixedDeltaTime),
                        rb.velocity.y,
                        rb.velocity.z / (1 + handBreakPower * Time.fixedDeltaTime));


            print("palancaDECambio");
        }


        if (movementInput.y < 0)
        {
            rb.angularVelocity -= new Vector3(
                rb.angularVelocity.x,
                rb.angularVelocity.y - rotate.y);

            if (gearLever)
            {
                rb.angularVelocity += new Vector3(
                rb.angularVelocity.x,
                rb.angularVelocity.y + giroFrenoMano.y);
            }
        }

        if (movementInput.y > 0)
        {
            rb.angularVelocity -= new Vector3(
                rb.angularVelocity.x,
                rb.angularVelocity.y + rotate.y);

            if (gearLever)
            {
                rb.angularVelocity -= new Vector3(
                rb.angularVelocity.x,
                rb.angularVelocity.y + giroFrenoMano.y);
            }
        }

        if (movementInput.x > 0)
        {
            rb.velocity += transform.forward * speedPlayer * Time.fixedDeltaTime;

            //if (Input.GetKey(KeyCode.S))
            //{
            //    rb.velocity = new Vector3(
            //        rb.velocity.x / (1 + breakPower * Time.deltaTime),
            //        rb.velocity.y,
            //        (rb.velocity.z / (1 + breakPower * Time.deltaTime)));
            //    print("Estoy Frenando");
            //}
        }

        else
        {
            rb.velocity = new Vector3(
                rb.velocity.x / (1 + friction * Time.deltaTime),
                rb.velocity.y,
                (rb.velocity.z / (1 + friction * Time.deltaTime)));
            //print(friction);
            particulasAC.Stop();
            //if (Input.GetKey(KeyCode.S))
            //{
            //    rb.velocity -= transform.forward * speedPlayer * Time.deltaTime;
            //    print("MarchaTra");
            //}
        }

        if (rb.velocity.x >= maxVelocity || rb.velocity.z >= maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}
