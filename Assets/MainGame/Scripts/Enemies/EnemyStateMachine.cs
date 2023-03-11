using System.Collections.Generic;
using System.Linq;

public class EnemyStateMachine : IStateSwitcher
{
    public EnemyBase Enemy;

    private readonly List<IState> _states;
    private IState _currentState;

    public EnemyStateMachine(EnemyBase enemy)
    {
        Enemy = enemy;
        _states = new List<IState>
        {
            new EnemyIdleState(this),
            new EnemyAssaultState(this),
            new EnemyAttackState(this),
            new EnemyComebackState(this),
            new EnemyDeadState(this),
        };
    }

    public void StateSwitch<TState>() where TState : IState
    {
        _currentState?.Exit();
        _currentState = _states.FirstOrDefault(state => state is TState);
        _currentState.Enter();
    }
}
