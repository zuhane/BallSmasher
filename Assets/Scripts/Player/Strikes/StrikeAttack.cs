﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
    None,
    Up,
    Down,
    Left,
    Right,
    DownOut,
    Clockwise,
    Anticlockwise
}

[RequireComponent(typeof(IntentToAction))]
[RequireComponent(typeof(InputToIntent))]
public class StrikeAttack : MonoBehaviour
{
    IntentToAction intentToAction;
    //GameObject spinStrike, normalStrike;
    InputToIntent playerManager;
    StatsRPG rpgStats;
    AttackOrb attackOrb;

    private AudioSource channelSound;
    //private StrikeBox activeStrikeBox;

    private bool releaseDetected = false;

    private float chargeLimit = 2f, charge = 1f;
    private FacingDirection strikeDirection = FacingDirection.None;

    private void Start()
    {
        rpgStats = GetComponent<StatsRPG>();
        attackOrb = gameObject.GetComponentInChildren<AttackOrb>();
        attackOrb.xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        attackOrb.yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;

        //spinStrike = Resources.Load<GameObject>("SpinningStrikeBox");
        //spinStrike.GetComponent<SpinningStrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        //spinStrike.GetComponent<SpinningStrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        ////normalStrike.GetComponent<SpinningStrikeBox>().dama

        //normalStrike = Resources.Load<GameObject>("NormalStrikeBox");

        intentToAction = GetComponent<IntentToAction>();
        playerManager = GetComponent<InputToIntent>();
        channelSound = attackOrb.GetComponentInChildren<AudioSource>();

    }

    void Update()
    {
        if (attackOrb.fireState != AttackOrb.FireState.Live && attackOrb.fireState != AttackOrb.FireState.Returning)
            OrbChargeSetup();  

    }

    private void OrbChargeSetup()
    {
        if (intentToAction.intent.attack)
        {
            charge += Time.deltaTime;

            attackOrb.lifeTimeLimit = (int)(charge * attackOrb.distancefactor);

            if (attackOrb.fireState == AttackOrb.FireState.Idling)
            {
                attackOrb.fireState = AttackOrb.FireState.Charging;
                attackOrb.damage = rpgStats.attackDamage;

                channelSound.PlayOnce(attackOrb.chargeUpSound);

                strikeDirection =
                          (intentToAction.intent.holdLeft ? FacingDirection.Left :
                          (intentToAction.intent.holdRight ? FacingDirection.Right :
                          (intentToAction.intent.holdDown ? FacingDirection.Down :
                          (intentToAction.intent.holdUp ? FacingDirection.Up :
                          intentToAction.intent.holdClockwiseAttack ? FacingDirection.Clockwise :
                          intentToAction.intent.holdAnticlockwiseAttack ? FacingDirection.Anticlockwise :
                          FacingDirection.None))));

                attackOrb.SetDirection(strikeDirection);
            }

            if (charge > chargeLimit)
            {
                charge = chargeLimit;

                if (attackOrb.fireState == AttackOrb.FireState.Charging)
                {
                    channelSound.PlayLooped(attackOrb.chargedFullySound);
                    attackOrb.fireState = AttackOrb.FireState.Charged;
                    attackOrb.lifeTimeLimit = 40;
                }
            }

            if (attackOrb.thisFacingDirection == FacingDirection.Clockwise || attackOrb.thisFacingDirection == FacingDirection.Anticlockwise)
            {
                attackOrb.Rotate();
            }
        }

        releaseDetected = false;

        if (strikeDirection != FacingDirection.None)
        {
            if (intentToAction.intent.release)
            {
                releaseDetected = true;
                if (strikeDirection == FacingDirection.Down && intentToAction.state.brushingFeet)
                    attackOrb.SetDirection(FacingDirection.DownOut);
            }

            if (releaseDetected)
            {
                channelSound.PlayOnce(attackOrb.attackReleaseSound);


                if (charge < 1.5f)
                    charge = 1f;

                //Debug.Log("Euler angles: " + activeStrikeBox.GetComponent<StrikeBox>().transform.rotation.eulerAngles.z);
                //Debug.Log("Final fling direction: " + activeStrikeBox.GetComponent<StrikeBox>().finalFlingDirection);

                attackOrb.fireState = AttackOrb.FireState.Live;

                attackOrb.SetFlingDirection();
                attackOrb.force = 0.5f + (10f * charge);
                charge = 1;
                //attackOrb.SetDirection(FacingDirection.None);
                attackOrb.attacking = false;
                //attackOrb.GetComponent<SpawnEcho>().enabled = true;
            }
        }
    }

}
