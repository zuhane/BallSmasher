using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMe : MonoBehaviour
{
    public float duration;
    void Start()
    {
        if (duration == 0) duration = GetComponent<AudioSource>().clip.length;
        Destroy(gameObject, duration);
    }

}
