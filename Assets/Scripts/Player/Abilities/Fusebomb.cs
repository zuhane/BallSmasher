﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusebomb : BaseAbility
{
    private float dashSpeed = 10;


    public Fusebomb()
    {

    }

    public override void Activate(GameObject player)
    {
        base.Activate(player);
        Debug.Log("Fuse bomb worked!");

        //AudioManager.PlaySound("Woosh", Random.Range(0.8f, 1.2f));

        //if (player.GetComponent<IntentToAction>().state.facingLeft) player.GetComponent<PlayerPhysicsMovement>().AddVelocity(new Vector2(-dashSpeed, 0));
        //else if (!player.GetComponent<IntentToAction>().state.facingLeft) player.GetComponent<PlayerPhysicsMovement>().AddVelocity(new Vector2(dashSpeed, 0));

        //GameObject echo = Resources.Load<GameObject>("EchoTrailSpawner");
        //Instantiate(echo, player.transform.GetChild(0).transform);
    }
}
