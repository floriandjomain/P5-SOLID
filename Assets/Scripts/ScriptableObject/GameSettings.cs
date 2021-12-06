using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int MaxPlayerNumber;
    public int TileMaxLifePoints;

    [Tooltip("Temps en ms")] public int CommandInputTime;
    
    public AudioClip JumpSound;
    public AudioClip BumpSound;
    public AudioClip FallSound;
    public AudioClip EndGameWithAWinnerSound;
    public AudioClip EndGameWithNoWinnerSound;
}
