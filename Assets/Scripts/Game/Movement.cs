using UnityEngine;

public static class MovementData
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
    
    public static string ToString(Movement m)
    {
        switch (m)
        {
            case Movement.Up:
                return " upwards";
            case Movement.Right:
                return " right";
            case Movement.Down:
                return " downwards";
            case Movement.Left:
                return " left";
            default:
                return ".... no, they won't move";
        }
    }
}
public enum Movement : int
{
    None = 0,
    Up,
    Right,
    Down,
    Left
}
