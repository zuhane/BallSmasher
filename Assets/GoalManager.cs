using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private List<GameObject> spawners = new List<GameObject>();
    private List<GameObject> players = new List<GameObject>();

    private void Start()
    {
        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void ScoreGoal()
    {



        //if (Random.Range(0, 15) == 0)
        //{
        //    players.ForEach(a => a.GetComponent<Rigidbody2D>().gravityScale = 0.5f);
        //}
        //else
        //{
        //    players.ForEach(a => a.GetComponent<Rigidbody2D>().gravityScale = 1f);
        //}

        if (Random.Range(0, 8) == 0)
        {
            SpawnSquid();
        }
        else if (Random.Range(0, 6) == 0)
        {
            SpawnFireball();
        }
        else
        {
            SpawnBall();
        }
    }

    public void SpawnBall(int numBalls = 1)
    {
        Vector3 spawnPos = Vector3.zero;

        spawnPos = spawners[(Random.Range(0, spawners.Count))].transform.position;

        GameObject squid = new GameObject();
        squid = Resources.Load<GameObject>("Ball");
        Instantiate(squid, spawnPos, Quaternion.identity);

    }

    public void SpawnFireball(int numBalls = 1)
    {
        Vector3 spawnPos = Vector3.zero;

        spawnPos = spawners[(Random.Range(0, spawners.Count))].transform.position;

        GameObject squid = new GameObject();
        squid = Resources.Load<GameObject>("FireBall");
        Instantiate(squid, spawnPos, Quaternion.identity);

    }

    public void SpawnSquid(int numBalls = 1)
    {
        Vector3 spawnPos = Vector3.zero;

        spawnPos = spawners[(Random.Range(0, spawners.Count))].transform.position;

        GameObject squid = new GameObject();
        squid = Resources.Load<GameObject>("Squid");
        Instantiate(squid, spawnPos, Quaternion.identity);
    }

}
