using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public AbilitySO abilitySO;

    public float Cooldown { get; private set; }
    public bool CanActivate { get; set; } //TODO: Set inside here instead - having to set extenrally because of weird ordering shit going on
    public bool justActivated;

    public virtual void Update()
    { 

        if (!CanActivate)
        {
            Cooldown -= Time.deltaTime;
        }

        if (Cooldown <= 0)
        {
            Cooldown = abilitySO.cooldownLimitInSeconds;
            CanActivate = true;
        }
    }

    public void PassStatsFromSO(AbilitySO inAbilitySO)
    {
        abilitySO = inAbilitySO;
    }

    public virtual void Activate(GameObject player)
    {
        Debug.Log("Base worked!");
        CanActivate = false;
        justActivated = true;
    }

    public virtual void LateUpdate()
    {
        justActivated = false;
    }



}
