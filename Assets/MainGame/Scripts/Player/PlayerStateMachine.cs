using System.Collections.Generic;
using System.Linq;

public class PlayerStateMachine : IStateSwitcher
{
    public PlayerController Player;

    private readonly List<IState> _states;
    private IState _currentState;

    public PlayerStateMachine(PlayerController player)
    {
        Player = player;
        _states = new List<IState>
        {
            new PlayerRespawnState(this),
            new PlayerWalkState(this),
            new PlayerAttackState(this),
            new PlayerDeadState(this),
        };
    }

    public void StateSwitch<TState>() where TState : IState
    {
        _currentState?.Exit();
        _currentState = _states.FirstOrDefault(state => state is TState);
        _currentState.Enter();
    }
}
