using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum team
{
    red = 1,
    green = 2,
    blue = 3,
    yellow = 4
}

[RequireComponent(typeof(Movement))]
public class PlayerManager : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer playerRenderer;
    [SerializeField] private Movement movement;
    public int playerNumber = 1;
    public team team = team.red;

    public ControllerType controllerType;

    [SerializeField] private int dashRechargeTime = 200;
    private int dashRegenCounter;
    private bool dashOnCooldown = false;

    int SH_isAxisInUse = 0, SV_isAxisInUse = 0;

    public enum ControllerType
    {
        None,
        Keyboard,
        Pad1,
        Pad2,
        Pad3,
        Pad4
    }

    void Start()
    {

    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dashOnCooldown)
        {
            dashRegenCounter++;

            if (dashRegenCounter >= dashRechargeTime)
            {
                dashRegenCounter = 0;
                dashOnCooldown = false;
            }
        }

        Movement.Intent prevIntent = movement.intent;

        movement.intent = new Movement.Intent();

        if (Input.GetAxis("HorizontalP" + (int)controllerType) < 0)
            movement.intent.left = true;
        if (Input.GetAxis("HorizontalP" + (int)controllerType) > 0)
            movement.intent.right = true;


        if (Input.GetButtonDown("JumpP" + (int)controllerType))
            movement.intent.jump = true;
        if (Input.GetAxis("VerticalP" + (int)controllerType) < 0)
            movement.intent.crouch = true;

        if (Input.GetButtonDown("DashP" + (int)controllerType) && !dashOnCooldown)
        {
            dashOnCooldown = true;
            movement.intent.dash = true;
        }

        if (Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType) < 0)
        {
            movement.intent.holdLeft = true;
        }
        else if (prevIntent.holdLeft)
        {
            movement.intent.releaseLeft = true;
        }

        if (Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType) > 0)
        {
            movement.intent.holdRight = true;
        }
        else if (prevIntent.holdRight)
        {
            movement.intent.releaseRight = true;
        }

        if (Input.GetAxisRaw("StrikeVertical" + (int)controllerType) < 0)
        {
            movement.intent.holdDown = true;
        }
        else if (prevIntent.holdDown)
        {
            movement.intent.releaseDown = true;
        }

        if (Input.GetAxisRaw("StrikeVertical" + (int)controllerType) > 0)
        {
            movement.intent.holdUp = true;
        }
        else if (prevIntent.holdUp)
        {
            movement.intent.releaseUp = true;
        }

        //if (Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType) < 0 && SH_isAxisInUse >= 0)
        //{
        //    //Debug.Log(Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType));
        //    movement.intent.hitLeft = true;
        //    SH_isAxisInUse = -1;
        //}
        //else if (Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType) > 0 && SH_isAxisInUse <= 0)
        //{
        //    //Debug.Log(Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType));
        //    movement.intent.hitRight = true;
        //    SH_isAxisInUse = 1;
        //}
        //else if (Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType) == 0)
        //{
        //    //Debug.Log(Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType));
        //    SH_isAxisInUse = 0;
        //}


        //if (Input.GetAxisRaw("StrikeVertical" + (int)controllerType) < 0 && SV_isAxisInUse >= 0)
        //{
        //    movement.intent.hitDown = true;
        //    SV_isAxisInUse = -1;
        //}
        //else if (Input.GetAxisRaw("StrikeVertical" + (int)controllerType) > 0 && SV_isAxisInUse <= 0)
        //{
        //    movement.intent.hitUp = true;
        //    SV_isAxisInUse = 1;
        //}
        //else if (Input.GetAxisRaw("StrikeVertical" + (int)controllerType) == 0)
        //{
        //    SV_isAxisInUse = 0;
        //}
    }

    private void LateUpdate()
    {

        animator.SetBool("Descending", movement.descending);
        animator.SetBool("Ascending", movement.ascending);
        animator.SetBool("IsRunning", movement.moving);
        animator.SetInteger("JumpCount", movement.currJump);
        animator.SetBool("Crouching", movement.crouching);
        animator.SetBool("WallSliding", movement.wallSliding);

    }

}
