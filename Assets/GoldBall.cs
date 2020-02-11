using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBall : MonoBehaviour
{
    private GameObject shardPickup;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject popEffect;
    private GoldenBallTransformation goldenInfo;

    private void Start()
    {
        shardPickup = Resources.Load<GameObject>("Pickups/ShardPickup");
        goldenInfo = GetComponent<GoldenBallTransformation>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject goCollisionRoot = collision.transform.root.gameObject;

        if (goCollisionRoot.tag == "Player" && goldenInfo.numElectrifications >= 2)
        {
            if (audioClip != null) AudioManager.PlaySound(audioClip, Random.Range(1, 1.5f));
            if (popEffect != null) Instantiate(popEffect, transform.position, Quaternion.identity, null);

            int amount = Random.Range(1, 5);
            float spread = 0.2f;

            for (int i = 0; i < amount; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
                Instantiate(shardPickup, transform.position + randomPos, Quaternion.identity, null);
            }


        }
    }
}
