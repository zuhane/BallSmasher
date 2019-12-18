using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerPhysicsMovement))]
public class Movement : MonoBehaviour
{
    public class Intent
    {
        public bool right { get; set; }
        public bool left { get; set; }
        public bool up { get; set; }
        public bool down { get; set; }
        public bool jump { get; set; }
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
    }

    [HideInInspector] public bool moving, ascending, descending, crouching, wallSliding;
    [HideInInspector] public int currJump;

    [HideInInspector] public PlayerPhysicsMovement playerPhysics;

    [HideInInspector] public Intent intent = new Intent();

    private Vector3 originalScale, flipScale;

    private Vector4 standingScale, crouchingScale;

    [HideInInspector] public bool facingLeft;

    [HideInInspector] public bool brushingFeet, brushingHead, brushingLeft, brushingRight;

    private SpriteRenderer sprRend;

    // Start is called before the first frame update
    void Start()
    {
        standingScale = new Vector4(-0.02f, 0.04f, 0.3f, 0.45f);
        crouchingScale = new Vector4(-0.02f, -0.01f, 0.3f, 0.35f);
        //transform.Find("Graphics").gameObject.AddComponent<PolygonCollider2D>();
        originalScale = transform.localScale;
        flipScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        playerPhysics = GetComponent<PlayerPhysicsMovement>();
        sprRend = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        facingLeft = playerPhysics.facingLeft;

        moving = playerPhysics.running;
        ascending = playerPhysics.ascending;
        descending = playerPhysics.descending;
        crouching = playerPhysics.crouching;
        wallSliding = playerPhysics.wallSliding;
        currJump = playerPhysics.currJump;

        brushingFeet = playerPhysics.brushingFeet;
        brushingHead = playerPhysics.brushingHead;
        brushingLeft = playerPhysics.brushingLeft;
        brushingRight = playerPhysics.brushingRight;

        if (facingLeft)
        {
            sprRend.flipX = true;
        }
        
        else sprRend.flipX = false;

        //if (intent.crouch && bumpingFeet)
        //{
        //    if (gameObject.tag == "Player")
        //    {
        //        collider.offset = new Vector2(crouchingScale.x, crouchingScale.y);
        //        collider.size = new Vector2(crouchingScale.z, crouchingScale.w);
        //    }

        //    crouching = true;
        //    moving = false;

        //}
        //else
        //{
        //    if (gameObject.tag == "Player")
        //    {
        //        collider.offset = new Vector2(standingScale.x, standingScale.y);
        //        collider.size = new Vector2(standingScale.z, standingScale.w);
        //    }


        //    crouching = false;
        //}

        //if (!crouching)
        //{
        //    if (intent.left)
        //    {
        //        graphicsObj.transform.localScale = flipScale;

        //        if (rigid.velocity.x > -maxMoveSpeed.x)
        //            rigid.addX(-(moveSpeed));

        //        facingLeft = true;
        //    }
        //    else if (intent.right)
        //    {
        //        graphicsObj.transform.localScale = originalScale;
        //        if (rigid.velocity.x < maxMoveSpeed.x)
        //            rigid.addX(moveSpeed);

        //        facingLeft = false;
        //    }
        //    else
        //    {
        //        moving = false;
        //    }
        //}



        AbilityManager abilities = GetComponent<AbilityManager>();

        if (abilities != null)
        {
            if (intent.useAbility1) abilities.ActivateAbility(1, gameObject);
            if (intent.useAbility2) abilities.ActivateAbility(2, gameObject);
            if (intent.useAbility3) abilities.ActivateAbility(3, gameObject);
        }

    }



}
