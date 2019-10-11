using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BaseAbility
{
    private float dashSpeed = 8;


    public Dash()
    {

    }

    public override void Activate(GameObject player)
    {
        base.Activate(player);
        Debug.Log("Dash worked!");

        if (player.GetComponent<Movement>().intent.left) player.GetComponent<Rigidbody2D>().addX(-dashSpeed);
        else if (player.GetComponent<Movement>().intent.right) player.GetComponent<Rigidbody2D>().addX(dashSpeed);
    }
}
