using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOrb : BaseBoomerang
{

    protected override void CollisionReaction(Collider2D collision, GameObject collisionRoot, Rigidbody2D rigidbody)
    {
        Ball ballHit = collisionRoot.GetComponent<Ball>();
        if (ballHit != null)
        {
            rigidbody.AddForce((velocity * 25) * impactForce);
            ballHit.TakeDamage(damage);
            if (charged)
            {
                ballHit.ElectrifyBall();
            }
        }

        PlayerPhysicsMovement playerPhysics = collisionRoot.GetComponent<PlayerPhysicsMovement>();
        if (playerPhysics != null)
        {
            playerPhysics.AddVelocity((velocity * 10) * impactForce / 5);
        }

        StatsRPG stats = collisionRoot.GetComponent<StatsRPG>();
        if (stats != null)
        {
            Debug.Log("Damage dealth: " + finalDamage);
            stats.TakeDamage(finalDamage);
        }

        base.CollisionReaction(collision, collisionRoot, rigidbody);
    }

}
