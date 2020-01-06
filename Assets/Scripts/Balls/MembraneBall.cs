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
        public SpriteRenderer spriteRend; //TODO: Draw player stuck inside? Optional init!
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

        for (int i = 0; i < capturedPlayers.Count; i++)
        {
            if (capturedPlayers[i].timer.LimitReached())
            {
                releasePlayer(capturedPlayers[i]);
            }
        }
    }

    private void EnableSlurp()
    {
        AudioManager.PlaySound("MembraneRelease", UnityEngine.Random.Range(0.8f, 1.2f));
        Instantiate(Resources.Load<GameObject>("Effects/MembraneSplat"), transform.position, Quaternion.identity, transform);
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
        AudioManager.PlaySound("MembraneRelease", UnityEngine.Random.Range(0.8f, 1.2f));
        Instantiate(Resources.Load<GameObject>("Effects/MembraneSplat"), transform.position, Quaternion.identity, transform);
        animator.SetTrigger("Wobble");

        player.SetActive(false);

        CapturedPlayer capturedPlayer = new CapturedPlayer();
        capturedPlayer.player = player;
        capturedPlayer.timer = Timer.CreateComponent(gameObject, (int)consumeTime);
        capturedPlayers.Add(capturedPlayer);
    }

    public void releasePlayer(CapturedPlayer cp)
    {
        AudioManager.PlaySound("MembraneRelease", UnityEngine.Random.Range(0.8f, 1.2f));
        Instantiate(Resources.Load<GameObject>("Effects/MembraneSplat"), transform.position, Quaternion.identity, transform);

        cp.player.SetActive(true);
        cp.player.transform.position = transform.position;
        cp.player.transform.rotation = Quaternion.identity;
        
        capturedPlayers.Remove(cp);

        DisableSlurp();
    }
}
