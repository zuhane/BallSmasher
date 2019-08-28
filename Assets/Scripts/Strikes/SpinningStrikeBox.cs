using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinningStrikeBox : StrikeBox
{
    public FacingDirection facingDirection;

    private float rotateFrequency = 25f;
    private float frequency;

    public override void Start()
    {

        frequency = rotateFrequency;

        base.Start();
    }

    public override void Update()
    {

        if (!released)
        {
            Rotate();
        }

        float inputValue = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

        finalFlingDirection = new Vector2((float)Math.Cos(inputValue), (float)Math.Sin(inputValue)).normalized;

        finalFlingDirection *= 10;

        base.Update();
    }

    private void Rotate()
    {
        float angle = (180 / frequency) * (thisFacingDirection == FacingDirection.Clockwise ? -1 : 1);
        transform.RotateAround(transform.parent.transform.position, new Vector3(0, 0, 1), angle);

    }

    public override void SetDirection(FacingDirection facingDirection)
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset + 0.2f, transform.localPosition.z);

        base.SetDirection(facingDirection);
    }

}
