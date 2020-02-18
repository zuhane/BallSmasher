using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IntentToAction))]
public class PlayerPhysicsMovement : PhysicsObject
{
    //Animator stuff
    [HideInInspector] public bool crouching;
    [HideInInspector] public bool wallSliding;
    [HideInInspector] public bool sliding;


    public IntentToAction.Intent intent;
    //TODO:
    public IntentToAction.State state;

    //Modifiable
    [SerializeField] [Range(0, 10)] private int maxJumps = 2;
    [HideInInspector] public int currJump;
    [HideInInspector] public bool facingLeft;
    [Range(0, 100)] public float jumpTakeOffSpeed = 30f;
    [Range(0, 10)] public float acceleration = 0.0013f;

    public void AddVelocity(Vector2 inVelocity)
    {
        targetVelocity = inVelocity;
    }

    protected override void ComputeVelocity()
    {
        if (brushingFeet || brushingLeft || brushingRight)
            currJump = 0;

        if (crouching || (!intent.left && !intent.right))
        {
            if (brushingFeet) targetVelocity.x *= velocityXDecayGrounded;
            else targetVelocity.x *= velocityXDecayAerial;
        }
        else
        {
            if (intent.left)
            {
                if (targetVelocity.x > -maxRunSpeedX)
                    targetVelocity.x -= acceleration;
            }
            else if (intent.right)
            {
                if (targetVelocity.x < maxRunSpeedX)
                    targetVelocity.x += acceleration;
            }
        }




        if (intent.crouch && brushingFeet) crouching = true; else crouching = false;

        if ((pushingLeft || pushingRight) && aerial) wallSliding = true; else wallSliding = false;


        //Wall sliding //TODO: Use Physics Material to apply Friction.
        if (wallSliding && descending) currentGravModifier = 0.1f;
        else currentGravModifier = originalGravModifier;

        if (intent.jump && currJump < maxJumps)
        {
            targetVelocity.y = jumpTakeOffSpeed;

            //Wall jumping
            if (brushingLeft && aerial && !wallSliding)
            {
                targetVelocity.x += jumpTakeOffSpeed;
            }
            if (brushingRight && aerial && !wallSliding)
            {
                targetVelocity.x -= jumpTakeOffSpeed;
            }
        }
        else if (intent.releaseJump)
        {
            currJump++;
            if (ascending)
            {
                targetVelocity.y = targetVelocity.y * 0.5f;
            }
        }
        if (movingLeft) facingLeft = true;
        if (movingRight) facingLeft = false;
    }
}
