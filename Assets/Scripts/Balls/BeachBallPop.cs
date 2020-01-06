using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallPop : MonoBehaviour
{
    Ball ball;
    [SerializeField] Explosion explosion;

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
            Instantiate(explosion, transform.position, Quaternion.identity);
            ball.DestroyBall();
        }
    }

}
