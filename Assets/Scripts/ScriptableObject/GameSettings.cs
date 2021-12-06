using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int MaxPlayerNumber;
    public int TileMaxLifePoints;

    [Tooltip("Temps en s")] public float CommandInputTime;
    [Tooltip("Temps en s")] public float PlayTime;
    
    public AudioClip JumpSound;
    public AudioClip BumpSound;
    public AudioClip FallSound;
    public AudioClip EndGameWithAWinnerSound;
    public AudioClip EndGameWithNoWinnerSound;
}
