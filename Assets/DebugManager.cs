using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnBall(string ball)
    {
        if (ball == null || ball == "")
        {
            GameObject.Find("GoalManager").GetComponent<GoalManager>().SpawnRandomBall();
        }
        else
        {
            GameObject.Find("GoalManager").GetComponent<GoalManager>().SpawnBall(Resources.Load<GameObject>("Balls/" + ball));
        }
    }


}
