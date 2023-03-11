using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Boostraper : MonoBehaviour
{
    [HideInInspector]
    public GameStateMachine StateMachine;

    [SerializeField]
    private PlayerController PlayerPrefub;
    [SerializeField]
    private GroundController GroundPrefub;
    [SerializeField]
    private BulletController BulletPrefub;
    [SerializeField]
    private List<EnemyClassifier> Classifier;
    [SerializeField]
    private CinemachineVirtualCamera VirtualCamera;

    private void Awake()
    {
        Initialize();
        InitializeServices();
    }

    void Start()
    {
        StateMachine = new GameStateMachine(VirtualCamera);
        StateMachine.StateSwitch<BoostraperState>();
    }

    private void Initialize()
    {

    }

    private void InitializeServices()
    {
        AllServices.RegisterService(new FactoryPlayer(PlayerPrefub));
        AllServices.RegisterService(new FactoryGround(GroundPrefub));
        AllServices.RegisterService(new FactoryBullet(BulletPrefub));
        AllServices.RegisterService(new FactoryEnemy(Classifier));
    }
}
