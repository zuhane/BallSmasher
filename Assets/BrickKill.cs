using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickKill : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        audioSource.pitch = Random.Range(.4f, 1);
        Destroy(gameObject, GetComponentInChildren<ParticleSystem>().main.duration);
    }

}
