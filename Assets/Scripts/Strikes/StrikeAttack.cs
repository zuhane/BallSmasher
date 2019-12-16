using System.Collections;
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

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerManager))]
public class StrikeAttack : MonoBehaviour
{
    Movement movement;
    //GameObject spinStrike, normalStrike;
    PlayerManager playerManager;
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

        movement = GetComponent<Movement>();
        playerManager = GetComponent<PlayerManager>();
        channelSound = attackOrb.GetComponentInChildren<AudioSource>();

    }

    void Update()
    {
        if (attackOrb.fireState != AttackOrb.FireState.Live && attackOrb.fireState != AttackOrb.FireState.Returning)
            OrbChargeSetup();  

    }

    private void OrbChargeSetup()
    {
        if (movement.intent.attack)
        {
            charge += Time.deltaTime;

            attackOrb.liveTimeLimit = (int)(charge * 8 - 3);

            if (attackOrb.fireState == AttackOrb.FireState.Idling)
            {
                attackOrb.fireState = AttackOrb.FireState.Charging;
                attackOrb.damage = rpgStats.attackDamage;

                channelSound.PlayOnce(attackOrb.chargeUpSound);

                strikeDirection =
                          (movement.intent.holdLeft ? FacingDirection.Left :
                          (movement.intent.holdRight ? FacingDirection.Right :
                          (movement.intent.holdDown ? FacingDirection.Down :
                          (movement.intent.holdUp ? FacingDirection.Up :
                          movement.intent.holdClockwiseAttack ? FacingDirection.Clockwise :
                          movement.intent.holdAnticlockwiseAttack ? FacingDirection.Anticlockwise :
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
                    attackOrb.liveTimeLimit = 40;
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
            if (movement.intent.release)
            {
                releaseDetected = true;
                if (strikeDirection == FacingDirection.Down && movement.bumpingFeet)
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

    private AttackOrb NormalStrike(FacingDirection direction)
    {
        //attackOrbObject.GetComponent<AttackOrbIdle>().thisFacingDirection = direction;
        //attackOrbObject.GetComponent<AttackOrbIdle>().damage = rpgStats.attackDamage;
        ////GameObject outStrike = Instantiate(normalStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        //channelSound = attackOrbObject.transform.GetChild(0).GetComponent<AudioSource>();
        return gameObject.GetComponent<AttackOrb>();
    }

    private AttackOrb SpinStrike(FacingDirection direction)
    {
        return null;
        //spinStrike.GetComponent<SpinningStrikeBox>().thisFacingDirection = direction;
        //spinStrike.GetComponent<SpinningStrikeBox>().damage = rpgStats.attackDamage;
        //GameObject outStrike = Instantiate(spinStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        //channelSound = outStrike.transform.GetChild(0).GetComponent<AudioSource>();
        //return outStrike.GetComponent<SpinningStrikeBox>();

    }

}
