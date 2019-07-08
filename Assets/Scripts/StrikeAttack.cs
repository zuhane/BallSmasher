using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerManager))]
public class StrikeAttack : MonoBehaviour
{
    Movement movement;
    GameObject spinStrike, normalStrike;
    PlayerManager playerManager;
    StatsRPG rpgStats;

    private int chargeLimit = 120, chargeCounter = 0;
    private int coolDownLimit = 20, coolDownCounter = 0;
    private bool charging, cooling;

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
            coolDownCounter++;
            if (coolDownCounter >= coolDownLimit)
            {
                cooling = false;
                coolDownCounter = 0;
            }
            else
                return;
        }

        if (movement.intent.holdLeft || movement.intent.holdRight || movement.intent.holdUp || movement.intent.holdDown)
        {
            charging = true;
        }
        else
        {
            charging = false;
        }

        if (charging)
        {
            chargeCounter++;

            if (chargeCounter >= chargeLimit)
            {
                chargeCounter = chargeLimit;
            }
        }

        if (!charging)
        {
            if (movement.intent.releaseLeft)
            {
                Debug.Log("Hitting left");
                NormalStrike(SpinningStrikeBox.FacingDirection.Left);
                return;
            }

            if (movement.intent.releaseRight)
            {
                Debug.Log("Hitting left");
                NormalStrike(SpinningStrikeBox.FacingDirection.Right);
                return;
            }

            if (movement.intent.releaseUp)
            {
                Debug.Log("Hitting left");
                NormalStrike(SpinningStrikeBox.FacingDirection.Up);
                return;
            }

            if (movement.intent.releaseDown)
            {
                if (movement.bumpingFeet)
                {
                    NormalStrike(SpinningStrikeBox.FacingDirection.DownOut);
                    return;
                }
                else
                {
                    NormalStrike(SpinningStrikeBox.FacingDirection.Down);
                    return;
                }
            }
        }

    }

    private void NormalStrike(SpinningStrikeBox.FacingDirection direction)
    {
        normalStrike.GetComponent<NormalStrikeBox>().facingDirection = direction;
        normalStrike.GetComponent<NormalStrikeBox>().damage = rpgStats.attackDamage;
        normalStrike.GetComponent<NormalStrikeBox>().force = 0.15f * chargeCounter;
        GameObject outStrike = Instantiate(normalStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        cooling = true;
        chargeCounter = 0;
    }

    private void SpinStrike(SpinningStrikeBox.FacingDirection direction)
    {
        spinStrike.GetComponent<SpinningStrikeBox>().facingDirection = direction;
        normalStrike.GetComponent<NormalStrikeBox>().damage = rpgStats.attackDamage;
        GameObject outStrike = Instantiate(spinStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        cooling = true;
    }
}
