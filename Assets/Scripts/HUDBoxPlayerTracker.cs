using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDBoxPlayerTracker : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    private StatsRPG playerStats;

    private void Start()
    {
        playerStats = player.GetComponent<StatsRPG>();
    }

    public void Update()
    {
        transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = playerStats.HP + "/" + playerStats.MaxHP;
     
    }
}
