using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
    public static void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public static void PlaySoundWithVariation(AudioSource source, AudioClip clip)
    {
        float originalPitch = source.pitch;
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(clip);
        source.pitch = originalPitch;
    }

    public static void PlaySoundWithVariation(AudioClip clip)
    {
        float originalPitch = source.pitch;
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(clip);
        source.pitch = originalPitch;
    }
}
