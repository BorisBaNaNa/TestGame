using UnityEngine;

public class PlayerRespawnState : IState
{
    private readonly PlayerStateMachine _stateMachine;

    public PlayerRespawnState(PlayerStateMachine playerStateMachine)
    {
        _stateMachine = playerStateMachine;
    }

    public void Enter()
    {
        _stateMachine.Player.Spawn();

        _stateMachine.StateSwitch<PlayerWalkState>();
    }

    public void Exit() { }
}
