using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStrikeBox : MonoBehaviour
{

    public SpinningStrikeBox.FacingDirection facingDirection;

    private Vector2 flingDirection;
    private Vector2 finalFlingDirection;
    public float xOffset = 0f, yOffset = 0f;
    private float force = 10;
    private bool used = false;
    private Vector3 startPos;

    [SerializeField] private float lifeLimit = 0.55f;

    private void Start()
    {
        float offset = 0.3f;

        switch (facingDirection)
        {
            case SpinningStrikeBox.FacingDirection.Up:
                flingDirection = new Vector2(0, 1);
                transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
                break;
            case SpinningStrikeBox.FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
                break;
            case SpinningStrikeBox.FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.position = new Vector3(transform.position.x - xOffset, transform.position.y, transform.position.z);
                break;
            case SpinningStrikeBox.FacingDirection.Down:
                flingDirection = new Vector2(0, -1);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
                break;
            case SpinningStrikeBox.FacingDirection.DownOut:
                flingDirection = new Vector2(0, 1);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localScale = new Vector3(5, 0.8f, 1);
                transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
                break;
        }

        finalFlingDirection = flingDirection * force;

        Destroy(gameObject, lifeLimit);
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
