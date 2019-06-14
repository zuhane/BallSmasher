using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Movement))]
[RequireComponent(typeof(PlayerManager))]
public class StrikeAttack : MonoBehaviour
{
    Movement movement;
    GameObject spinStrike, normalStrike;
    PlayerManager playerManager;

    private void Start()
    {
        spinStrike = Resources.Load<GameObject>("SpinningStrikeBox");
        spinStrike.GetComponent<SpinningStrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        spinStrike.GetComponent<SpinningStrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;

        normalStrike = Resources.Load<GameObject>("NormalStrikeBox");
        normalStrike.GetComponent<NormalStrikeBox>().xOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        normalStrike.GetComponent<NormalStrikeBox>().yOffset = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;

        movement = GetComponent<Movement>();
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (movement.intent.hitLeft)
        {
            Debug.Log("Hitting left");
            SpinStrike(SpinningStrikeBox.FacingDirection.Left);
        }

        if (movement.intent.hitRight)
        {
            Debug.Log("Hitting left");
            SpinStrike(SpinningStrikeBox.FacingDirection.Right);
        }

        if (movement.intent.hitUp)
        {
            Debug.Log("Hitting left");
            NormalStrike(SpinningStrikeBox.FacingDirection.Up);
        }

        if (movement.intent.hitDown)
        {
            if (movement.bumpingFeet)
            {
                Debug.Log("Hitting left");
                NormalStrike(SpinningStrikeBox.FacingDirection.DownOut);
            }
            else
            {
                Debug.Log("Hitting left");
                NormalStrike(SpinningStrikeBox.FacingDirection.Down);
            }

        }


    }

    private void NormalStrike(SpinningStrikeBox.FacingDirection direction)
    {
        normalStrike.GetComponent<NormalStrikeBox>().facingDirection = direction;
        GameObject outStrike = Instantiate(normalStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
    }

    private void SpinStrike(SpinningStrikeBox.FacingDirection direction)
    {
        spinStrike.GetComponent<SpinningStrikeBox>().facingDirection = direction;
        GameObject outStrike = Instantiate(spinStrike, gameObject.transform.position, Quaternion.identity, gameObject.transform);
    }
}
