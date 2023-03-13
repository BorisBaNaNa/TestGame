using Cinemachine;
using System;
using UnityEngine;

public class LoadLevelResoursesState : IState
{
    private IStateSwitcher _gameStateMachine;
    private Transform _spawnPoint;
    private CinemachineVirtualCamera _virtualCamera;
    private PlayerController _player;

    public LoadLevelResoursesState(IStateSwitcher gameStateMachine, CinemachineVirtualCamera virtualCamera)
    {
        _gameStateMachine = gameStateMachine;
        _virtualCamera = virtualCamera;
    }

    public void Enter()
    {
        _spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        SpawnPlayer();

        LevelManager levelManager = AllServices.GetService<LevelManager>();
        levelManager.Player = _player;
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

        _player = AllServices.GetService<FactoryPlayer>().BuildPlayer(_spawnPoint);
        _player.SpawnPoint = _spawnPoint.position;

        _virtualCamera.Follow = _player.transform;
        _virtualCamera.LookAt = _player.transform;
    }
}
