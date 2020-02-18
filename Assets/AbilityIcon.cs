using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    private int percentOfMaskOverlay = 100;

    private Animator animator;
    public BaseAbility Ability { get; set; }

    private float onePercentOfCooldown;
    private float onePercentOfUIBar;

    private float miniTimer;

    private RectTransform cooldownMaskRect;
    private TextMeshProUGUI cooldownText;

    void Start()    
    {
        animator = GetComponent<Animator>();
        cooldownMaskRect = transform.GetChild(0).GetComponent<RectTransform>();
        cooldownText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        GetComponent<Image>().sprite = Ability.abilitySO.icon;

        onePercentOfCooldown = Ability.abilitySO.cooldownLimitInSeconds / 100;
        onePercentOfUIBar = cooldownMaskRect.sizeDelta.y / 100;

        SetCooldownOverlay(0);
        cooldownText.text = "";

        Ability.Activated += AbilityActivated;
    }

    private void SetCooldownOverlay(float percentageFullness)
    {
        cooldownMaskRect.sizeDelta = new Vector2(cooldownMaskRect.sizeDelta.x, percentageFullness);
    }

    private void AbilityActivated(object sender, EventArgs e)
    {
        percentOfMaskOverlay = 100;
        miniTimer = 0;
    }

    void Update()
    {
        animator.SetBool("CanActivate", Ability.CanActivate);


        //Divide the UI cooldown overlay graphic into bits of 1%. Remove 1% each time the cooldown drops by 1%.
        //Gives a nice smooth transition. Also shows a numerical display of the cooldown time left.
        if (!Ability.CanActivate)
        {
          
            SetCooldownOverlay(onePercentOfUIBar * percentOfMaskOverlay);

            miniTimer += Time.deltaTime;

            if (miniTimer >= onePercentOfCooldown)
            {
                miniTimer = 0;
                percentOfMaskOverlay--;
            }

            cooldownText.text = string.Format("{0:0.0}", Ability.Cooldown).ToString();



        }

        if (Ability.CanActivate)
        {
            SetCooldownOverlay(percentOfMaskOverlay = 0);
            cooldownText.text = "";
        }
        
    }
}
