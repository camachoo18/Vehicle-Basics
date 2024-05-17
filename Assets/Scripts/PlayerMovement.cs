using System.Collections;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speedPlayer = 50f;
    Rigidbody rb;
    float maxVelocity = 10f;
    [SerializeField] TrailRenderer skidTrail;
    float reboundStrength = 40.75f;
    TargetTp targetTp;

    [Header("Score")]
    float startScore = 50;
    float totalScore;
    float passTimeScore;
    float plusMinusScoreToShow;
    [SerializeField] GameObject canvasScore;
    float seconds;
    float animationDuration = 2;
    [SerializeField] TMP_Text TotalScoreText;
    [SerializeField] TMP_Text MinusPlusScoreText;
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
    Controls control;


    void Start()
    {
        particulasAC.Stop();
        particulasL.Stop();
        particulasR.Stop();
        rb = GetComponent<Rigidbody>();
        targetTp = FindObjectOfType<TargetTp>();
        passTimeScore = startScore;
        canvasScore.SetActive(false);

        control = new Controls();
        control.Enable();

    }

    void Update()
    {
        MovementInput();
        passTimeScore -= Time.deltaTime;
        if (passTimeScore <= 0)
            passTimeScore = 0;
        if (totalScore <= 0)
            totalScore = 0;

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
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
            if (!particulasAC.isPlaying)
                particulasAC.Play();
        }
        else
        {
            particulasAC.Stop();
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementInput.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementInput.y += 1;
            if (!particulasL.isPlaying)
                particulasL.Play();
        }
        else if (!Input.GetKey(KeyCode.A) && !control.Movement.Turn.triggered)
        {
            particulasL.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementInput.y -= 1;
            if (!particulasR.isPlaying)
                particulasR.Play();
        }
        else if (!Input.GetKey(KeyCode.D) && !control.Movement.Turn.triggered)
        {
            particulasR.Stop();
        }

        float breakPowerValue = control.Movement.Break.ReadValue<float>();
        if (breakPowerValue != 0)
        {
            movementInput.x += breakPowerValue;
        }

        float forwardValue = control.Movement.Forward.ReadValue<float>();
        if (forwardValue != 0)
        {
            movementInput.x += forwardValue;
        }

        float turnValue = control.Movement.Turn.ReadValue<float>();
        if (turnValue != 0)
        {
            movementInput.y += turnValue;
        }

        gearLever = Input.GetKey(KeyCode.Space);
    }

    void Movement()
    {

        float breakPowerValue = control.Movement.Break.ReadValue<float>();

        if (movementInput.x < 0)
        {
            if (transform.InverseTransformDirection(rb.velocity).z < 0)
            {
                rb.velocity -= transform.forward * speedPlayer * Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = new Vector3(
                        rb.velocity.x / (1 + breakPowerValue * Time.fixedDeltaTime),
                        rb.velocity.y,
                        rb.velocity.z / (1 + breakPowerValue * Time.fixedDeltaTime));
            }
        }


        float breakValue = control.Movement.HandBreak.ReadValue<float>();
        if (gearLever || breakValue > 0.5f)
        {
            rb.velocity = new Vector3(
                rb.velocity.x / (1 + handBreakPower * Time.fixedDeltaTime),
                rb.velocity.y,
                rb.velocity.z / (1 + handBreakPower * Time.fixedDeltaTime));


            skidTrail.emitting = true;


        }
        else
        {

            skidTrail.emitting = false;
        }



        if (movementInput.y < 0)
        {
            rb.angularVelocity -= new Vector3(
                rb.angularVelocity.x,
                rb.angularVelocity.y - rotate.y);

            if (gearLever)
            {
                rb.angularVelocity -= new Vector3(
                rb.angularVelocity.x,
                rb.angularVelocity.y - giroFrenoMano.y);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PropChoque"))
        {
            Rebound(collision.GetContact(0).normal);
            plusMinusScoreToShow = -Mathf.FloorToInt(passTimeScore);

            if (totalScore <= 0)
            {
                totalScore = 0;
                TotalScoreText.text = "0";

            }
            else
            {
                totalScore -= Mathf.FloorToInt(passTimeScore);
                TotalScoreText.text = totalScore.ToString();
            }
            MinusPlusScoreText.text = plusMinusScoreToShow.ToString();
            StartCoroutine(fadeOut());

            if (passTimeScore <= 0)
                passTimeScore = startScore;

        }
    }

    void Rebound(Vector3 normal)
    {
        //se utiliza para calcular la dirección de rebote del objeto después de una colisión
        Vector3 reboteDirection = Vector3.Reflect(rb.velocity, normal).normalized;

        //se utiliza para determinar el ángulo entre la dirección actual del objeto y la dirección de rebote calculad
        float angle = Vector3.Angle(rb.velocity, reboteDirection);
        if (angle > 185f)
        {
            //se utiliza para suavizar la dirección de rebote cuando el ángulo entre la dirección actual del objeto y la dirección de rebote es mayor que 185 grados
            reboteDirection = Vector3.Slerp(rb.velocity.normalized, reboteDirection, 1.5f);
        }

        rb.velocity = reboteDirection * reboundStrength;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            targetTp.MoveToNext();
            totalScore += Mathf.FloorToInt(passTimeScore);
            plusMinusScoreToShow = +Mathf.FloorToInt(passTimeScore);
            TotalScoreText.text = totalScore.ToString();
            MinusPlusScoreText.text = "+" + plusMinusScoreToShow.ToString();

            StartCoroutine(fadeOut());

            passTimeScore = startScore;
        }
    }

    public IEnumerator fadeOut()
    {
        float t = animationDuration;
        while (t > 0)
        {
            canvasScore.SetActive(true);
            t -= Time.deltaTime;
            yield return null;
        }
        canvasScore.SetActive(false);
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

