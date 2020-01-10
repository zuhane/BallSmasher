/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(IntentToAction))]
[RequireComponent(typeof(InputToIntent))]
public class horseshit : MonoBehaviour
{
    IntentToAction intentToAction;
    StatsRPG rpgStats;
    BaseAttack baseAttackObject;

    private AudioSource channelSound;

    private bool releaseDetected = false;

    private float chargeLimit = 2f, charge = 1f;
    private FacingDirection strikeDirection = FacingDirection.None;

    [SerializeField] [Range(1, 10)] private float rotateFrequency = 2;

    [Range(1f, 10f)] public float strikeForce = 5f;
    [HideInInspector] public FacingDirection thisFacingDirection;

    [HideInInspector] public bool attacking;

    [HideInInspector] protected Vector2 flingDirection;
    [HideInInspector] public Vector2 finalFlingDirection;

    [HideInInspector] public float xOffset = 0f, yOffset = 0f;



    private bool canAttack;

    private void Start()
    {
        rpgStats = GetComponent<StatsRPG>();

        //spinStrike = Resources.Load<GameObject>("SpinningStrikeBox");
        //spinStrike.GetComponent<SpinningStrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        //spinStrike.GetComponent<SpinningStrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        ////normalStrike.GetComponent<SpinningStrikeBox>().dama

        //normalStrike = Resources.Load<GameObject>("NormalStrikeBox");

        intentToAction = GetComponent<IntentToAction>();
        channelSound = baseAttackObject.GetComponentInChildren<AudioSource>();

    }

    void Update()
    {
        //Check if you can attack

        //Check for inputs to decide direction and force



            AttackChargeWithDirection();  

    }

    private void AttackChargeWithDirection()
    {
        if (intentToAction.intent.attack)
        {
            charge += Time.deltaTime;

            baseAttackObject.lifeTimeLimit = (int)(charge * baseAttackObject.distancefactor);

            if (baseAttackObject.fireState == AttackOrb.FireState.Idling)
            {
                baseAttackObject.fireState = AttackOrb.FireState.Charging;
                baseAttackObject.damage = rpgStats.attackDamage;

                channelSound.PlayOnce(baseAttackObject.chargeUpSound);

                strikeDirection =
                          (intentToAction.intent.holdLeft ? FacingDirection.Left :
                          (intentToAction.intent.holdRight ? FacingDirection.Right :
                          (intentToAction.intent.holdDown ? FacingDirection.Down :
                          (intentToAction.intent.holdUp ? FacingDirection.Up :
                          intentToAction.intent.holdClockwiseAttack ? FacingDirection.Clockwise :
                          intentToAction.intent.holdAnticlockwiseAttack ? FacingDirection.Anticlockwise :
                          FacingDirection.None))));

                baseAttackObject.SetDirection(strikeDirection);
            }

            if (charge > chargeLimit)
            {
                charge = chargeLimit;

                if (baseAttackObject.fireState == AttackOrb.FireState.Charging)
                {
                    channelSound.PlayLooped(baseAttackObject.chargedFullySound);
                    baseAttackObject.fireState = AttackOrb.FireState.Charged;
                    baseAttackObject.lifeTimeLimit = 40;
                }
            }

            if (baseAttackObject.thisFacingDirection == FacingDirection.Clockwise || baseAttackObject.thisFacingDirection == FacingDirection.Anticlockwise)
            {
                baseAttackObject.Rotate();
            }
        }

        releaseDetected = false;

        if (strikeDirection != FacingDirection.None)
        {
            if (intentToAction.intent.release)
            {
                releaseDetected = true;
                if (strikeDirection == FacingDirection.Down && intentToAction.state.brushingFeet)
                    baseAttackObject.SetDirection(FacingDirection.DownOut);
            }

            if (releaseDetected)
            {
                channelSound.PlayOnce(baseAttackObject.attackReleaseSound);


                if (charge < 1.5f)
                    charge = 1f;

                //Debug.Log("Euler angles: " + activeStrikeBox.GetComponent<StrikeBox>().transform.rotation.eulerAngles.z);
                //Debug.Log("Final fling direction: " + activeStrikeBox.GetComponent<StrikeBox>().finalFlingDirection);

                baseAttackObject.fireState = AttackOrb.FireState.Live;

                baseAttackObject.SetFlingDirection();
                
                baseAttackObject.force = 0.5f + (10f * charge);

                charge = 1;
                //attackOrb.SetDirection(FacingDirection.None);
                baseAttackObject.attacking = false;
                //attackOrb.GetComponent<SpawnEcho>().enabled = true;
            }
        }
    }

    public void Rotate()
    {
        float angle = rotateFrequency * (thisFacingDirection == FacingDirection.Clockwise ? -1 : 1);
        transform.RotateAround(transform.parent.transform.position, new Vector3(0, 0, 1), angle);
        transform.parent.Find("AttackOrb").RotateAround(transform.parent.transform.position, new Vector3(0, 0, 1), angle);
    }

    public void SwitchAttackOrb()
    {
        baseAttackObject = gameObject.GetComponentInChildren<AttackOrb>();
    }

    public void SetDirection(FacingDirection facingDirection)
    {
        thisFacingDirection = facingDirection;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        switch (facingDirection)
        {
            case FacingDirection.Up:
                flingDirection = new Vector2(0, 1f);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
                break;
            case FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.localPosition = new Vector3(transform.localPosition.x + xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.localPosition = new Vector3(transform.localPosition.x - xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Down:
                flingDirection = new Vector2(0, -1);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset, transform.localPosition.z);
                break;
            case FacingDirection.DownOut:
                flingDirection = new Vector2(1, .85f);
                if (transform.parent.GetComponent<IntentToAction>().state.facingLeft) flingDirection.x *= -1;
                //force *= 5;
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset - 0.1f, transform.localPosition.z);
                break;

            case FacingDirection.Clockwise:
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
                break;
            case FacingDirection.Anticlockwise:
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
                break;
        }
    }

    public void SetFlingDirection()
    {
        if (thisFacingDirection == FacingDirection.Clockwise || thisFacingDirection == FacingDirection.Anticlockwise)
        {
            float inputValue = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            flingDirection = new Vector2(-(float)Math.Sin(inputValue), (float)Math.Cos(inputValue)).normalized;
        }

    }

}
*/