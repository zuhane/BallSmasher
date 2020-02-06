using System.Collections;
using System.Collections.Generic;
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

    void Start()    
    {
        animator = GetComponent<Animator>();
        GetComponent<Image>().sprite = Ability.abilitySO.icon;
        onePercentOfCooldown = Ability.abilitySO.cooldownLimitInSeconds / 100;
        cooldownMaskRect = transform.GetChild(0).GetComponent<RectTransform>();
        onePercentOfUIBar = cooldownMaskRect.sizeDelta.y / 100;
        SetCooldownOverlay(0);

    }

    private void SetCooldownOverlay(float percentageFullness)
    {
        cooldownMaskRect.sizeDelta = new Vector2(cooldownMaskRect.sizeDelta.x, percentageFullness);
    }

    void Update()
    {
        animator.SetBool("CanActivate", Ability.CanActivate);

        if (!Ability.CanActivate)
        {
            if (Ability.justActivated)
            {
                percentOfMaskOverlay = 100;
                miniTimer = 0;
            }
           


            SetCooldownOverlay(onePercentOfUIBar * percentOfMaskOverlay);

            miniTimer += Time.deltaTime;

            if (miniTimer >= onePercentOfCooldown)
            {
                miniTimer = 0;
                percentOfMaskOverlay--;
            }
        }

        if (Ability.CanActivate)
            SetCooldownOverlay(percentOfMaskOverlay = 0);           

    }
}
