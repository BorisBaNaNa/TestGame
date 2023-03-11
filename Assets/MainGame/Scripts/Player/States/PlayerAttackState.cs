using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : IState
{
    private readonly PlayerStateMachine _stateMachine;
    private readonly PlayerController _player;
    private readonly List<EnemyBase> _enemies;
    private EnemyBase _currentEnemyTarget;
    private float _nextAttackTime;

    public PlayerAttackState(PlayerStateMachine playerStateMachine)
    {
        _stateMachine = playerStateMachine;
        _enemies = new List<EnemyBase>();
        _player = _stateMachine.Player;
    }

    public void Enter()
    {
        if (_player.AttackZone != null)
        {
            AttackZoneController attackZone = _player.AttackZone.GetComponent<AttackZoneController>();
            _enemies.AddRange(attackZone.GetAllEnemies());

            _player.CurrentAction = Fighting;
        }
        else Debug.LogError("Error! AttackZone is not deffined!");
    }

    public void Exit()
    {
        _player.CurrentAction = null;
    }

    private void Fighting()
    {
        if (_enemies.Count == 0)
        {
            _stateMachine.StateSwitch<PlayerWalkState>();
            return;
        }

        if (_currentEnemyTarget == null)
            SetNextTarget();

        _player.RotateOnTarget(_currentEnemyTarget.transform.position);

        if (_nextAttackTime <= Time.time/* && IsLookedToTarget*/)
            AttackEnemy();
    }

    private void AttackEnemy()
    {
        _nextAttackTime = _player.AttackCooldown + Time.time;

        BulletController bullet = AllServices.GetService<FactoryBullet>().BuildBullet(_player.FirePoint.position, _currentEnemyTarget.transform, _player.Damage);
        bullet.EnemyLayerMask = _player.EnemyLayerMask;
    }

    private void SetNextTarget()
    {
        float minDist = float.MaxValue;
        EnemyBase closestEnemy = null;
        foreach (EnemyBase enemy in _enemies)
        {
            float dist = Vector3.Distance(_player.transform.position, enemy.transform.position);    
            if (dist < minDist)
            {
                minDist = dist;
                closestEnemy = enemy;
            }
        }
        _currentEnemyTarget = closestEnemy;
        _currentEnemyTarget.Dead.AddListener(DeleteTarget); 
    }

    private void DeleteTarget()
    {
        _enemies.Remove(_currentEnemyTarget);
        _currentEnemyTarget = null;
    }
}
