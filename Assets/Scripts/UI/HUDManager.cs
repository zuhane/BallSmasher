using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private GameObject playerHUDContainer;
    private GameObject playerHUDRContainer;
    private GameObject playerManager;
    private PlayerSpawnManager playerSpawnManager;

    // Start is called before the first frame update
    void Start()
    {
        playerHUDContainer = GameObject.Find("PlayerHUDs");
        playerHUDRContainer = GameObject.Find("PlayerHUDsR");
        playerManager = GameObject.Find("PlayerManager");
        playerSpawnManager = playerManager.GetComponent<PlayerSpawnManager>();

        for (int i = 0; i < playerSpawnManager.GetPlayerCount(); i++)
        {
            GameObject playerHUD = Resources.Load<GameObject>("UI/PlayerHUDBox");
            playerHUD.GetComponent<HUDBoxPlayerTracker>().player = playerSpawnManager.GetPlayer(i);
            if (playerSpawnManager.GetPlayer(i).GetComponent<InputToIntent>().team == 1)
            {
                Instantiate(playerHUD, playerHUDContainer.transform);
            }
            else
            {
                Instantiate(playerHUD, playerHUDRContainer.transform);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
