using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGrabber : MonoBehaviour
{

    private Vector2 _velocity;

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }



    public void AddVelocity(Vector2 velocity)
    {
        _velocity += velocity;
    }


    private void ChangeInternalVelocity(Vector2 velocity)
    {

        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = velocity;
        }

        if (GetComponent<PhysicsObject>() != null)
        {

        }

    }





}
