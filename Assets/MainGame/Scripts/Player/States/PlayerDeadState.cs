using UnityEngine;
public class PlayerDeadState : IState
{
    private readonly PlayerStateMachine _stateMachine;

    public PlayerDeadState(PlayerStateMachine playerStateMachine)
    {
        _stateMachine = playerStateMachine;
    }

    public void Enter()
    {
        _stateMachine.Player.CurrentAction = null;
        _stateMachine.Player.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    public void Exit()
    {
        _stateMachine.Player.transform.rotation = Quaternion.identity;
    }
}
