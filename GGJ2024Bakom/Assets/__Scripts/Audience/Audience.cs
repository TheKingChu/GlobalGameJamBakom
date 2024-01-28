//Created by Charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audience : MonoBehaviour
{
    public GameObject tomatoPrefab;
    public Transform[] points;
    private int current = 0;
    private float speed = 5f;
    private float repeatTime = 2f;

    // Update is called once per frame
    void Update()
    {
        AudienceMovement();
    }

    private void AudienceMovement()
    {
        if (Vector3.Distance(points[current].transform.position, transform.position) < repeatTime)
        {
            current++;
            if(current >= points.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, points[current].transform.position, Time.deltaTime * speed);
    }

    public void ThrowTomato()
    {

    }
}
