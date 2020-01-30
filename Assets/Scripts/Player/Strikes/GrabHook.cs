using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class GrabHook : BaseBoomerang
{
    private GameObject hookedObject;
    private LineRenderer lineRenderer;


    

    public void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }
    

    protected override void CollisionReaction(Collider2D collision, GameObject collisionRoot, Rigidbody2D rigidbody)
    {
        hookedObject = collisionRoot;
        hookedObject.transform.SetParent(transform);

        base.CollisionReaction(collision, collisionRoot, rigidbody);
    }

    protected override void Returned()
    {
        hookedObject?.transform.SetParent(null);
        base.Returned();
    }
    
    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, new Vector2(transform.position.x, transform.position.y));
        lineRenderer.SetPosition(1, new Vector2(attackContainer.transform.position.x, attackContainer.transform.position.y));
    }
}

