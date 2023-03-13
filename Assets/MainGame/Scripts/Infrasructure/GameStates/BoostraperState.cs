using UnityEngine;

public class BoostraperState : IState
{
    private IStateSwitcher _gameStateMachine;
    private StartGameStates _startState;

    public BoostraperState(IStateSwitcher gameStateMachine, StartGameStates startState)
    {
        _gameStateMachine = gameStateMachine;
        _startState = startState;
    }

    public void Enter()
    {
        switch (_startState)
        {
            case StartGameStates.LoadLevelResourcesState:
                _gameStateMachine.StateSwitch<LoadLevelResoursesState>();
                break;
            default:
                Debug.LogError("Start state is not defined!");
                break;
        }
    }

    public void Exit()
    {

    }
}