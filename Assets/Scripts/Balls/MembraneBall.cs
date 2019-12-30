using UnityEngine;

public class MembraneBall : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        timer = Timer.CreateComponent(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.SecondsPassed(0.5f))
        {
            rigid.AddForce(VectorMath.Random() * Random.Range(6, 12));
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject goCollision = collision.transform.root.gameObject;
        if (goCollision.tag == "Player")
        {
            goCollision.transform.parent = transform;
            goCollision.transform.localPosition = new Vector3(0, 0, 0);
            foreach (Rigidbody2D rigid in goCollision.GetComponentsInChildren<Rigidbody2D>())
            {
                collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            goCollision.GetComponent<PlayerPhysicsMovement>().enabled = false;
        }
    }

}
