using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    public event Action OnEventRaised;

    public void PlayClip(AudioSource audioSource, AudioClip clip, float volume = 1)
    {
        audioSource.clip = clip;
        audioSource.Play();
        
    }
}
