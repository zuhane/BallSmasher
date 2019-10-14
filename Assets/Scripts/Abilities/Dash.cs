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

        AudioManager.PlaySound("Woosh", Random.Range(0.8f, 1.2f));

        if (player.GetComponent<Movement>().intent.left) player.GetComponent<Rigidbody2D>().addX(-dashSpeed);
        else if (player.GetComponent<Movement>().intent.right) player.GetComponent<Rigidbody2D>().addX(dashSpeed);

        GameObject echo = Resources.Load<GameObject>("EchoSpawner");
        Instantiate(echo, player.transform);
    }
}
