using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    public static void PlaySound(string audioString)
    {
        GameObject audiocube = Resources.Load<GameObject>("AudioCube");

        AudioSource audioSource = audiocube.GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("SFX/" + audioString);

        audioSource.pitch = 1;

        Instantiate(audioSource);
    }

    public static void PlaySound(AudioClip audioClip)
    {

        if (audioClip == null) return;
        GameObject audiocube = Resources.Load<GameObject>("AudioCube");

        AudioSource audioSource = audiocube.GetComponent<AudioSource>();
        audioSource.clip = audioClip;

        audioSource.pitch = 1;

        Instantiate(audioSource);
    }


    public static void PlaySound(AudioClip audioClip, float pitch)
    {

        if (audioClip == null) return;
        GameObject audiocube = Resources.Load<GameObject>("AudioCube");

        AudioSource audioSource = audiocube.GetComponent<AudioSource>();
        audioSource.clip = audioClip;

        audioSource.pitch = pitch;

        Instantiate(audioSource);
    }

    public static void PlaySound(string audioString, float pitch)
    {
        GameObject audiocube = Resources.Load<GameObject>("AudioCube");

        AudioSource audioSource = audiocube.GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("SFX/" + audioString);
        audioSource.pitch = pitch;
        Instantiate(audioSource);
    }



}
