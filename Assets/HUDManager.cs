using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private GameObject playerHUDContainer;
    private GameObject playerManager;
    private PlayerSpawnManager playerSpawnManager;

    // Start is called before the first frame update
    void Start()
    {
        playerHUDContainer = GameObject.Find("PlayerHUDs");
        playerManager = GameObject.Find("PlayerManager");
        playerSpawnManager = playerManager.GetComponent<PlayerSpawnManager>();

        for (int i = 0; i < playerManager.GetComponent<PlayerSpawnManager>().GetPlayerCount(); i++)
        {
            GameObject playerHUD = Resources.Load<GameObject>("PlayerHUDBox");
            playerHUD.GetComponent<HUDBoxPlayerTracker>().player = playerSpawnManager.GetPlayer(i);
            Instantiate(playerHUD, playerHUDContainer.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
