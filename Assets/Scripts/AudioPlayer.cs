using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/Audio Player")]
public class AudioPlayer : ScriptableObject
{
    [SerializeField]
    private AudioSource _audioSource;
}
