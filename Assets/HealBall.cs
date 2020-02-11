using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBall : MonoBehaviour
{
    private GameObject healthPickup;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject popEffect;

    private void Start()
    {
        healthPickup = Resources.Load<GameObject>("Pickups/HealthPickup");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject goCollisionRoot = collision.transform.root.gameObject;

        if (goCollisionRoot.tag == "Player")
        {
            if (audioClip != null) AudioManager.PlaySound(audioClip, Random.Range(1, 1.5f));
            if (popEffect != null) Instantiate(popEffect, transform.position, Quaternion.identity, null);

            float spread = 0f;
            Vector3 randomPos = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
            Instantiate(healthPickup, transform.position + randomPos, Quaternion.identity, null);
        }
    }
}
