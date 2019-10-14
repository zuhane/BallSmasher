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

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerManager))]
public class StrikeAttack : MonoBehaviour
{
    Movement movement;
    GameObject spinStrike, normalStrike;
    PlayerManager playerManager;
    StatsRPG rpgStats;

    private AudioSource channelSound;
    private StrikeBox activeStrikeBox;

    private bool releaseDetected = false;

    private float chargeLimit = 2f, charge = 1f;
    private float coolDownSeconds = 0.3f, coolDownTimer = 0f;
    private bool cooling;
    private FacingDirection strikeDirection = FacingDirection.None;

    private void Start()
    {
        rpgStats = GetComponent<StatsRPG>();

        spinStrike = Resources.Load<GameObject>("SpinningStrikeBox");
        spinStrike.GetComponent<SpinningStrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        spinStrike.GetComponent<SpinningStrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        //normalStrike.GetComponent<SpinningStrikeBox>().dama

        normalStrike = Resources.Load<GameObject>("NormalStrikeBox");
        normalStrike.GetComponent<NormalStrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        normalStrike.GetComponent<NormalStrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        movement = GetComponent<Movement>();
        playerManager = GetComponent<PlayerManager>();
        channelSound = activeStrikeBox.GetComponent<AudioSource>();

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

        if (movement.intent.holdLeft || movement.intent.holdRight || movement.intent.holdUp || movement.intent.holdDown || movement.intent.holdClockwiseAttack || movement.intent.holdAnticlockwiseAttack)
        {
            if (strikeDirection == FacingDirection.None)
            {
                strikeDirection =
                        (movement.intent.holdLeft ? FacingDirection.Left :
                        (movement.intent.holdRight ? FacingDirection.Right :
                        (movement.intent.holdDown ? FacingDirection.Down :
                        (movement.intent.holdUp ? FacingDirection.Up :
                        movement.intent.holdClockwiseAttack ? FacingDirection.Clockwise :
                        movement.intent.holdAnticlockwiseAttack ? FacingDirection.Anticlockwise :
                        FacingDirection.None))));
            }
            charge += Time.deltaTime;

            if (activeStrikeBox == null)
            {
                if (strikeDirection == FacingDirection.Anticlockwise || strikeDirection == FacingDirection.Clockwise)
                    activeStrikeBox = SpinStrike(strikeDirection);
                else
                    activeStrikeBox = NormalStrike(strikeDirection);

                channelSound.clip = activeStrikeBox.chargeUpSound;
                channelSound.loop = false;
                channelSound.Play();
            }

            if (charge > chargeLimit)
            {
                charge = chargeLimit;

                if (!activeStrikeBox.charged)
                {
                    if (channelSound != null)
                    {
                        channelSound.clip = activeStrikeBox.chargedFullySound;
                        channelSound.loop = true;
                        channelSound.Play();
                    }

                    activeStrikeBox.charged = true;
                    activeStrikeBox.damage = activeStrikeBox.damage * 2;
                }
            }

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
                            activeStrikeBox.SetDirection(FacingDirection.DownOut);
                    }
                    break;
                case FacingDirection.Clockwise:
                    if (movement.intent.releaseClockwiseAttack)
                        releaseDetected = true;
                    break;
                case FacingDirection.Anticlockwise:
                    if (movement.intent.releaseAnticlockwiseAttack)
                        releaseDetected = true;
                    break;
                default:
                    break;
            }

            if (releaseDetected)
            {
                channelSound.Stop();
                channelSound.clip = activeStrikeBox.attackReleaseSound;
                channelSound.loop = false;
                channelSound.Play();

                if (charge < 1.5f)
                    charge = 1f;

                Debug.Log("Euler angles: " + activeStrikeBox.GetComponent<StrikeBox>().transform.rotation.eulerAngles.z);
                Debug.Log("Final fling direction: " + activeStrikeBox.GetComponent<StrikeBox>().finalFlingDirection);

                activeStrikeBox.released = true;
                activeStrikeBox.force = 0.5f + (4f * charge);
                charge = 1;
                activeStrikeBox = null;
                transform.Find("AttackOrb").localPosition = Vector3.zero;
                cooling = true;
                strikeDirection = FacingDirection.None;
            }
        }

    }

    private NormalStrikeBox NormalStrike(FacingDirection direction)
    {
        normalStrike.GetComponent<NormalStrikeBox>().thisFacingDirection = direction;
        normalStrike.GetComponent<NormalStrikeBox>().damage = rpgStats.attackDamage;
        GameObject outStrike = Instantiate(normalStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        channelSound = outStrike.transform.GetChild(0).GetComponent<AudioSource>();
        return outStrike.GetComponent<NormalStrikeBox>();
    }

    private SpinningStrikeBox SpinStrike(FacingDirection direction)
    {
        spinStrike.GetComponent<SpinningStrikeBox>().thisFacingDirection = direction;
        spinStrike.GetComponent<SpinningStrikeBox>().damage = rpgStats.attackDamage;
        GameObject outStrike = Instantiate(spinStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        channelSound = outStrike.transform.GetChild(0).GetComponent<AudioSource>();
        return outStrike.GetComponent<SpinningStrikeBox>();

    }

}
