using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private GameObject camera;
    private List<GameObject> spawners = new List<GameObject>();
    private List<GameObject> players = new List<GameObject>();

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    private string[] balls = {
            //"Ball"
            /*,*/"FireBall"
            ,"BeachBall"
            //,"HealBall"
            //,"PlasmaBall"
    };


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        SpawnRandomBall();
    }

    public void ScoreGoal(int team)
    {
        ScoreKeeper.instance.UpdateScore(team);

        audioSource.clip = clips[Random.Range(0, clips.GetLength(0))];
        audioSource.Play();

        SpawnRandomBall();
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

        camera.GetComponent<SmashBrosCamera>().SetTargets();
    }

    public void SpawnRandomBall()
    {
        SpawnBall(BallType: balls[Random.Range(0, balls.Length)]);
    }
}
