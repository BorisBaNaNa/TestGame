using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [Serializable]
    public class WaveInfo
    {
        public int SpawnCount;
        public int EnemyCount;
        public EnemyType Type;
    }

    [SerializeField]
    private float MultiplyerHeath = 0.3f;
    [SerializeField]
    private float MultiplyerDamage= 0.5f;
    [SerializeField]
    private float MultiplyerCost = 0.5f;
    [SerializeField]
    private float MinDistanceForSpawn = 3f;
    [SerializeField]
    private List<WaveInfo> WaveInfos;

    private Vector3 _spawnAria;
    private Collider _collider;
    private GameObject _enemiesParent;
    private int _enemyCount;
    private static int _currentSpawnCounter;
    private static int _currentWave = -1;
    private static float _heathBonus;
    private static float _damageBonus;
    private static float _costBonus;
    private static EnemyType _currentEnemyType;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _collider = GetComponents<Collider>().FirstOrDefault(collider => collider.isTrigger);
        _spawnAria = _collider.bounds.extents - Vector3.one * 0.1f;

        _enemiesParent = new GameObject("Enemies");
        _enemiesParent.transform.parent = transform;
    }

    private void OnDestroy()
    {
        Destroy(_enemiesParent);
    }

    public void SpawnEnemies()
    {
        if (WaveInfos.Count == 0)
        {
            Debug.LogWarning("WaveInfos is epmty!!!");
            return;
        }

        if (_currentSpawnCounter <= 0)
            SwichWaveOnNext();

        EnemyBase lastBuildEnemy = BuildEnemies();
        UpgradeBonusStats(lastBuildEnemy);

        _currentSpawnCounter--;
    }

    private void UpgradeBonusStats(EnemyBase lastBuildEnemy)
    {
        if (lastBuildEnemy != null)
        {
            _heathBonus += lastBuildEnemy.MaxHeath * MultiplyerHeath;
            _damageBonus += lastBuildEnemy.Damage * MultiplyerDamage;
            _costBonus += lastBuildEnemy.Cost * MultiplyerCost;
        }
    }

    private EnemyBase BuildEnemies()
    {
        EnemyBase enemy = null;
        Vector3 spawnAriaCenter = _collider.bounds.center;
        _enemyCount = WaveInfos[_currentWave].EnemyCount;

        List<Vector3> allSpawnedPositions = new List<Vector3>();
        for (int currenEnmyCount = 0; currenEnmyCount < _enemyCount; currenEnmyCount++)
        {
            Vector3 enemyPos;
            do {
                enemyPos = GetRandomPos(spawnAriaCenter);
            } while (allSpawnedPositions.Exists(existPos => Vector3.Distance(existPos, enemyPos) < MinDistanceForSpawn));
            allSpawnedPositions.Add(enemyPos);

            enemy = AllServices.GetService<FactoryEnemy>().BuildEnemy<EnemyBase>(EnemyType.BaseEnemy, enemyPos, true);
            enemy.transform.parent = _enemiesParent.transform;
            enemy.MaxHeath += _heathBonus;
            enemy.Damage += _damageBonus;
            enemy.Cost += _costBonus;
        }

        return enemy;
    }

    private Vector3 GetRandomPos(Vector3 spawnAriaCenter)
    {
        return new Vector3(
            spawnAriaCenter.x + _spawnAria.x * UnityEngine.Random.Range(-0.5f, 1f),
            spawnAriaCenter.y - _spawnAria.y + 0.2f,
            spawnAriaCenter.z + _spawnAria.z * UnityEngine.Random.Range(-0.5f, 1f));
    }

    private void SwichWaveOnNext()
    {
        EnemyBase lastEnemy = null, currentEnemy;
        if (_currentWave >= 0)
            lastEnemy = AllServices.GetService<FactoryEnemy>().GetEnemyPrefub<EnemyBase>(WaveInfos[_currentWave].Type);

        if (++_currentWave >= WaveInfos.Count)
            _currentWave = 0;

        currentEnemy = AllServices.GetService<FactoryEnemy>().GetEnemyPrefub<EnemyBase>(WaveInfos[_currentWave].Type);

        _currentEnemyType = WaveInfos[_currentWave].Type;
        _currentSpawnCounter = WaveInfos[_currentWave].SpawnCount;

        LevelingStats(lastEnemy, currentEnemy);
    }

    private static void LevelingStats(EnemyBase lastEnemy, EnemyBase currentEnemy)
    {
        if (lastEnemy == null || currentEnemy)
            return;

        _heathBonus += Mathf.Max(lastEnemy.MaxHeath - currentEnemy.MaxHeath, -_heathBonus);
        _damageBonus += Mathf.Max(lastEnemy.Damage - currentEnemy.Damage, _damageBonus);
        _costBonus += Mathf.Max(lastEnemy.Cost - currentEnemy.Cost, _costBonus);
    }
}
