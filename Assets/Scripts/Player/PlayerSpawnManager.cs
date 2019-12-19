using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    private List<GameObject> players = new List<GameObject>();
    private List<GameObject> playerSpawners;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawners = new List<GameObject>();
        playerSpawners.AddRange(GameObject.FindGameObjectsWithTag("PlayerSpawner"));
        //players = new GameObject[GameObject.FindGameObjectsWithTag("Player").GetLength(0)];
        //players = new GameObject[LevelData.playerCount];

        for (int i = 0; i < LevelData.playerCount; i++)
        {
            GameObject player = Resources.Load<GameObject>("Player");
            player.GetComponent<PlayerManager>().playerNumber = i + 1;
            player.GetComponent<PlayerManager>().controllerType = (PlayerManager.ControllerType)i + 1;

            if (i == 0) player.GetComponent<PlayerManager>().team = LevelData.player1Team;
            if (i == 1) player.GetComponent<PlayerManager>().team = LevelData.player2Team;
            if (i == 2) player.GetComponent<PlayerManager>().team = LevelData.player3Team;
            if (i == 3) player.GetComponent<PlayerManager>().team = LevelData.player4Team;

            player.GetComponent<PlayerProfile>().SetColour();
            players.Add(Instantiate(player, playerSpawners[Random.Range(0, playerSpawners.Count - 1)].transform.position, Quaternion.identity));



        }

        //RefreshPlayers();
    }


    //public void Update()
    //{
    //    RefreshPlayers();
    //}


    public GameObject GetPlayer(int index)
    {
        return players[index];
    }


    public int GetPlayerCount()
    {
        return players.Count;
    }

    private void RefreshPlayers()
    {
        players.Clear();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void HandleDeath(int playerNumber)
    {

        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerManager>().playerNumber == playerNumber) p.transform.position = Vector3.zero;
            //playerSpawners[Random.Range(0, playerSpawners.Count - 1)].transform.position;
        }



        //Instantiate(playerToSpawn, playerSpawners[Random.Range(0, playerSpawners.Count - 1)].transform.position, Quaternion.identity);



        //RefreshPlayers();
    }

    public void HandleDeath(GameObject player)
    {
        player.transform.position = playerSpawners[Random.Range(0, playerSpawners.Count - 1)].transform.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

}
