using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDamageble
{
    public CharacterController CharacterController => _characterController;
    public bool IsLookedToTarget => _targetRotation == transform.rotation;
    public float CurrentHeath => _currentHeath;
    public float MoveSpeed => _moveSpeed;
    public int GetLayerMask => ~(1 << gameObject.layer);

    [HideInInspector]
    public AttackZoneController AttackZone;
    [HideInInspector]
    public Action CurrentAction;
    [HideInInspector]
    public Vector3 SpawnPoint;

    public Transform FirePoint;
    public LayerMask EnemyLayerMask;
    public float MaxHeath = 100f;
    public float AttackCooldown = 1f;
    public float Damage = 50f;

    [SerializeField]
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
        _currentHeath = MaxHeath;

        if (SpawnPoint != Vector3.zero)
        {
            _characterController.enabled = false;
            transform.position = SpawnPoint;
            _characterController.enabled = true;
        }

        AttackZone?.Respawn();
    }

    public void TakeDamage(float damage)
    {
        _currentHeath -= damage;
        if (_currentHeath <= 0)
            _stateMachine.StateSwitch<PlayerDeadState>();
    }

    public void RotateOnTarget(Vector3 target)
    {
        _targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 0.3f);
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
