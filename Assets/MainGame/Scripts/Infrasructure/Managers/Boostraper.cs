using Cinemachine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boostraper : MonoBehaviour
{
    [HideInInspector]
    public GameStateMachine StateMachine;

    [SerializeField]
    private bool IsMainBostraper;
    [SerializeField]
    private StartGameStates StartState;
    [SerializeField]
    private PlayerController PlayerPrefub;
    [SerializeField]
    private GroundController GroundPrefub;
    [SerializeField]
    private BulletController BulletPrefub;
    [SerializeField]
    private Image HeathBarPrefub;
    [SerializeField]
    private GameObject HeathBarParent;
    [SerializeField]
    private DamagePanel DamagePanelPrefub;
    [SerializeField]
    private GameObject DamagePanelParent;
    [SerializeField]
    private CinemachineVirtualCamera VirtualCamera;
    [SerializeField]
    private List<EnemyClassifier> Classifier;

    private void Awake()
    {
        if (!IsMainBostraper && FindObjectsOfType(typeof(Boostraper)).Length > 1)
        {
            Debug.Log("Level boostraper has destroyed");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Initialize();
        InitializeServices();
    }

    void Start()
    {
        StateMachine = new GameStateMachine(VirtualCamera, StartState);
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
        AllServices.RegisterService(new FactoryHeathBar(HeathBarPrefub, HeathBarParent));
        AllServices.RegisterService(new FactoryDamagePanel(DamagePanelPrefub, DamagePanelParent));
    }
}
