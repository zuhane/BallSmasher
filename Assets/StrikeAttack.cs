﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeAttack : MonoBehaviour
{
    
    GameObject strike;
    int SH_isAxisInUse = 0, SV_isAxisInUse = 0;
    private void Start()
    {
        strike = Resources.Load<GameObject>("StrikeBox");
        strike.GetComponent<StrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        strike.GetComponent<StrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
    }

    void Update()
    {
        
        if (Input.GetAxisRaw("StrikeHorizontal") < 0 && SH_isAxisInUse >= 0)
        {
            StrikeOut(StrikeBox.FacingDirection.Left);
            SH_isAxisInUse = -1;
        }
        else if (Input.GetAxisRaw("StrikeHorizontal") > 0 && SH_isAxisInUse <= 0)
        {
            StrikeOut(StrikeBox.FacingDirection.Right);
            SH_isAxisInUse = 1;
        }
        else if (Input.GetAxisRaw("StrikeHorizontal") == 0)
        {
            SH_isAxisInUse = 0;
        }

        
        if (Input.GetAxisRaw("StrikeVertical") < 0 && SV_isAxisInUse >= 0)
        {
            StrikeOut(StrikeBox.FacingDirection.Down);
            SV_isAxisInUse = -1;
        }
        else if (Input.GetAxisRaw("StrikeVertical") > 0 && SV_isAxisInUse <= 0)
        {
            StrikeOut(StrikeBox.FacingDirection.Up);
            SV_isAxisInUse = 1;
        }
        else if (Input.GetAxisRaw("StrikeVertical") == 0)
        {
            SV_isAxisInUse = 0;
        }
    }

    private void StrikeOut(StrikeBox.FacingDirection direction)
    {
        strike.GetComponent<StrikeBox>().facingDirection = direction;
        GameObject outStrike = Instantiate(strike, gameObject.transform.position, Quaternion.identity);
    }
}
