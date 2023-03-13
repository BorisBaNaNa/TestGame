using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDamageble
{
    public float CurrentHeath { get => _currentHeath; set => _currentHeath = value; }

    public CharacterController CharacterController => _characterController;
    public bool IsLookedToTarget => _targetRotation == transform.rotation;
    public float CutCritProbability => Mathf.Floor(CritProbability * 100f) / 100f;
    public float MoveSpeed => _moveSpeed;
    public int GetLayerMask => ~(1 << gameObject.layer);


    [HideInInspector]
    public AttackZoneController AttackZone;
    [HideInInspector]
    public Action CurrentAction;
    [HideInInspector]
    public Vector3 SpawnPoint;
    [HideInInspector]
    public bool IsDead;

    public Transform FirePoint;
    public LayerMask EnemyLayerMask;
    public bool IsHealAfterAttack = true;
    [Min(1f)]
    public float MaxHeath = 100f;
    [Min(0f)]
    public float HeathRecoveryPerSec = 1f;
    [Min(0.1f)]
    public float AttackCooldown = 1f;
    [Min(0f)]
    public float Damage = 50f;
    [Range(0f, 1f)]
    public float CritProbability = 0f;
    [Min(1f)]
    public float CritMultiply = 1.5f;

    [SerializeField, Min(0f)]
    private float _moveSpeed = 10f;

    private CharacterController _characterController;
    private PlayerStateMachine _stateMachine;
    private Quaternion _targetRotation;
    private Vector3 _verticalVelocity;
    private float _currentHeath;

    public void Awake()
    {
        Initialize();
    }

    public void Update()
    {
        CurrentAction?.Invoke();

        ApplyGravity();

        if (!IsDead)
            MakeHeal(HeathRecoveryPerSec * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackZone")
        {
            AttackZone = other.GetComponent<AttackZoneController>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "AttackZone" && AttackZone.AttackZoneCollider == other)
        {
            AttackZone = null;
        }
    }

    private void Initialize()
    {
        _characterController = GetComponent<CharacterController>();
        _stateMachine = new PlayerStateMachine(this);
        _stateMachine.StateSwitch<PlayerRespawnState>();
        _currentHeath = MaxHeath;
    }

    public void Respawn()
    {
        _stateMachine.StateSwitch<PlayerRespawnState>();
    }

    public void Spawn()
    {
        _currentHeath = MaxHeath;

        if (SpawnPoint != Vector3.zero)
        {
            _characterController.enabled = false;
            transform.position = SpawnPoint;
            _characterController.enabled = true;
        }

        AttackZone?.Reset();
    }

    public void TakeDamage(float damage)
    {
        _currentHeath -= damage;
        if (_currentHeath <= 0)
            _stateMachine.StateSwitch<PlayerDeadState>();
    }

    public void RotateOnTarget(Vector3 target)
    {
        target.y = transform.position.y;
        _targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 0.3f);
    }

    public void MakeHeal(float val)
    {
        _currentHeath = Mathf.Min(_currentHeath + val, MaxHeath);
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
