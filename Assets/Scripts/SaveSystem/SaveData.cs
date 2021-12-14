using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public ArenaStruct Arena;
    public List<PlayerStruct> Players;
    public int State;
    public int Turn;
}

public struct ArenaStruct
{
    public string ArenaType;
    public TileStruct[,] Tiles;
    public Vector3 Scale;
}

public struct TileStruct
{
    public string Name;
    public int StartLifePoints;
    public int CurrentLifePoints;
    public int Timer;
    public Vector2Int Position;
}

public struct PlayerStruct
{
    public string Name;
    public bool IsAlive;
    public Vector2Int Position;
}