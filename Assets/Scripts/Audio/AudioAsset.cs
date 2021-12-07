using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio")]
public class AudioAsset : ScriptableObject
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _audioClip;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float _volume;

    /// Call by objects of other scene like player, button...
    public void PlayClip()
    {
        _audioSource.PlayOneShot(_audioClip, _volume);
    }

    public void Play()
    {
        _audioSource.Play();
    }

    /// Call by AudioSourceSetter
    public void SetAudioSource(AudioSource newAudioSource)
    {
        _audioSource = newAudioSource;
    }
}
