using System.Collections.Generic;
using UnityEngine;

public class MembraneBall : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Timer timer;
    [SerializeField] private float consumeTime = 1.5f;
    [SerializeField] private int disableTime = 300;
    private bool disabled;
    private int disabledCounter;
    private Animator animator;

    private SpriteRenderer spriteRend;

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
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Add second timer in. Can't have 2 of the same component?!?!?! Hmmmm...
        if (disabled)
        {
            disabledCounter++;

            if (disabledCounter >= disableTime)
            EnableSlurp();
        }

        if (timer.SecondsPassed(0.5f) && capturedPlayers.Count < 1)
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

    private void EnableSlurp()
    {
        disabledCounter = 0;
        disabled = false;
        animator.SetBool("Dormant", disabled);
        animator.SetTrigger("Wobble");
    }

    private void DisableSlurp()
    {
        disabled = true;
        animator.SetBool("Dormant", disabled);
        animator.SetTrigger("Wobble");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject goCollisionRoot = collision.transform.root.gameObject;
        if (goCollisionRoot.tag == "Player" && !disabled)
        {
            consumePlayer(goCollisionRoot);
        }
    }


    private void consumePlayer(GameObject player)
    {
        rigid.velocity = Vector2.zero;
        AudioManager.PlaySound("MembraneRelease", UnityEngine.Random.Range(0.8f, 1.2f));
        Instantiate(Resources.Load<GameObject>("Effects/MembraneSplat"), transform.position, Quaternion.identity, transform);
        player.transform.parent = transform;
        player.transform.localPosition = new Vector3(0, 0, 0);
        foreach (Rigidbody2D rigid in player.GetComponentsInChildren<Rigidbody2D>())
        {
            player.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        }
        player.GetComponent<PlayerPhysicsMovement>().enabled = false;

        CapturedPlayer capturedPlayer = new CapturedPlayer();
        capturedPlayer.player = player;
        capturedPlayer.timer = Timer.CreateComponent(gameObject, (int)consumeTime);
        capturedPlayers.Add(capturedPlayer);
    }

    public void releasePlayer(CapturedPlayer cp)
    {
        AudioManager.PlaySound("MembraneRelease", UnityEngine.Random.Range(0.8f, 1.2f));
        Instantiate(Resources.Load<GameObject>("Effects/MembraneSplat"), transform.position, Quaternion.identity, transform);
        DisableSlurp();
        cp.player.transform.parent = null;
        foreach (Rigidbody2D rigid in cp.player.GetComponentsInChildren<Rigidbody2D>())
        {
            cp.player.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        }

        cp.player.transform.rotation = new Quaternion(0, 0, 0, 0);
        cp.player.GetComponent<PlayerPhysicsMovement>().enabled = true;
        capturedPlayers.Remove(cp);
    }
}
