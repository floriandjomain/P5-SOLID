using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceSetter : MonoBehaviour
{
    [SerializeField] private AudioAsset _audioAsset;

    private AudioSource _audioSource;

    /// Script need to be in gameObject with AudioSource
    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioAsset)
        {
            _audioAsset.SetAudioSource(_audioSource);
            _audioAsset.Active();
        }
    }

    private void OnDisable()
    {
        _audioAsset.SetAudioSource(null);
    }

    public void SwitchAudioAsset(AudioAsset newAudioAsset)
    {
        _audioAsset = newAudioAsset;
        _audioAsset.SetAudioSource(_audioSource);
        _audioAsset.Active();
    }
}
