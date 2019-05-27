using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rigidBody != null && collision.gameObject.tag == "Bouncy")
        {
            Destroy(collision.gameObject);

            GameObject.Find("GoalManager").GetComponent<GoalManager>().ScoreGoal();
        }
    }

}
