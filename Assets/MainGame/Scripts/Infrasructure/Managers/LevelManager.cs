using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GroundSpawner;

public class LevelManager : MonoBehaviour, IService
{
    public int CurrentGoldCount => _currentGoldCount;

    public int StartGoldCount;
    public List<WaveInfo> WaveInfos;
    public PlayerController Player;

    private int _currentGoldCount;

    private void Awake()
    {
        AllServices.RegisterService(this);

        SetupVals();
    }

    private void SetupVals()
    {
        _currentGoldCount = StartGoldCount;
    }

    public void IncreaseGold(int val)
    {
        _currentGoldCount += val;
    }

    public bool ReduceGold(int val)
    {
        if (val > _currentGoldCount)
            return false;

        _currentGoldCount -= val;
        return true;
    }

    public void ResetLevel()
    {
        Player.Respawn();
    }
}
