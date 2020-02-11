using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntentToAction : MonoBehaviour
{
    public class Intent
    {
        public bool right { get; set; }
        public bool left { get; set; }
        public bool up { get; set; }
        public bool down { get; set; }
        public bool jump { get; set; }
        public bool releaseJump { get; set; }
        public bool crouch { get; set; }
        public bool holdRight { get; set; }
        public bool holdLeft { get; set; }
        public bool holdUp { get; set; }
        public bool holdDown { get; set; }
        public bool release { get; set; }
        public bool releaseRight { get; set; }
        public bool releaseLeft { get; set; }
        public bool releaseUp { get; set; }
        public bool releaseDown { get; set; }
        public bool attack { get; set; }
        public bool holdClockwiseAttack { get; set; }
        public bool releaseClockwiseAttack { get; set; }
        public bool holdAnticlockwiseAttack { get; set; }
        public bool releaseAnticlockwiseAttack { get; set; }
        public bool useAbility1 { get; set; }
        public bool useAbility2 { get; set; }
        public bool useAbility3 { get; set; }
        public bool switchWeapon { get; set; }
    }

    public class State
    {
        public bool moving { get; set; }
        public bool ascending { get; set; }
        public bool descending { get; set; }
        public bool crouching { get; set; }
        public bool wallSliding { get; set; }
        public bool facingLeft { get; set; }
        public bool brushingFeet { get; set; }
        public bool brushingHead { get; set; }
        public bool brushingLeft { get; set; }
        public bool brushingRight { get; set; }
        public int currJump { get; set; }
    }

    //Contained systems
    [HideInInspector] public PlayerPhysicsMovement playerPhysics;
    private AbilityManager abilities;
    private AnimationManager animationManager;
    private AttackContainer attackContainer;

    //Bread and butter of the class
    [HideInInspector] public Intent intent = new Intent();
    [HideInInspector] public State state = new State();

    //Graphical doohickeys
    private Vector3 originalScale, flipScale;
    private Vector4 standingScale, crouchingScale;
    private SpriteRenderer sprRend;

    // Start is called before the first frame update
    void Start()
    {
        standingScale = new Vector4(-0.02f, 0.04f, 0.3f, 0.45f);
        crouchingScale = new Vector4(-0.02f, -0.01f, 0.3f, 0.35f);
        originalScale = transform.localScale;
        flipScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        playerPhysics = GetComponent<PlayerPhysicsMovement>();
        sprRend = GetComponentInChildren<SpriteRenderer>();
        abilities = GetComponent<AbilityManager>();
        animationManager = GetComponentInChildren<AnimationManager>();
        attackContainer = GetComponentInChildren<AttackContainer>();
    }

    // Update is called once per frame
    void Update()
    {

        state.facingLeft = playerPhysics.facingLeft;

        state.moving = playerPhysics.running;
        state.ascending = playerPhysics.ascending;
        state.descending = playerPhysics.descending;
        state.crouching = playerPhysics.crouching;
        state.wallSliding = playerPhysics.wallSliding;
        state.currJump = playerPhysics.currJump;

        state.brushingFeet = playerPhysics.brushingFeet;
        state.brushingHead = playerPhysics.brushingHead;
        state.brushingLeft = playerPhysics.brushingLeft;
        state.brushingRight = playerPhysics.brushingRight;


        animationManager.UpdateAnimation(state);


        playerPhysics.intent = intent;

        if (abilities != null)
        {
            if (intent.useAbility1) abilities.ActivateAbility(1, gameObject);
            if (intent.useAbility2) abilities.ActivateAbility(2, gameObject);
            if (intent.useAbility3) abilities.ActivateAbility(3, gameObject);
        }

        if (intent.switchWeapon) 
        { 
            Debug.Log("You switched weapon yo");
            attackContainer.SwitchWeapon();
        }

    }



}
