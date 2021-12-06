using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio")]
public class AudioManager : ScriptableObject
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _audioClip;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float _volume;

    public event Action OnEventRaise;

    //setaudioSource //
}
