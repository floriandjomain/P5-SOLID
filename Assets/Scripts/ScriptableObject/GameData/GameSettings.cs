using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameSettings")]
public class GameSettings : ScriptableObject
{
    public string StreamerControlType;

    public bool UseAutoLaunch;
    public int AutoLaunchTime;

    public bool KeepPlayersAfterFinish;

    public int MaxPlayerNumber;
    public int TileMaxLifePoints;

    [Tooltip("Temps en s")] public int CommandInputTime;
    [Tooltip("Temps en s")] public int PlayTime;
    
    public AudioClip JumpSound;
    public AudioClip BumpSound;
    public AudioClip FallSound;
    public AudioClip EndGameWithAWinnerSound;
    public AudioClip EndGameWithNoWinnerSound;
}
