using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability")]
public class AbilitySO : ScriptableObject
{
    public string abilityName;
    public string fileName;

    [Multiline]
    public string description;
    [HideInInspector] public int cooldown;
        
    public float cooldownLimitInSeconds;
    public int shardCost;

    public Sprite icon;

    void Start()
    {
        GenerateDescription();

    }

    public void DrawFlashCard()
    {
        //TODO
    }

    protected virtual void GenerateDescription()
    {
        Debug.Log("Description not generated!");
    }

}
