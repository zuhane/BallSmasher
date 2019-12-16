using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    PlayerControls controls;

    Vector2 move;
    float moveSpeed = 5f;

    private void Awake()
    {

        //controls = new PlayerControls();

        //controls.Gameplay.Enable();

        //controls.Gameplay.Grow.performed += ctx => Grow();
        //controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    private void Grow()
    {
        
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(move.x, 0, move.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move);
    }

    //private void OnEnable()
    //{
    //    controls.Gameplay.Enable();
    //}

    //private void OnDisable()
    //{
    //    controls.Gameplay.Disable();
    //}
    

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();

        //Debug.Log("Moving! - " + move);
    }

    private void OnGrow()
    {
        Debug.Log("Growing!");
    }

    private void OnCombo()
    {
        Debug.Log("Combo worked!");
    }
}
