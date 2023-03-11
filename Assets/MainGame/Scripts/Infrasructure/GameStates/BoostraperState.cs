public class BoostraperState : IState
{
    private IStateSwitcher _gameStateMachine;

    public BoostraperState(IStateSwitcher gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public void Enter()
    {
        _gameStateMachine.StateSwitch<LoadLevelResoursesState>();
    }

    public void Exit()
    {

    }
}