using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [HideInInspector] public List<BaseAbility> abilities = new List<BaseAbility>();
    [SerializeField] public List<AbilitySO> AbilityObjects;

    void Start()
    {
        for (int i = 0; i < AbilityObjects.Count; i++)
        {
            var type = Type.GetType(AbilityObjects[i].fileName);
            BaseAbility newAbility = (BaseAbility)Activator.CreateInstance(type);

            abilities.Add(newAbility);
            abilities[i]?.PassStatsFromSO(AbilityObjects[i]);
        }
    }
     
    public void ActivateAbility(int index, GameObject user)
    {
        index--;

        if (abilities.Count-1 < index || abilities[index] != null) return;

        if (abilities[index].CanActivate) 
        {
            abilities[index].Activate(transform.gameObject);
        }
    }

    private void Update()
    {
        foreach (BaseAbility b in abilities)
        {
            b.Update();
        }
    }




}

