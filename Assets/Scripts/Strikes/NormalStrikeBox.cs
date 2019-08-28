using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStrikeBox : StrikeBox
{

    public override void SetDirection(FacingDirection facingDirection)
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        switch (facingDirection)
        {
            case FacingDirection.Up:
                flingDirection = new Vector2(0, 1);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset + 0.2f, transform.localPosition.z);
                break;
            case FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.localPosition = new Vector3(transform.localPosition.x + xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.localPosition = new Vector3(transform.localPosition.x - xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Down:
                flingDirection = new Vector2(0, -1);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset, transform.localPosition.z);
                break;
            case FacingDirection.DownOut:
                flingDirection = new Vector2(0.5f, 1.5f);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localScale = new Vector3(3, 0.8f, 1);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset + 0.2f, transform.localPosition.z);
                _force /= 3;
                break;
        }

        base.SetDirection(facingDirection);
    }

}
