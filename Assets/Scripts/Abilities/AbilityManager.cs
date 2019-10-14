using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{
    [HideInInspector] private List<BaseAbility> abilities = new List<BaseAbility>();
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
        if (abilities[index].canActivate())
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

