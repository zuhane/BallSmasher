using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public AbilitySO abilitySO;

    public float Cooldown { get; private set; }
    public bool CanActivate { get; set; } //TODO: Set inside here instead - having to set extenrally because of weird ordering shit going on

    public event EventHandler Activated;

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

    public virtual void Activate(GameObject player) //sub<----
    {
        Debug.Log("Base worked!");
        CanActivate = false;

        Activated?.Invoke(this, EventArgs.Empty);
    }


}
