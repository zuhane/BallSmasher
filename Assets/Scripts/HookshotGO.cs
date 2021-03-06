﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotGO : MonoBehaviour
{
    private LineRenderer line;
    private GameObject player;
    private GameObject attackOrb;
    //private StrikeAttack strikeAttack;

    private Transform grabbedObject;
    private Transform grabbedObjectParent;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.root.gameObject;
        attackOrb = player.transform.Find("AttackOrb").gameObject;
        attackOrb.SetActive(false);
        attackOrb.transform.SetParent(null);
        //strikeAttack = player.GetComponent<StrikeAttack>();

        //strikeAttack.SwitchAttackOrb();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void BallhitTrigger(Collider2D collision)
    {
        GameObject goCollisionRoot = collision.transform.root.gameObject;
        if (goCollisionRoot == player) { return; }

        grabbedObject = goCollisionRoot.transform;
        grabbedObjectParent = goCollisionRoot.transform.parent;
        grabbedObject.transform.SetParent(gameObject.transform);
        //grabbedObject.transform.localPosition = new Vector3(0, 0);

    }

    public void HookshotReturned()
    {
        if (grabbedObject != null)
        {
            grabbedObject.parent = grabbedObjectParent;
            Rigidbody2D tempRigid = GetComponent<Rigidbody2D>();
            Collider2D tempColl = GetComponent<Collider2D>();

            tempRigid.velocity = Vector2.zero;

            //tempRigid.position = player.transform.position - new Vector3(tempColl.bounds.size.x / 2, 0, 0);

        }
        
        attackOrb.SetActive(true);
        attackOrb.transform.SetParent(player.transform);
        attackOrb.transform.localPosition = new Vector3(0,0);

        Destroy(gameObject);
        //strikeAttack.SwitchAttackOrb();
    }
}
