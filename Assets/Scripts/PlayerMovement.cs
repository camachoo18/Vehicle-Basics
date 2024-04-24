using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStats stats; 

    Rigidbody rb;
    float currentSpeed = 0f;

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

        //friction
        Vector3 friction = -rb.velocity.normalized * stats.groundFriction;
        rb.velocity += friction * Time.deltaTime;

        
        currentSpeed = rb.velocity.magnitude;

        //velocidad maxima
        if (currentSpeed > stats.maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * stats.maxSpeed;
            currentSpeed = stats.maxSpeed;
        }

        //aceleration
        rb.velocity += movement * stats.forwardAcceleration * Time.deltaTime;

    }
}
