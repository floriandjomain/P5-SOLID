using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get => _instance; }

    [SerializeField] private AudioSourceSetter _musicAudioSourceSetter;
    [SerializeField] private AudioMixer _audioMixer;

    [Space(15)]
    [SerializeField] private AudioAsset _musicLobby;
    [SerializeField] private AudioAsset _musicLevel;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    void Start()
    {
        PlayLobbyMusic();
    }

    public void PlayLobbyMusic()
    {
        _musicAudioSourceSetter.SwitchAudioAsset(_musicLobby);
        _musicLobby.PlayClip();
    }

    public void PlayLevelMusic()
    {
        _musicAudioSourceSetter.SwitchAudioAsset(_musicLevel);
        _musicLevel.PlayClip();
    }
}
