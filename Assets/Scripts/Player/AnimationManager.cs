using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateAnimation(IntentToAction.State state)
    {
        animator.SetBool("IsRunning", state.moving);
        animator.SetBool("Ascending", state.ascending);
        animator.SetBool("Descending", state.descending);
        animator.SetBool("Crouching", state.crouching);
        animator.SetBool("WallSliding", state.wallSliding);
        animator.SetInteger("JumpCount", state.currJump);

        if (state.facingLeft) spRenderer.flipX = true;
        else spRenderer.flipX = false;
    }

}
