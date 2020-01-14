using System;
using UnityEngine;

public class AttackContainer : MonoBehaviour
{
    private enum ChargeState
    {
        ReadyToFire,
        Charging,
        FullyCharged,
        Returning
    }
    private enum AttackDirection
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

    [SerializeField][Range(0, 5f)] private float rotateFrequency = 5f;

    private AttackDirection attackDirection = AttackDirection.None;
    private ChargeState chargeState = ChargeState.ReadyToFire;

    private GameObject player;
    private IntentToAction intentToAction;
    BaseBoomerang baseBoomerang; //The artifact attached to the attack container, i.e. orb, hook, gun, etc

    private Vector2 attackDirectionVector;
    private Vector2 initialAttackDisplacement;

    private float currentForceChargeAmount;

    private void Start()
    {
        player = transform.root.gameObject;

        intentToAction = player.GetComponent<IntentToAction>();
        initialAttackDisplacement = player.GetComponent<Collider2D>().bounds.size;
        initialAttackDisplacement.y /= 2;

        baseBoomerang = gameObject.GetComponentInChildren<BaseBoomerang>();
    }

    void Update()
    {
        ModifyContainer();

        ModifyAttackObject();
    }

    private bool HoldingWeapon()
    {
        if (GetComponentInChildren<BaseBoomerang>() != null) return true;
        return false;
    }

    private void ModifyContainer()
    {
        if (chargeState == ChargeState.Returning)
        {
            if (HoldingWeapon())
            {
                chargeState = ChargeState.ReadyToFire;
                transform.localPosition = Vector3.zero;
            }
        }

        if (intentToAction.intent.attack)
        {
            if (chargeState == ChargeState.ReadyToFire)
            {
                attackDirection =
                    (intentToAction.intent.holdLeft ? AttackDirection.Left :
                    (intentToAction.intent.holdRight ? AttackDirection.Right :
                    (intentToAction.intent.holdDown ? AttackDirection.Down :
                    (intentToAction.intent.holdUp ? AttackDirection.Up :
                    intentToAction.intent.holdClockwiseAttack ? AttackDirection.Clockwise :
                    intentToAction.intent.holdAnticlockwiseAttack ? AttackDirection.Anticlockwise :
                    AttackDirection.None))));

                chargeState = ChargeState.Charging;
                currentForceChargeAmount = .75f;
                SetAttackDirection();
            }

            if (chargeState == ChargeState.Charging)
            {
                currentForceChargeAmount += Time.deltaTime;

                if (currentForceChargeAmount > baseBoomerang.chargeLimit)
                {
                    chargeState = ChargeState.FullyCharged;
                    baseBoomerang.fireState = BaseBoomerang.FireState.FullyCharged;
                }

                baseBoomerang.fireState = BaseBoomerang.FireState.Charging;
            }

            if (chargeState == ChargeState.Charging || chargeState == ChargeState.FullyCharged)
            {
                if (attackDirection == AttackDirection.Anticlockwise || attackDirection == AttackDirection.Clockwise)
                {
                    Rotate();
                }
            }
        }

        if (intentToAction.intent.release)
        {
            if (chargeState == ChargeState.Charging || chargeState == ChargeState.FullyCharged)
            {
                if (attackDirection == AttackDirection.Down && intentToAction.state.brushingFeet)
                {
                    attackDirection = AttackDirection.DownOut;
                    attackDirectionVector = intentToAction.state.facingLeft ? new Vector2(-1, .85f) : new Vector2(1, .85f);
                }

                if (attackDirection == AttackDirection.Clockwise || attackDirection ==  AttackDirection.Anticlockwise)
                {
                    float inputValue = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
                    attackDirectionVector = new Vector2(-(float)Math.Sin(inputValue), (float)Math.Cos(inputValue)).normalized;
                }

                baseBoomerang.Release(attackDirectionVector, currentForceChargeAmount);
                transform.localPosition = Vector2.zero;
                chargeState = ChargeState.Returning;
            }
        }
    }

    private void ModifyAttackObject()
    {
    }

    private void Rotate()
    {
        float angle = rotateFrequency * (attackDirection == AttackDirection.Clockwise ? -1 : 1);
        transform.RotateAround(player.transform.position, new Vector3(0, 0, 1), angle);
    }

    private void SetAttackDirection()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        switch (attackDirection)
        {
            case AttackDirection.Up:
                attackDirectionVector = new Vector2(0, 1f);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + initialAttackDisplacement.y, transform.localPosition.z);
                break;
            case AttackDirection.Right:
                attackDirectionVector = new Vector2(1, 0);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.localPosition = new Vector3(transform.localPosition.x + initialAttackDisplacement.x, transform.localPosition.y, transform.localPosition.z);
                break;
            case AttackDirection.Left:
                attackDirectionVector = new Vector2(-1, 0);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.localPosition = new Vector3(transform.localPosition.x - initialAttackDisplacement.x, transform.localPosition.y, transform.localPosition.z);
                break;
            case AttackDirection.Down:
                attackDirectionVector = new Vector2(0, -1);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - initialAttackDisplacement.y, transform.localPosition.z);
                break;
            case AttackDirection.Clockwise:
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + initialAttackDisplacement.y, transform.localPosition.z);
                break;
            case AttackDirection.Anticlockwise:
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + initialAttackDisplacement.y, transform.localPosition.z);
                break;
        }
    }



}
