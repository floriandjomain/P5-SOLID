using UnityEngine;

namespace Level
{
    public static class MovementManager
    {
        public static Vector2Int GetVector(Movement m)
        {
            switch (m)
            {
                case Movement.Up:
                    return Vector2Int.up;
                case Movement.Right:
                    return Vector2Int.right;
                case Movement.Down:
                    return Vector2Int.down;
                case Movement.Left:
                    return Vector2Int.left;
                default:
                    return Vector2Int.zero;
            }
        }
    }
    public enum Movement 
    {
        None,
        Up,
        Right,
        Down,
        Left
    }
}
