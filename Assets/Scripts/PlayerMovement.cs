using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer playerRenderer;
    [SerializeField] private Movement movement;

    void Start()
    {

    }

    void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        movement.intent = new Movement.Intent();

        if (Input.GetAxis("Horizontal") < 0)
            movement.intent.left = true;
        if (Input.GetAxis("Horizontal") > 0)
            movement.intent.right = true;


        if (Input.GetButtonDown("Jump"))
            movement.intent.jump = true;
        if (Input.GetAxis("Vertical") < 0)
            movement.intent.crouch = true;
    }

    private void LateUpdate()
    {
        
        animator.SetBool("Falling", movement.falling);
        animator.SetBool("Jumping", movement.jumping);
        animator.SetBool("IsRunning", movement.moving);
        animator.SetInteger("JumpCount", movement.currJump);
        animator.SetBool("Crouching", movement.crouching);
        
    }
    
}
