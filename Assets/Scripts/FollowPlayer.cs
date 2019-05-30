using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Movement))]
public class FollowPlayer : MonoBehaviour
{

    private Transform playerPos;
    [SerializeField] private Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.intent = new Movement.Intent();
        Vector3 direction = (playerPos.position - transform.position).normalized;

        if (direction.x < 0)
            movement.intent.left = true;
        if (direction.x > 0)
            movement.intent.right = true;
        if (direction.y > 0)
            movement.intent.jump = true;

        


    }
}
