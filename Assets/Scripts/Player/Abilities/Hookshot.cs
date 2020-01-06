using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : BaseAbility
{
    
        public override void Activate(GameObject player)
        {
            base.Activate(player);
            //AudioManager.PlaySound("Rockwall", Random.Range(0.8f, 1.2f));
            Debug.Log("Hookshot worked!");
            GameObject hookshot = Resources.Load<GameObject>("Hookshot");
            Instantiate(hookshot, player.transform.position, Quaternion.identity, player.transform);
            //Instantiate(Resources.Load<GameObject>("Effects/RockwallForm"), hookshot.transform.position, Quaternion.identity);
        }

}
