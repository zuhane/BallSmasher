using UnityEngine;

public static class AudioSourceExtension
{

    public static void PlayLooped(this AudioSource audio, AudioClip clip)
    {
        if (audio.enabled)
        {
            if (audio.isPlaying)
                audio.Stop();

            audio.clip = clip;
            audio.loop = true;
            audio.Play();
        }
    }

    public static void PlayOnce(this AudioSource audio, AudioClip clip)
    {
        if (audio.enabled)
        {
            if (audio.isPlaying)
                audio.Stop();

            audio.clip = clip;
            audio.loop = false;
            audio.Play();
        }
    }
}
