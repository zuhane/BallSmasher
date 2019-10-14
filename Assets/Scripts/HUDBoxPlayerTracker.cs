using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDBoxPlayerTracker : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    private StatsRPG playerStats;
    private PlayerManager playerManager;

    private void Start()
    {
        playerStats = player.GetComponent<StatsRPG>();
        playerManager = player.GetComponent<PlayerManager>();

    }

    public void Update()
    {
        transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "P" + playerManager.playerNumber;
        transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = playerStats.HP + "/" + playerStats.MaxHP + " HP";
        
    }
}
