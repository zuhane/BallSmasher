using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWall : BaseAbility
{

    public override void Activate(GameObject player)
    {
        base.Activate(player);
        AudioManager.PlaySound("Rockwall", Random.Range(0.8f, 1.2f));
        Debug.Log("Rock wall worked!");

        GameObject RockWall = Resources.Load<GameObject>("AbilityObjects/Rock Wall");

        if (player.GetComponent<IntentToAction>().state.facingLeft) RockWall = Instantiate(RockWall, player.transform.position + new Vector3(-1, 0), Quaternion.identity);
        else
            RockWall = Instantiate(RockWall, player.transform.position + new Vector3(1, 0), Quaternion.identity);

        Instantiate(Resources.Load<GameObject>("Effects/RockwallForm"), RockWall.transform.position, Quaternion.identity);

    }

}
