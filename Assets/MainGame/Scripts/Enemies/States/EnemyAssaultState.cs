using UnityEngine;

public class EnemyAssaultState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyBase _enemy;

    public EnemyAssaultState(EnemyStateMachine enemyStateMachine)
    {
        _stateMachine = enemyStateMachine;
        _enemy = _stateMachine.Enemy;
    }

    public void Enter()
    {
        _enemy.CurrentAction = MoveToAttackDistance;
    }

    public void Exit()
    {
        _enemy.CurrentAction = null;
    }

    public virtual void MoveToAttackDistance()
    {
        Vector3 moveDir = _enemy.MoveTo(_enemy.Player.position);

        if (Vector3.Magnitude(moveDir) <= _enemy.AttackDistance)
            _stateMachine.StateSwitch<EnemyAttackState>();
    }

}