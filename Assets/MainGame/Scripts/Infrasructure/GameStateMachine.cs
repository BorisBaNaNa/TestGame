using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStateMachine : IStateSwitcher
{
    private readonly List<IState> _states;
    private IState _currentState;

    public GameStateMachine(CinemachineVirtualCamera virtualCamera, StartGameStates startState)
    {
        _states = new List<IState>
        {
            new BoostraperState(this, startState),
            new LoadLevelResoursesState(this, virtualCamera),
        };
    }

    public void StateSwitch<TState>() where TState : IState
    {
        _currentState?.Exit();
        _currentState = _states.FirstOrDefault(state => state is TState);
        _currentState.Enter();
    }
}