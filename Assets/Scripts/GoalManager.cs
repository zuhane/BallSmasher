using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private GameObject camera;
    private List<GameObject> spawners = new List<GameObject>();
    private List<GameObject> players = new List<GameObject>();

    private string[] balls = {
            "Ball"
            ,"FireBall"
            ,"BeachBall"
            ,"HealBall"
            ,"PlasmaBall"
    };


    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void ScoreGoal(int team)
    {
        ScoreKeeper.instance.UpdateScore(team);

        SpawnBall(BallType: balls[Random.Range(0, balls.Length)]);

        camera.GetComponent<SmashBrosCamera>().SetTargets();
    }

    public void SpawnBall(int numBalls = 1, string BallType = "Ball")
    {
        Vector3 spawnPos = Vector3.zero;

        spawnPos = spawners[(Random.Range(0, spawners.Count))].transform.position;

        for (int i = 0; i < numBalls; i++)
        {
            GameObject Ball = new GameObject();
            Ball = Resources.Load<GameObject>("Balls/" + BallType);
            Instantiate(Ball, spawnPos, Quaternion.identity);
        }

    }
}
