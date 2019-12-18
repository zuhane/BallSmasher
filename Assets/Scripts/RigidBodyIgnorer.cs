using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyIgnorer : MonoBehaviour
{
    //Set the speed number in the Inspector window
    public float m_Speed;
    Rigidbody2D m_Rigidbody;

    void Start()
    {
        //Fetch the Rigidbody component from the GameObject
        m_Rigidbody = GetComponent<Rigidbody2D>();
        //Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
        Physics2D.IgnoreLayerCollision(0, 0);
        //Physics.IgnoreLayerCollision(0, 9);
        //Physics.IgnoreLayerCollision(0, 8);
        //Physics.IgnoreLayerCollision(0, 0);
    }

    void Update()
    {
        //Press right to move the GameObject to the right. Make sure you set the speed high in the Inspector window.
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_Rigidbody.AddForce(Vector3.right * m_Speed);
        }

        //Press the left arrow key to move the GameObject to the left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_Rigidbody.AddForce(Vector3.left * m_Speed);
        }
    }

    //Detect when there is a collision
    void OnCollisionStay(Collision collide)
    {
        //Output the name of the GameObject you collide with
        Debug.Log("I hit the GameObject : " + collide.gameObject.name);
    }
}
