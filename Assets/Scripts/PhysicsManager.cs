using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //HitballOnly layer is told to ignore all other layers
        Physics2D.IgnoreLayerCollision(11, 0);
        Physics2D.IgnoreLayerCollision(11, 9);
        Physics2D.IgnoreLayerCollision(11, 11);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
