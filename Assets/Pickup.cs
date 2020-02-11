using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        HP,
        Energy,
        Shards
    }

    [SerializeField] protected PickupType pickupType;
    [SerializeField] public int quantity;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject pickupParticleEffect;

    private enum MoveState
    {
        Still,
        Exploding,
        TravellingToPlayer
    }
    private MoveState moveState = MoveState.Still;
    private GameObject playerToReach;

    private float lerpReturnSpeed;
    private Vector2 velocity;
    private float explosionTimeCount, explosionTimeLimit = .2f;
    private float explosionForce = .06f;
    private float rotationSpeed;

    private AudioClip genericGrabSound;

    private void Start()
    {
        genericGrabSound = Resources.Load<AudioClip>("SFX/itemgrab");
    }

    protected virtual void Collect()
    {
       
        AudioManager.PlaySound(genericGrabSound);
        if (pickupType == PickupType.HP) playerToReach.GetComponent<StatsRPG>().Heal(1);
        if (pickupType == PickupType.Energy) playerToReach.GetComponent<StatsRPG>().ChangeMP(1);
        if (pickupType == PickupType.Shards) playerToReach.GetComponent<StatsRPG>().ChangeShards(1);

        if (pickupParticleEffect != null)
        {
            //TODO: Redo this. Just wanted to get the transfers working :)


            Instantiate(pickupParticleEffect, transform.position, Quaternion.identity, playerToReach.transform);
        }

    }

    private void Update()
    {
        switch (moveState)
        {
            case MoveState.Exploding:
                explosionTimeCount += Time.deltaTime;
                velocity *= 0.96f;
                transform.position += new Vector3(velocity.x, velocity.y, 0);

                if (explosionTimeCount >= explosionTimeLimit)
                {
                    rotationSpeed *= -1;
                    moveState = MoveState.TravellingToPlayer;
                }

                break;
            case MoveState.TravellingToPlayer:
                lerpReturnSpeed += 0.01f;
                Vector2 lerp = new Vector2(Mathf.Lerp(transform.position.x, playerToReach.transform.position.x, lerpReturnSpeed), Mathf.Lerp(transform.position.y, playerToReach.transform.position.y, lerpReturnSpeed));

                transform.position = lerp;

                if (lerpReturnSpeed > 0.2f)
                {
                    Collect();
                    Destroy(gameObject);
                }
                break;
        }

        transform.Rotate(0, 0, rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Layer.Player && moveState == MoveState.Still)
        {
            AudioManager.PlaySound(pickupSound);
            moveState = MoveState.Exploding;
            playerToReach = collision.gameObject;
            velocity = new Vector2(Random.Range(-explosionForce, explosionForce), Random.Range(-explosionForce, explosionForce));
            rotationSpeed = Random.Range(-5, 5);
        }

    }

}
