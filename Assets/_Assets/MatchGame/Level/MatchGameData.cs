using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatchGameData", menuName = "ScriptableObjects/MatchGameData")]
public class MatchGameData : ScriptableObject
{
    public List<LevelData> levelDataList;
}

[Serializable]
public class LevelData
{
    public float timeLimit;
    public BackgroundType backgroundType;

    public List<MatchlingPlacementData> matchlingPlacementDataList;
    public List<MatchGoal> matchGoalList;
}

[Serializable]
public class MatchlingPlacementData
{
    public MatchlingType matchlingType;
    public List<MatchlingPlacement> MatchlingPlacementList;
}

[Serializable]
public class MatchlingPlacement
{
    public Vector2 position;
    public float size;
    public int order;
}

[Serializable]
public class MatchGoal
{
    public MatchlingType matchlingType;
    public int count;
}

public enum MatchlingType
{
    None = 0,
    Tree = 1,
    House = 2,
    Car = 3,
    Elephant = 4,
    Rock = 5,
}


public enum BackgroundType
{
    None = 0,
    Roads = 1,
    Beach = 2,
}