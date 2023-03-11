using System.Collections.Generic;
using UnityEngine;

public class FactoryEnemy : IService
{
    private Dictionary<EnemyType, EnemyBase> _enemyPrefubs;

    public FactoryEnemy(List<EnemyClassifier> classifiers)
    {
        _enemyPrefubs = new Dictionary<EnemyType, EnemyBase>();
        foreach (EnemyClassifier classifier in classifiers)
            _enemyPrefubs.Add(classifier.Type, classifier.EnemyPrefub);
    }

    public EnemyT BuildEnemy<EnemyT>(EnemyType enemyType, Vector3 at, bool randRotate = false) where EnemyT : EnemyBase
        => Object.Instantiate(_enemyPrefubs[enemyType], at, randRotate ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) : Quaternion.identity) as EnemyT;

    public EnemyT GetEnemyPrefub<EnemyT>(EnemyType enemyType) where EnemyT : EnemyBase => _enemyPrefubs[enemyType] as EnemyT;
}
