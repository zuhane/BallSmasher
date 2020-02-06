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

    private GameObject nameText;
    private GameObject hpText;
    private GameObject abilityContainer;

    private void Start()
    {
        playerStats = player.GetComponent<StatsRPG>();
        playerManager = player.GetComponent<InputToIntent>();
        playerProfile = player.GetComponent<PlayerProfile>();
        //Debug.Log($"player colour {playerProfile.colour}");
        transform.GetComponent<Image>().material = player.GetComponentInChildren<SpriteRenderer>().material;

        //Debug.Log($"player Hud {transform.parent.GetComponent<Image>().color}");

        nameText = transform.Find("Name Text").gameObject;
        hpText = transform.Find("HP Text").gameObject;
        abilityContainer = transform.Find("Abilities").gameObject;
        List<BaseAbility> abilities = player.GetComponent<AbilityManager>().abilities;

        for (int i = 0; i < abilities.Count; i++)
        {
            GameObject abilityIcon = Resources.Load<GameObject>("UI/Ability Icon");
            abilityIcon = Instantiate(abilityIcon, abilityContainer.transform);
            abilities[i].CanActivate = true;
            abilityIcon.GetComponent<AbilityIcon>().Ability = abilities[i];
        }
    }

    public void Update()
    {
        nameText.GetComponent<TextMeshProUGUI>().text = "P" + playerManager.playerNumber;
        hpText.GetComponent<TextMeshProUGUI>().text = playerStats.HP + "/" + playerStats.MaxHP + " HP";
        
    }
}
