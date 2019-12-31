using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public int team;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        GameObject goCollisionRoot = collision.transform.root.gameObject;
        if (goCollisionRoot.tag == "Player")
        {
            InputToIntent player = goCollisionRoot.GetComponent<InputToIntent>();
            int playerTeam = player.team;

            if (playerTeam != team)
            {
                goCollisionRoot.GetComponent<StatsRPG>().TakeDamage(1);
            }

        }
    }

}
