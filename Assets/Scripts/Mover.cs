using System.Collections;
using UnityEngine;


public class Mover : MonoBehaviour
{
    [SerializeField] Transform objectToMove;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float ADuration = 1;
    [SerializeField] float BDuration = 1;
    [SerializeField] AnimationCurve upEase;
    [SerializeField] AnimationCurve downEase;


    void Awake()
    {
        StartCoroutine(MovingCoroutine());
    }


    IEnumerator MovingCoroutine()
    {
        float t;

        while (true)
        {
            t = 0;

            while (t < ADuration)
            {
                t += Time.deltaTime;
                objectToMove.position = Vector3.Lerp(
                    pointA.position,
                    pointB.position,
                    upEase.Evaluate(t / ADuration)
                );

                yield return null;
            }


            t = 0;

            while (t < BDuration)
            {
                t += Time.deltaTime;
                objectToMove.position = Vector3.Lerp(
                    pointB.position,
                    pointA.position,
                    downEase.Evaluate(t / BDuration)
                );

                yield return null;
            }
        }
    }



}
