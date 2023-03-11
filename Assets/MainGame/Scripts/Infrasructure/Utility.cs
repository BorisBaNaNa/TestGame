using System;
using UnityEngine;

public enum EnemyType
{
    BaseEnemy,
}

[Serializable]
public struct EnemyClassifier
{
    public EnemyType Type;
    public EnemyBase EnemyPrefub;
} 