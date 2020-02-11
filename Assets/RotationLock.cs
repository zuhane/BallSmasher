using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour
{

    void LateUpdate()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
