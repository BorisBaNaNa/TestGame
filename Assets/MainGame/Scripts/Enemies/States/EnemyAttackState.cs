public class EnemyAttackState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyBase _enemy;

    public EnemyAttackState(EnemyStateMachine enemyStateMachine)
    {
        _stateMachine = enemyStateMachine;
        _enemy = _stateMachine.Enemy;
    }

    public void Enter()
    {
        _enemy.CurrentAction = _enemy.Fighting;
    }

    public void Exit()
    {
        _enemy.CurrentAction = null;
    }
}

