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


    

    public GrabHook()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, attackContainer.transform.position);
    }
}

