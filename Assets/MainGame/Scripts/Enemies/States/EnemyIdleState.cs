public class EnemyIdleState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyBase _enemy;

    public EnemyIdleState(EnemyStateMachine enemyStateMachine)
    {
        _stateMachine = enemyStateMachine;
        _enemy = _stateMachine.Enemy;
    }

    public void Enter()
    {
        _enemy.CurrentAction = _enemy.WaitPlayer;
        // включить анимацию
    }

    public void Exit()
    {
        _enemy.CurrentAction = null;
        // выключить анимацию
    }
}

