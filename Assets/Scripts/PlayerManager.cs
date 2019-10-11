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
        
        //playerControls.Gameplay.Enable();
        

    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

        if (Input.GetButtonDown("Ability1P" + (int)controllerType))
            movement.intent.useAbility1 = true;
        if (Input.GetButtonDown("Ability2P" + (int)controllerType))
            movement.intent.useAbility2 = true;
        if (Input.GetButtonDown("Ability3P" + (int)controllerType))
            movement.intent.useAbility3 = true;


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
}
