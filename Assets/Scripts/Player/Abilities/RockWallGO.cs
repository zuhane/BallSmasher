using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWallGO : MonoBehaviour
{
    private SmashBrick smashBrick;
    private int currentTick;
    private int ticksBetweenDamage = 30;

    void Start()
    {
        smashBrick = GetComponent<SmashBrick>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTick++;

        if (currentTick >= ticksBetweenDamage)
        {
            currentTick = 0;
            smashBrick.Damage(1);
        }

    }
}
