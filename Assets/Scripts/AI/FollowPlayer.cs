using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(IntentToAction))]
public class FollowPlayer : MonoBehaviour
{

    private Transform playerPos;
    [SerializeField] private IntentToAction movement;
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> transforms = new List<Transform>();
        transforms.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>());

        playerPos = transforms[Random.Range(0, transforms.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        movement.intent = new IntentToAction.Intent();

        if (playerPos != null)
        {
            Vector3 direction = (playerPos.position - transform.position).normalized;

            if (direction.x < 0)
                movement.intent.left = true;
            if (direction.x > 0)
                movement.intent.right = true;
            if (direction.y > 0)
                movement.intent.jump = true;
        }




        


    }
}
