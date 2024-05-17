using System.Collections.Generic;
using UnityEngine;


public class TargetTp : MonoBehaviour
{
    List<Transform> positions = new List<Transform>();
    int current = 0;


    private void Awake()
    {
        var objects = GameObject.FindGameObjectsWithTag("Moving");

        for (int i = 0; i < objects.Length; i++)
        {
            positions.Add(objects[i].transform);
        }

        MoveToNext();
    }




    public void MoveToNext()
    {
        int previous = current;
        int randomIndex = previous;

        while (randomIndex == previous)
        {
            randomIndex = Random.Range(0, positions.Count);
        }

        current = randomIndex;
        transform.position = positions[current].position;

    }
}
