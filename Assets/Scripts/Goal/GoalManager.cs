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

    private GameObject[] balls;
    private GameOver gameOverScreen;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        balls = Resources.LoadAll<GameObject>("Balls");

        gameOverScreen = GameObject.Find("GameOver").GetComponent<GameOver>();

        SpawnRandomBall();
    }

    public void ScoreGoal(int team, int points = 1)
    {
        ScoreKeeper.instance.UpdateScore(team, points);

        audioSource.clip = clips[Random.Range(0, clips.GetLength(0))];
        audioSource.Play();

        if (!gameOverScreen.CheckScoreCount())
        {
            SpawnRandomBall();
        }
    }

    public void SpawnBall(GameObject ball, int numBalls = 1, Vector3? spawnPos = null)
    {
        if (spawnPos == null)
        {
            spawnPos = spawners[(Random.Range(0, spawners.Count))].transform.position;
        }

        for (int i = 0; i < numBalls; i++)
        {
            Instantiate(ball, spawnPos.GetValueOrDefault(), Quaternion.identity);
        }

        camera.GetComponent<SmashBrosCamera>().SetTargets();
    }

    public void SpawnRandomBall(Vector3? spawnPos = null)
    {
        SpawnBall(balls[Random.Range(0, balls.Length)], spawnPos: spawnPos);
    }
}
