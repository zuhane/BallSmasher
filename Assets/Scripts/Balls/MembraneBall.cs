using System.Collections.Generic;
using UnityEngine;

public class MembraneBall : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Timer timer;

    public class CapturedPlayer
    {
        public GameObject player;
        public Timer timer;
    }

    public List<CapturedPlayer> capturedPlayers = new List<CapturedPlayer>();

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
        foreach (CapturedPlayer capturedPlayer in capturedPlayers)
        {
            if (capturedPlayer.timer.LimitReached())
            {
                releasePlayer(capturedPlayer);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject goCollisionRoot = collision.transform.root.gameObject;
        if (goCollisionRoot.tag == "Player")
        {
            consumePlayer(goCollisionRoot);
        }
    }


    private void consumePlayer(GameObject player)
    {
        player.transform.parent = transform;
        player.transform.localPosition = new Vector3(0, 0, 0);
        foreach (Rigidbody2D rigid in player.GetComponentsInChildren<Rigidbody2D>())
        {
            player.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        }
        player.GetComponent<PlayerPhysicsMovement>().enabled = false;

        CapturedPlayer capturedPlayer = new CapturedPlayer();
        capturedPlayer.player = player;
        capturedPlayer.timer = Timer.CreateComponent(gameObject, 3);
        capturedPlayers.Add(capturedPlayer);
    }

    public void releasePlayer(CapturedPlayer cp)
    {
        cp.player.transform.parent = null;
        foreach (Rigidbody2D rigid in cp.player.GetComponentsInChildren<Rigidbody2D>())
        {
            cp.player.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        }
        cp.player.GetComponent<PlayerPhysicsMovement>().enabled = true;
        capturedPlayers.Remove(cp);
    }
}
