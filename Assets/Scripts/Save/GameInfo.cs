using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameInfo
{
    public string modificationDate;
    public Player player;
    public List<Doors> doors = new List<Doors>();
}

[Serializable]
public struct Level
{
    public string levelName;
}

[Serializable]
public struct Doors
{
    public int index;
    public bool isOpenned;
}

[Serializable]
public struct Player
{
    public Vector2 pos;
}
