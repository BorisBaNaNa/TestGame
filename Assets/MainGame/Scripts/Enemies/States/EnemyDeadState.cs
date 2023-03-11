using UnityEngine;

public class EnemyDeadState : IState
{
    private readonly EnemyStateMachine _stateMachine;

    public EnemyDeadState(EnemyStateMachine enemyStateMachine)
    {
        _stateMachine = enemyStateMachine;
    }

    public void Enter()
    {
        _stateMachine.Enemy.Dead?.Invoke();
        Object.Destroy(_stateMachine.Enemy.gameObject);
    }

    public void Exit()
    {

    }
}

