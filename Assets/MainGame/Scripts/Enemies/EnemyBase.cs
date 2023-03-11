using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class EnemyBase : MonoBehaviour, IDamageble
{
    public Transform Player { get => _player; set => _player = value; }
    public Action CurrentAction { get => _currentAction; set => _currentAction = value; }
    public Vector3 StartPos => _startPos;
    public float CurrentHeath => _currentHeath;


    [HideInInspector]
    public UnityEvent Dead;

    public float MaxHeath = 100f;
    public float MoveSpeed = 10f;
    public float AttackDistance = 1f;
    public float AttackCooldown = 1f;
    public float Damage = 5f;
    public float Cost = 50f;

    private Transform _player;
    private Action _currentAction;
    private EnemyStateMachine _stateMachine;
    private CharacterController _characterController;
    private Vector3 _verticalVelocity;
    private Vector3 _startPos;
    private float _currentHeath;
    private float _nextAttackTime;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        SetupValls();
    }

    private void Update()
    {
        _currentAction?.Invoke();

        ApplyGravity();
    }

    private void Initialize()
    {
        _characterController = GetComponent<CharacterController>();
        _stateMachine = new EnemyStateMachine(this);
        _stateMachine.StateSwitch<EnemyIdleState>();
    }

    private void SetupValls()
    {
        _startPos = transform.position;
        _currentHeath = MaxHeath;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHeath -= damage;
        if (_currentHeath <= 0)
            _stateMachine.StateSwitch<EnemyDeadState>();
    }

    public virtual void WaitPlayer()
    {
        if (Player != null)
            _stateMachine.StateSwitch<EnemyAssaultState>();
    }

    public virtual Vector3 MoveTo(Vector3 target)
    {
        target.y = transform.position.y;
        Vector3 moveDir = target - transform.position;
        _characterController.Move(moveDir.normalized * MoveSpeed * Time.deltaTime);
        RotateOnTarget(moveDir);
        return moveDir;
    }

    private void RotateOnTarget(Vector3 dir)
    {
        Quaternion _targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 0.5f);
    }

    public virtual void Fighting()
    {
        if (Player.GetComponent<PlayerController>().CurrentHeath <= 0)
        {
            Player = null;
            _stateMachine.StateSwitch<EnemyComebackState>();
            return;
        }

        if (_nextAttackTime < Time.time)
            AttackPlayer();
    }

    private void AttackPlayer()
    {
        //Debug.Log("Attack Player!");
        IDamageble player = Player.GetComponent<IDamageble>();
        player.TakeDamage(Damage);
        _nextAttackTime = AttackCooldown + Time.time;
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity.y <= 0)
            _verticalVelocity = Vector3.zero;
        else
            _verticalVelocity += Physics.gravity * Time.deltaTime;

        _characterController.Move(_verticalVelocity * Time.deltaTime);
    }
}
