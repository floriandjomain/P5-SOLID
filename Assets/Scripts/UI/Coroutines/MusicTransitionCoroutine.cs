using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Coroutine/MusicTransition")]
public class MusicTransitionCoroutine : GameCoroutine
{
    [SerializeField] private float _speedTransition;

    private float _minSound = -20;

    [SerializeField] private AudioMixer _audioMixer;
    public override IEnumerator ExecuteCoroutine()
    {
        bool result = _audioMixer.GetFloat("BackgroundMusic-Volume", out float value);
        while (value > _minSound)
        {
            bool setter = _audioMixer.SetFloat("BackgroundMusic-Volume", value - _speedTransition);
            yield return null;
        }
    }
}
