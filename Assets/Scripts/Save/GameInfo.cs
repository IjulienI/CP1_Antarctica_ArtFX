using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameInfo
{
    public string modificationDate;
    public Player player;
    public Ai ai;
    public List<Doors> doors = new List<Doors>();
    public string levelName;
}

[Serializable]
public struct Doors
{
    public int index;
    public bool isOpenned;
}

[Serializable]

public struct Ai
{
    public Vector2 pos;
}

[Serializable]
public struct Player
{
    public Vector2 pos;
}
