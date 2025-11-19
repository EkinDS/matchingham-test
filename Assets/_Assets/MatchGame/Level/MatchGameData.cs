using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatchGameData", menuName = "ScriptableObjects/MatchGameData")]
public class MatchGameData : ScriptableObject
{
    public List<LevelData> levelDataList;
    public List<MatchlingData> matchlingDataList;

    
    public Sprite GetSprite(MatchlingType matchlingType)
    {
        foreach (var matchlingData in matchlingDataList)
        {
            if (matchlingData.matchlingType == matchlingType)
            {
                return matchlingData.sprite;
            }
        }

        return null;
    }
}

[Serializable]
public class LevelData
{
    public float timeLimit;
    public Sprite backgroundSprite;
    public List<MatchlingPlacementData> matchlingPlacementDataList;
    public List<MatchGoal> matchGoalList;
}

[Serializable]
public class MatchlingData
{
    public MatchlingType matchlingType;
    public Sprite sprite;
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
}
