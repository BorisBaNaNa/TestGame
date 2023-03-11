using Cinemachine;
using System;
using UnityEngine;

public class LoadLevelResoursesState : IState
{
    private IStateSwitcher _gameStateMachine;
    private Transform _spawnPoint;
    private CinemachineVirtualCamera _virtualCamera;

    public LoadLevelResoursesState(IStateSwitcher gameStateMachine, CinemachineVirtualCamera virtualCamera)
    {
        _gameStateMachine = gameStateMachine;
        _virtualCamera = virtualCamera;

        _spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
    }

    public void Enter()
    {
        SpawnPlayer();
    }

    public void Exit()
    {

    }

    private void SpawnPlayer()
    {
        if (_spawnPoint == null)
        {
            Debug.LogError("Spawn point is not defined!");
            return;
        }

        PlayerController player = AllServices.GetService<FactoryPlayer>().BuildPlayer(_spawnPoint);
        player.SpawnPoint = _spawnPoint.position;

        _virtualCamera.Follow = player.transform;
        _virtualCamera.LookAt = player.transform;
    }
}
