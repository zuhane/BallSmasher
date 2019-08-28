using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerManager : MonoBehaviour
{
    private PlayerControls playerControls;
    public PlayerInput playerInput;

    public Animator animator;
    public SpriteRenderer playerRenderer;
    [SerializeField] private Movement movement;
    [HideInInspector] public int playerNumber = 1;
    [HideInInspector] public int team = 1;

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
        
        playerControls.Gameplay.Enable();
        

    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("UPDATE!");

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

        //if (playerControls.Gameplay.Move.ReadValue<Vector2>().x > 0)
        //{
        //    movement.intent.right = true;
        //}
        //else if (playerControls.Gameplay.Move.ReadValue<Vector2>().x < 0)
        //{
        //    movement.intent.left = true;
        //}

        
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


        if (Input.GetButton("ClockwiseHitP" + (int)controllerType))
        {
            movement.intent.holdClockwiseAttack = true;
        }
        else if (prevIntent.holdClockwiseAttack)
        {
            movement.intent.releaseClockwiseAttack = true;
        }

        if (Input.GetButton("AntiClockwiseHitP" + (int)controllerType))
        {
            movement.intent.holdAnticlockwiseAttack = true;
        }
        else if (prevIntent.holdAnticlockwiseAttack)
        {
            movement.intent.releaseAnticlockwiseAttack = true;
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

    private void OnJump()
    {
        Debug.Log("Growing " + playerNumber);
        movement.intent.jump = true;
    }


    private void OnDash()
    {
        Debug.Log("Combo " + playerNumber);
        movement.intent.dash = true;
    }

    private void OnMove(InputValue direction)
    {



        //movement.intent.right = false;
        //movement.intent.left = false;



        //Vector2 tmpDir = direction.Get<Vector2>();
        //if (tmpDir.x < 0)
        //    movement.intent.left = true;
        //else if (tmpDir.x > 0)
        //    movement.intent.right = true;


        //Debug.Log("ON MOVE! " + $"Direction: {tmpDir}");
    }
}
