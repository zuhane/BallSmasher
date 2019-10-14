using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMe : MonoBehaviour
{

    void Start()
    {
        float duration = GetComponent<AudioSource>().clip.length;
        Destroy(gameObject, duration);
    }

}
