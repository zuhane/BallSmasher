using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDBoxPlayerTracker : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    private StatsRPG playerStats;
    private InputToIntent playerManager;
    private PlayerProfile playerProfile;

    private void Start()
    {
        playerStats = player.GetComponent<StatsRPG>();
        playerManager = player.GetComponent<InputToIntent>();
        playerProfile = player.GetComponent<PlayerProfile>();
        //Debug.Log($"player colour {playerProfile.colour}");
        transform.GetComponent<Image>().material = player.GetComponentInChildren<SpriteRenderer>().material;
        
        //Debug.Log($"player Hud {transform.parent.GetComponent<Image>().color}");
    }

    public void Update()
    {
        transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "P" + playerManager.playerNumber;
        transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = playerStats.HP + "/" + playerStats.MaxHP + " HP";
        
    }
}
