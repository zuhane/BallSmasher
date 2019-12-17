using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBallExplode : MonoBehaviour
{

    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigid.velocity.magnitude > 40)
        {
            AudioManager.PlaySound("PlasmaBallExplode", Random.Range(0.8f, 1.2f));
            Instantiate(Resources.Load<GameObject>("Effects/PlasmaBallExplosion"), transform.position, Quaternion.identity);
            Destroy(gameObject);

            GameObject.Find("GoalManager").GetComponent<GoalManager>().SpawnBall(3);
        }
    }
}
