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
    DownOut
}

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerManager))]
public class StrikeAttack : MonoBehaviour
{
    Movement movement;
    GameObject spinStrike, normalStrike;
    PlayerManager playerManager;
    StatsRPG rpgStats;

    private bool releaseDetected = false;

    private float chargeLimit = 3f, charge = 1f;
    private float coolDownSeconds = 1f, coolDownTimer = 0f;
    private bool cooling;
    private FacingDirection strikeDirection = FacingDirection.None;

    private void Start()
    {
        rpgStats = GetComponent<StatsRPG>();

        spinStrike = Resources.Load<GameObject>("SpinningStrikeBox");
        spinStrike.GetComponent<SpinningStrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        spinStrike.GetComponent<SpinningStrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
        //normalStrike.GetComponent<SpinningStrikeBox>().dama

        normalStrike = Resources.Load<GameObject>("NormalStrikeBox");
        normalStrike.GetComponent<NormalStrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        normalStrike.GetComponent<NormalStrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;

        movement = GetComponent<Movement>();
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (cooling)
        {
            coolDownTimer += Time.deltaTime;

            if (coolDownTimer >= coolDownSeconds)
            {
                cooling = false;
                coolDownTimer = 0f;
            }
            else
                return;
        }

        if (movement.intent.holdLeft || movement.intent.holdRight || movement.intent.holdUp || movement.intent.holdDown)
        {
            if (strikeDirection == FacingDirection.None)
            {
                strikeDirection =
                        (movement.intent.holdLeft ? FacingDirection.Left :
                        (movement.intent.holdRight ? FacingDirection.Right :
                        (movement.intent.holdDown ? FacingDirection.Down :
                        (movement.intent.holdUp ? FacingDirection.Up :
                        FacingDirection.None))));
            }
            charge += Time.deltaTime;

            if (charge > chargeLimit)
                charge = chargeLimit;
        }

        releaseDetected = false;

        if (strikeDirection != FacingDirection.None)
        { 
            switch (strikeDirection)
            {
                case FacingDirection.Left:
                    if (movement.intent.releaseLeft)
                        releaseDetected = true;
                    break;
                case FacingDirection.Right:
                    if (movement.intent.releaseRight)
                        releaseDetected = true;
                    break;
                case FacingDirection.Up:
                    if (movement.intent.releaseUp)
                        releaseDetected = true;
                    break;
                case FacingDirection.Down:
                    if (movement.intent.releaseDown)
                    {
                        releaseDetected = true;
                        if (movement.bumpingFeet)
                            strikeDirection = FacingDirection.DownOut;
                    }
                    break;
                default:
                    break;
            }

            if (releaseDetected)
                NormalStrike(strikeDirection);
    }

}

private void NormalStrike(FacingDirection direction)
{
    if (charge < 1.5f)
        charge = 1f;

    Debug.Log(charge);

    normalStrike.GetComponent<NormalStrikeBox>().facingDirection = direction;
    normalStrike.GetComponent<NormalStrikeBox>().damage = rpgStats.attackDamage;
    normalStrike.GetComponent<NormalStrikeBox>().force = 5f + (5 * charge);
    GameObject outStrike = Instantiate(normalStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
    cooling = true;
    charge = 1;
    strikeDirection = FacingDirection.None;
}

private void SpinStrike(FacingDirection direction)
{
    spinStrike.GetComponent<SpinningStrikeBox>().facingDirection = direction;
    normalStrike.GetComponent<NormalStrikeBox>().damage = rpgStats.attackDamage;
    GameObject outStrike = Instantiate(spinStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
    cooling = true;
}
}
