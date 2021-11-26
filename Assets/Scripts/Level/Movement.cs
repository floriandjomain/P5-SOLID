using UnityEngine;

namespace Level
{
    public static class MovementManager
    {
        public static Vector2Int GetVector(Movement m)
        {
            switch (m)
            {
                case Movement.UP:
                    return Vector2Int.up;
                case Movement.RIGHT:
                    return Vector2Int.right;
                case Movement.DOWN:
                    return Vector2Int.down;
                case Movement.LEFT:
                    return Vector2Int.left;
                default:
                    return Vector2Int.zero;
            }
        }
    }
    public enum Movement 
    {
        NONE,
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
}
