using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOrbIdle : MonoBehaviour
{
    private float x = 0.01f, y = 0;

    private Vector3 originalPosition, orbitalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {



        //if (x > 1) x *= 0.9f;
        //else if (x < -1) x *= 1.1f;

        //orbitalPosition = new Vector3(x, y, 0);

        //transform.position += orbitalPosition;
    }
}
