using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStrikeBox : MonoBehaviour
{

    public FacingDirection thisFacingDirection;

    private Vector2 flingDirection;
    private Vector2 finalFlingDirection;
    public float xOffset = 0f, yOffset = 0f;

    [SerializeField] public AudioClip chargeUpSound, chargedFullySound, attackReleaseSound;

    private float _force = 10;

    public float force
    {
        get
        {
            return _force;
        }
        set
        {
            _force = value;
            transform.localScale *= 0.1f + (_force / 4);
            finalFlingDirection = flingDirection * _force ;
        }
    }

    private bool used = false;
    private Vector3 startPos;
    private Animator anim;

    private float lifeLimit = 0.2f;
    private bool _released;
    public bool released
    {
        get
        {
            return _released;
        }
        set
        {
            _released = value;
            anim.SetBool("Firing", _released);
            Destroy(gameObject, lifeLimit);
        }
    }

    private bool _charged;
    public bool charged
    {
        get
        {
            return _charged;
        }
        set
        {
            _charged = value;
            anim.SetBool("FullyCharged", _charged);
        }
    }

    public int damage = 1;


    private void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        float offset = 0.3f;

        SetDirection(thisFacingDirection);
    }

    public void SetDirection(FacingDirection facingDirection)
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        switch (facingDirection)
        {
            case FacingDirection.Up:
                flingDirection = new Vector2(0, 1);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset + 0.2f, transform.localPosition.z);
                break;
            case FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.localPosition = new Vector3(transform.localPosition.x + xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.localPosition = new Vector3(transform.localPosition.x - xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Down:
                flingDirection = new Vector2(0, -1);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset, transform.localPosition.z);
                break;
            case FacingDirection.DownOut:
                flingDirection = new Vector2(0.5f, 1.5f);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localScale = new Vector3(3, 0.8f, 1);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset + 0.2f, transform.localPosition.z);
                _force /= 3;
                break;
        }
    }

    public void MultiplyScale(float scale)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_released && !used)
        {
            Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rigidBody != null && rigidBody.bodyType == RigidbodyType2D.Dynamic)
            {
                used = true;

                if (rigidBody.velocity.y < 0) rigidBody.setY(0);

                if (collision.transform.position.x < transform.parent.transform.position.x && thisFacingDirection == FacingDirection.DownOut) { finalFlingDirection.x *= -1; }

                rigidBody.addX(finalFlingDirection.x);
                rigidBody.addY(finalFlingDirection.y);

                Destroy(gameObject);

                StatsRPG stats = collision.gameObject.GetComponent<StatsRPG>();

                if (stats != null)
                {
                    Debug.Log(damage);
                    stats.TakeDamage(damage);
                }
            } 



        }



    }

}
