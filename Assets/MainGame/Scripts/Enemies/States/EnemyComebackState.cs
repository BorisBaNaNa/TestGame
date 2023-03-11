using UnityEngine;

public class EnemyComebackState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyBase _enemy;

    public EnemyComebackState(EnemyStateMachine enemyStateMachine)
    {
        _stateMachine = enemyStateMachine;
        _enemy = _stateMachine.Enemy;
    }

    public void Enter()
    {
        _enemy.CurrentAction = MoveToComeback;
    }

    public void Exit()
    {
        _enemy.CurrentAction = null;
    }

    public virtual void MoveToComeback()
    {
        Vector3 moveDir = _enemy.MoveTo(_enemy.StartPos);

        if (Vector3.Magnitude(moveDir) <= 0.5f)
            _stateMachine.StateSwitch<EnemyIdleState>();
    }
}

