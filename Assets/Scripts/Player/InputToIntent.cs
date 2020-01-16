using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IntentToAction))]
public class InputToIntent : MonoBehaviour
{ 

    public Animator animator;
    public SpriteRenderer playerRenderer;
    private IntentToAction intentToAction;
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
        intentToAction = GetComponent<IntentToAction>();

    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {

        IntentToAction.Intent prevIntent = intentToAction.intent;

        intentToAction.intent = new IntentToAction.Intent();

        //if (playerControls.Gameplay.Move.ReadValue<Vector2>().x > 0)
        //{
        //    movement.intent.right = true;
        //}
        //else if (playerControls.Gameplay.Move.ReadValue<Vector2>().x < 0)
        //{
        //    movement.intent.left = true;
        //}

        
        if (Input.GetAxis("HorizontalP" + (int)controllerType) < 0)
            intentToAction.intent.left = true;
        if (Input.GetAxis("HorizontalP" + (int)controllerType) > 0)
            intentToAction.intent.right = true;


        if (Input.GetButtonDown("JumpP" + (int)controllerType))
        {
            intentToAction.intent.jump = true;
        }
        if (Input.GetButtonUp("JumpP" + (int)controllerType))
        {
            intentToAction.intent.releaseJump = true;
        }

        if (Input.GetAxis("VerticalP" + (int)controllerType) < 0)
            intentToAction.intent.crouch = true;

        if (Input.GetButtonDown("Ability1P" + (int)controllerType))
            intentToAction.intent.useAbility1 = true;
        if (Input.GetButtonDown("Ability2P" + (int)controllerType))
            intentToAction.intent.useAbility2 = true;
        if (Input.GetButtonDown("Ability3P" + (int)controllerType))
            intentToAction.intent.useAbility3 = true;

        if (Input.GetButtonDown("SwitchWeaponP" + (int)controllerType))
            intentToAction.intent.switchWeapon = true;

        if (Input.GetAxisRaw("ClockwiseHitP" + (int)controllerType) > 0)
        {
            intentToAction.intent.holdClockwiseAttack = intentToAction.intent.attack = true;
        }
        else if (prevIntent.holdClockwiseAttack)
        {
            intentToAction.intent.releaseClockwiseAttack = intentToAction.intent.release = true;
        }

        if (Input.GetAxisRaw("AntiClockwiseHitP" + (int)controllerType) < 0 )
        {
            intentToAction.intent.holdAnticlockwiseAttack = intentToAction.intent.attack = true;
        }
        else if (prevIntent.holdAnticlockwiseAttack)
        {
            intentToAction.intent.releaseAnticlockwiseAttack = intentToAction.intent.release = true;
        }

        if (Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType) < 0)
        {
            intentToAction.intent.holdLeft = intentToAction.intent.attack = true;
        }
        else if (prevIntent.holdLeft)
        {
            intentToAction.intent.releaseLeft = intentToAction.intent.release = true;
        }

        if (Input.GetAxisRaw("StrikeHorizontal" + (int)controllerType) > 0)
        {
            intentToAction.intent.holdRight = intentToAction.intent.attack = true;
        }
        else if (prevIntent.holdRight)
        {
            intentToAction.intent.releaseRight = intentToAction.intent.release = true;
        }

        if (Input.GetAxisRaw("StrikeVertical" + (int)controllerType) < 0)
        {
            intentToAction.intent.holdDown = intentToAction.intent.attack = true;
        }
        else if (prevIntent.holdDown)
        {
            intentToAction.intent.releaseDown = intentToAction.intent.release = true;
        }

        if (Input.GetAxisRaw("StrikeVertical" + (int)controllerType) > 0)
        {
            intentToAction.intent.holdUp = intentToAction.intent.attack = true;
        }
        else if (prevIntent.holdUp)
        {
            intentToAction.intent.releaseUp = intentToAction.intent.release = true;
        }
        
    }

    private void LateUpdate()
    {

        //animator.SetBool("Descending", movement.descending);
        //animator.SetBool("Ascending", movement.ascending);
        //animator.SetBool("IsRunning", movement.moving);
        //animator.SetInteger("JumpCount", movement.currJump);
        //animator.SetBool("Crouching", movement.crouching);
        //animator.SetBool("WallSliding", movement.wallSliding);

    }

    private void OnJump()
    {
        Debug.Log("Growing " + playerNumber);
        intentToAction.intent.jump = true;
    }
}
