using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBallExplode : MonoBehaviour
{

    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigid.velocity.magnitude > 40)
        {
            gameObject.GetComponent<Ball>().DestroyBall();
        }
    }
}
