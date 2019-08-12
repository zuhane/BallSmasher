using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    private GameObject[] players;
    private List<GameObject> playerSpawners;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawners = new List<GameObject>();
        playerSpawners.AddRange(GameObject.FindGameObjectsWithTag("PlayerSpawner"));
        //players = new GameObject[GameObject.FindGameObjectsWithTag("Player").GetLength(0)];
        players = new GameObject[LevelData.playerCount];

        for (int i = 0; i < players.GetLength(0); i++)
        {
            GameObject player = Resources.Load<GameObject>("Player");
            Instantiate(player, playerSpawners[Random.Range(0, playerSpawners.Count - 1)].transform.position, Quaternion.identity);
            player.GetComponent<PlayerManager>().playerNumber = i + 1;

            switch (player.GetComponent<PlayerManager>().playerNumber)
            {
                case 1:
                    player.GetComponent<PlayerManager>().controllerType = PlayerManager.ControllerType.Keyboard;
                    break;
                case 2:
                    player.GetComponent<PlayerManager>().controllerType = PlayerManager.ControllerType.Pad1;
                    break;
            }

            players[0] = player;
        }

        RefreshPlayers();
    }

    private void RefreshPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void HandleDeath(int playerNumber)
    {
        GameObject playerToSpawn = null;

        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerManager>().playerNumber == playerNumber) playerToSpawn = p;
        }

        playerToSpawn.transform.position = playerSpawners[Random.Range(0, playerSpawners.Count - 1)].transform.position;

        //Instantiate(playerToSpawn, playerSpawners[Random.Range(0, playerSpawners.Count - 1)].transform.position, Quaternion.identity);



        //RefreshPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
