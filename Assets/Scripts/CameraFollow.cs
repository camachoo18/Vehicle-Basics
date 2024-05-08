using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float followSmooth = .3f;
    [SerializeField] float lookAheadDistance = 2;
    [SerializeField] float lookAheadSmooth = 1;
    [SerializeField] float rotationSmooth = 1;
    [SerializeField] float minDistanceToTarget = 5;

    Vector3 positionVelocity = Vector3.zero;
    Vector3 lookAhead;
    bool playerIsDead = false;
    Vector3 lookAheadVelocity = Vector3.zero;
    Vector3 directionToTarget;


    void Start()
    {
        transform.position = target.position;

      
    }


     void Update()
    {
        if (!playerIsDead)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                target.position - target.forward * minDistanceToTarget,
                ref positionVelocity,
                followSmooth
            );

            directionToTarget = target.position - transform.position;
            if (directionToTarget.magnitude < minDistanceToTarget)
                transform.position = target.position - directionToTarget.normalized * minDistanceToTarget;

            lookAhead = Vector3.SmoothDamp(
                lookAhead,
                target.forward * lookAheadDistance,
                ref lookAheadVelocity,
                lookAheadSmooth
            );

            transform.LookAt(target.position + lookAhead);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }

        else
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                target.position,
                ref positionVelocity,
                followSmooth
            );

            transform.LookAt(target.position);
        }
    }






}
