using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public int MaxPlayerNumber;
        public int FieldSizeX;
        public int FieldSizeY;
        public int TileMaxLifePoints;

        [Tooltip("Temps en ms")] public float CommandInputTime;
    
        public AudioClip JumpSound;
        public AudioClip BumpSound;
        public AudioClip FallSound;
        public AudioClip EndGameWithAWinnerSound;
        public AudioClip EndGameWithNoWinnerSound;
    }
}
