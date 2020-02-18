using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyBodyFollowPlayer : MonoBehaviour
{
    Transform playerTransform;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform.parent.transform;
        //transform.position = playerPosition.position;   
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
        //rb.velocity = playerTransform.gameObject.GetComponent<PlayerPhysicsMovement>().velocity;
    }
}
