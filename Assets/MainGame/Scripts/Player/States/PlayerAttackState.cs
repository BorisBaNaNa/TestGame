using System;
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
    private float _lastCritNormalProbability;
    private float _сritProbability;
    private float _critC;

    public PlayerAttackState(PlayerStateMachine playerStateMachine)
    {
        _stateMachine = playerStateMachine;
        _enemies = new List<EnemyBase>();
        _player = _stateMachine.Player;

        CalculateCritC();
        _сritProbability = _critC;
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

        if (_player.IsHealAfterAttack && _player.CurrentHeath > 0)
            _player.MakeHeal(_player.MaxHeath);

        if (_enemies.Count != 0)
            _enemies.Clear();

        _currentEnemyTarget = null;
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

        CalculateCritC();

        bool rollResult;
        BulletController bullet = AllServices.GetService<FactoryBullet>().BuildBullet(
            _player.FirePoint.position, 
            _currentEnemyTarget.transform, 
            RolCritDamage(out rollResult));

        bullet.EnemyLayerMask = _player.EnemyLayerMask;
        bullet.IsCrit = rollResult;
    }

    private void CalculateCritC()
    {
        if (_lastCritNormalProbability != _player.CutCritProbability)
        {
            _lastCritNormalProbability = _player.CutCritProbability;
            _critC = CfromP(_lastCritNormalProbability);
        }
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

    private float RolCritDamage(out bool rollResult)
    {
        if (UnityEngine.Random.value <= _сritProbability)
        {
            rollResult = true;
            _сritProbability = _critC;
            Debug.Log("Хуяк!");
            return _player.Damage * _player.CritMultiply;
        }
        rollResult = false;
        _сritProbability += _critC;
        return _player.Damage;
    }

    public float CfromP(float p)
    {
        if (p < 0) return 0;

        float Cupper = p;
        float Clower = 0f;
        float Cmid;
        float p1;
        float p2 = 1f;

        while (true)
        {
            Cmid = (Cupper + Clower) / 2f;
            p1 = PfromC(Cmid);
            if (Mathf.Approximately(Math.Abs(p1 - p2), 0f))
                break;
            if (p1 > p)
            {
                Cupper = Cmid;
            }
            else
            {
                Clower = Cmid;
            }
            p2 = p1;
        }
        return Cmid;
    }

    private float PfromC(float C)
    {
        float pProcOnN;
        float pProcByN = 0f;
        float sumNpProcOnN = 0f;
        int maxFails = (int)Math.Ceiling(1f / C);
        for (int N = 1; N <= maxFails; ++N)
        {
            pProcOnN = Math.Min(1f, N * C) * (1f - pProcByN);
            pProcByN += pProcOnN;
            sumNpProcOnN += N * pProcOnN;
        }
        return (1f / sumNpProcOnN);
    }
}
