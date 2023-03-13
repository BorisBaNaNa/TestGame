using System;
using UnityEngine;

public enum EnemyType
{
    BaseEnemy,
}

public enum StartGameStates
{
    LoadLevelResourcesState,
}

[Serializable]
public struct EnemyClassifier
{
    public EnemyType Type;
    public EnemyBase EnemyPrefub;
} 