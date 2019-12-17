using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public AbilitySO abilitySO;
    public BaseAbility()
    {

    }


    public virtual void Update()
    {
        
        if (abilitySO.cooldown > 0)
            abilitySO.cooldown--;
    }

    public void PassStatsFromSO(AbilitySO inAbilitySO)
    {
        abilitySO = inAbilitySO;
        abilitySO.cooldown = 0;
    }

    public virtual void Activate(GameObject player)
    {
        Debug.Log("Base worked!");
        abilitySO.cooldown = abilitySO.cooldownLimit;
    }
        

    public bool canActivate()
    {


        if(abilitySO.cooldown == 0)
        {
            return true;
        }

        return false;
        
    }


}
