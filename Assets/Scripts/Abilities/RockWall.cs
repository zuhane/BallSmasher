using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWall : BaseAbility
{

    public RockWall()
    {

    }

    public override void Activate(GameObject player)
    {
        base.Activate(player);
        Debug.Log("Rock wall worked!");

        GameObject RockWall = Resources.Load<GameObject>("Rock Wall");
        RockWall = Instantiate(RockWall, player.transform.position + new Vector3(1, 0), Quaternion.identity);

    }
}
