using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWallGO : MonoBehaviour
{
    private SmashBrick smashBrick;
    private float timeTick = 0.4f;

    private Timer timer;

    void Start()
    {
        smashBrick = GetComponent<SmashBrick>();
        timer = Timer.CreateComponent(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.SecondsPassed(timeTick))
        {
            smashBrick.Damage(1);
        }
    }
}
