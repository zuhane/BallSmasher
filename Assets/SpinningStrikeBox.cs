using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinningStrikeBox : MonoBehaviour
{
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right,
        DownOut
    }

    public FacingDirection facingDirection;

    private Vector2 flingDirection;
    private Vector2 finalFlingDirection;
    public float xOffset = 0f, yOffset = 0f;
    private float force = 10;
    private bool used = false;
    private Vector3 startPos;


    [SerializeField] private float lifeLimit = 0.3f;
    [SerializeField] private float rotateFrequency = 0.02f;
    private float frequency;

    private void Start()
    {
        float offset = 0.3f;

        startPos = transform.position;
        switch (facingDirection)
        {
            case FacingDirection.Up:
                flingDirection = new Vector2(0, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 90);
                transform.localPosition = new Vector3(-xOffset, 0, 0);
                break;
            case FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                transform.localPosition = new Vector3(0, yOffset, 0);
                break;
            case FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                transform.localRotation = Quaternion.Euler(0, 0, 180);
                transform.localPosition = new Vector3(0, -yOffset, 0);
                break;
            case FacingDirection.Down:
                flingDirection = new Vector2(0, -1);                
                transform.localRotation = Quaternion.Euler(0, 0, 270);
                transform.localPosition = new Vector3(xOffset, 0, 0);
                break;
        }

        finalFlingDirection = flingDirection * force;
        frequency = lifeLimit / rotateFrequency;
        //InvokeRepeating("Rotate", 0, rotateFrequency);11
        Destroy(gameObject, lifeLimit);
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float angle = 180 / frequency;
        transform.RotateAround(transform.parent.transform.position, new Vector3(0, 0, 1), -angle * 
            ((facingDirection == FacingDirection.Left || facingDirection == FacingDirection.Right )
            ? 1f
            : transform.parent.transform.lossyScale.x));
        Debug.Log(transform.rotation);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!used)
        {
            Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rigidBody != null)
            {
                used = true;

                if (rigidBody.velocity.y < 0) rigidBody.setY(0);

                rigidBody.addX(finalFlingDirection.x);
                rigidBody.addY(finalFlingDirection.y);

                Destroy(gameObject);
            }


        }

    }

}
