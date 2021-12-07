using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSetter : MonoBehaviour
{
    [SerializeField] private AudioAsset _audioAsset;

    private AudioSource _audioSource;

    /// Script need to be in gameObject with AudioSource
    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioAsset.SetAudioSource(_audioSource);
    }

    private void OnDisable()
    {
        _audioAsset.SetAudioSource(null);
    }
}
