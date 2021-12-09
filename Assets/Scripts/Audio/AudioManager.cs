using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region === Singleton ===
    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] private AudioSourceSetter _musicAudioSourceSetter;
    [SerializeField] private AudioMixer _audioMixer;

    [Space(15)]
    [SerializeField] private AudioAsset _musicLobby;
    [SerializeField] private AudioAsset _musicLevel;

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
