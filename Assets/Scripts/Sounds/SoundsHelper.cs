using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsHelper : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayThisSound(AudioClip soundToPlay)
    {
        audioSource.PlayOneShot(soundToPlay);
    }
}
