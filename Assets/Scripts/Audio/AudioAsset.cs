using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio")]
public class AudioAsset : ScriptableObject
{
    [SerializeField] private AudioClip _audioClip;

    [Tooltip("Is it a soundeffect or a music ?")]
    [SerializeField] private bool _isBackground;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float _volume;

    private AudioSource _audioSource;

    /// Call by objects of other scene like player, button...
    public void PlayClip()
    {
        if (_audioSource != null)
        {
            if(_isBackground)
            {
                _audioSource.Play();
            }
            else
            {
                _audioSource.PlayOneShot(_audioClip, _volume);
            }
        }
    }

    ////////////
    /// Setting
    ////////////
    
    public void Active()
    {
        if (_audioSource != null)
        {
            if (_isBackground)
            {
                SettingMusic();
            }
            else
            {
                SettingSoundEffect();
            }
        }
    }

    public void SettingSoundEffect()
    {
        _audioSource.loop = false;
    }

    private void SettingMusic()
    {
        _audioSource.loop = true;
        _audioSource.clip = _audioClip;
        _audioSource.volume = _volume;
    }

    /*public void Play(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip, _volume);
    }*/

    /// Call by AudioSourceSetter
    public void SetAudioSource(AudioSource newAudioSource)
    {
        _audioSource = newAudioSource;
    }
}
