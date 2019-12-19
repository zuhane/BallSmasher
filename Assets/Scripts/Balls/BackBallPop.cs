﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBallPop : MonoBehaviour
{
    Ball ball;
    // Start is called before the first frame update
    void Start()
    {
        ball = gameObject.GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.electrified)
        {
            ball.DestroyBall();
        }
    }
}
