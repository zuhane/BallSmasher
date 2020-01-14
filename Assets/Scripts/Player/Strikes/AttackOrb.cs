//using System;

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AttackOrb : BaseAttack
//{

//    public void Update()
//    {

//        if (fireState == FireState.Live)
//        {
//            lifeTimeCounter++;

//            if (lifeTimeCounter >= lifeTimeLimit)
//                fireState = FireState.Returning;
//            else
//                transform.position += new Vector3(finalFlingDirection.normalized.x * speed, finalFlingDirection.normalized.y * speed);
//        }


//        if (fireState == FireState.Returning)
//        {
//            GetComponent<TrailRenderer>().time -= 0.08f;

//            transform.position = new Vector2(Mathf.Lerp(transform.position.x, transform.parent.transform.position.x, speed), Mathf.Lerp(transform.position.y, transform.parent.transform.position.y, speed));

//            if (transform.position.x < transform.parent.transform.position.x + 0.01f && transform.position.x > transform.parent.transform.position.x - 0.01f &&
//                transform.position.y < transform.parent.transform.position.y + 0.01f && transform.position.y > transform.parent.transform.position.y - 0.01f)
//            {
//                //If returning orb is near the sender
//                fireState = FireState.Idling;
//                GetComponent<TrailRenderer>().time = 0.4f;
//                GetComponent<TrailRenderer>().enabled = false;
//                transform.position = transform.parent.transform.position;

//                if (hookshot != null)
//                {
//                    hookshot.HookshotReturned();
//                }
//            }
//        }

//        CheckForHits();
//    }

//}
