using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StrikeBox : MonoBehaviour
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
    private Vector3 startPos, offsetVector, targetAngles = new Vector3(0, 0, 1);
    private Quaternion startRotation;

    [SerializeField] private float lifeLimit = 0.1f, swingIncrements = 0.01f;

    private void Start()
    {
        switch (facingDirection)
        {
            case FacingDirection.Up:
                flingDirection = new Vector2(0, 1);
                xOffset = -xOffset;
                transform.rotation = startRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.localPosition = startPos = new Vector3(xOffset, 0, 0);

                break;
            case FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                transform.rotation = startRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                transform.localPosition = startPos = new Vector3(0,  yOffset, 0);

                break;
            case FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                yOffset = -yOffset;
                transform.rotation = startRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = startPos = new Vector3(0, yOffset, 0);

                break;
            case FacingDirection.Down:
                flingDirection = new Vector2(0, -1);
                transform.rotation = startRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.localPosition = startPos = new Vector3(xOffset, 0, 0);
                break;
        }
        offsetVector = new Vector3(xOffset, yOffset, 0);
        finalFlingDirection = flingDirection * force;

        Destroy(gameObject, lifeLimit);
        InvokeRepeating("Swing", 0f, swingIncrements);
    }


    void Swing()
    {
        Debug.Log(transform.position);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + (180f / (lifeLimit / swingIncrements)));
        //transform.Rotate(0, 0, -(180f / (lifeLimit / swingIncrements)));//startPos, -(180f / (lifeLimit / swingIncrements)), Space.Self);
        transform.RotateAround(transform.parent.transform.position, targetAngles, -(180f / (lifeLimit / swingIncrements)));
        //transform.RotateAround(transform.position - offsetVector, targetAngles, -(180f / (lifeLimit / swingIncrements)));
        //transform.Rotate( targetAngles, -(180f / (lifeLimit / swingIncrements)), Space.Self);
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles,  swingIncrements);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!used)
        {
            Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rigidBody != null && collision.gameObject.tag == "Bouncy")
            {
                used = true;
                /*
                if (rigidBody.velocity.y < 0) rigidBody.setY(0);

                rigidBody.addX(finalFlingDirection.x);
                rigidBody.addY(finalFlingDirection.y);
                */
                Destroy(gameObject);
            }


        }

    }

}
