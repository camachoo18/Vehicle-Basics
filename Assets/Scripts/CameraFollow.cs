using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSmooth = 0.125f;
    [SerializeField] float lookAheadDistance = 5.0f;
    [SerializeField] float lookAheadSmooth = 0.125f;
    [SerializeField] float rotationSmooth = 0.125f;
    [SerializeField] float minDistanceToTarget = 2.0f;

    Vector3 offset;
    Vector3 desiredPosition;
    Vector3 smoothPosition;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        Vector3 targetPosition = target.position + offset;

        Vector3 lookAhead = target.forward * lookAheadDistance;

        desiredPosition = targetPosition + lookAhead;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < minDistanceToTarget)
        {
            desiredPosition = target.position - transform.forward * minDistanceToTarget;
        }

        smoothPosition = Vector3.Lerp(transform.position, desiredPosition, followSmooth);

        transform.position = smoothPosition;

        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSmooth);
    }
}
