using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Vector3 ConvertPositionToWorld(Vector2 vec, float height)
    {
        return new Vector3(vec.x, 0f, vec.y) * 2.5f + Vector3.up * height;
    }

    public static Vector2Int ConvertPositionFromWorld(Vector3 vec)
    {
        vec /= 2.5f;
        return new Vector2Int((int)vec.x, (int)vec.z);
    }
}
