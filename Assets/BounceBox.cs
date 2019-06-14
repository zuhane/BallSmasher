using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBox : MonoBehaviour
{
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public FacingDirection facingDirection;

    private Vector2 flingDirection;
    private Vector2 finalFlingDirection;
    public float xOffset = 0f, yOffset = 0f;
    private float force = 6;
    private bool used = false;

    private int counter, countLimit = 0;

    private float lifeLimit = 0.05f;

    private void Start()
    {
        float offset = 0.3f;

        switch (facingDirection)
        {
            case FacingDirection.Up:
                flingDirection = new Vector2(0, 1);
                transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
                break;
            case FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
                break;
            case FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.position = new Vector3(transform.position.x - xOffset, transform.position.y, transform.position.z);
                break;
            case FacingDirection.Down:
                flingDirection = new Vector2(0, -1);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
                break;
        }

        finalFlingDirection = flingDirection * force;

        //Destroy(gameObject, lifeLimit);
    }

    private void Update()
    {
        if (used)
        {
            counter++;

            if (counter >= countLimit)
            {
                counter = 0;
                used = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!used)
        {


            Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rigidBody == null)
                rigidBody = collision.gameObject.GetComponentInParent<Rigidbody2D>();


            if (rigidBody != null)
            {
                used = true;

                if (rigidBody.velocity.y < 0) rigidBody.setY(0);

                rigidBody.addX(finalFlingDirection.x);
                rigidBody.addY(finalFlingDirection.y);

                //Destroy(gameObject);
            }


        }


    }
}
