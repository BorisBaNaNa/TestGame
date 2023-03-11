using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public LayerMask EnemyLayerMask { get; set; }
    public float Damage { get; set; }
    public Transform Target { get; set; }

    [SerializeField]
    private float _flySpeed = 20;
    [SerializeField]
    private LayerMask _destroyLayerMask;

    private Vector3 _currentLinePos;
    private Vector3 _targetPos;
    private Vector3 _startPos;
    private float _flyHight = 0;
    private bool _targetDestroyed = false;

    private void Start()
    {
        _startPos = _currentLinePos = transform.position;
        _flyHight = Vector3.Distance(Target.position, _startPos) * 0.3f;
    }

    private void Update()
    {
        FlyToTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            var enemy = other.transform.GetComponent<IDamageble>();
            enemy.TakeDamage(Damage);
            DestroyThis();
            return;
        }
    }

    private void FlyToTarget()
    {
        SetTargetPosition();

        _currentLinePos = Vector3.MoveTowards(_currentLinePos, _targetPos, _flySpeed * Time.deltaTime);

        float x = ClampedLelerp(Vector3.Distance(_currentLinePos, _targetPos), Vector3.Distance(_startPos, _targetPos), 0f, 0f, 1f);
        float y = 4 * x * (1 - x); // Раскрывается в формулу параболы: -4x^2 + 4x + 0 = 4x - 4x^2 = 4x * (1 - x)

        Vector3 yVecUp = Vector3.up * y * _flyHight;
        Vector3 nextBelletPos = _currentLinePos + yVecUp;

        if (_targetDestroyed && Vector3.Distance(nextBelletPos, _targetPos) <= 0.1f)
        {
            DestroyThis();
            return;
        }

        transform.SetPositionAndRotation(nextBelletPos, Quaternion.LookRotation(_currentLinePos - transform.position));
    }

    private void SetTargetPosition()
    {
        if (!Target.IsDestroyed())
            _targetPos = Target.position;
        else if (!_targetDestroyed)
        {
            _targetDestroyed = true;
            SetTargetOnGroundPos();
        }
    }

    private void SetTargetOnGroundPos()
    {
        if (Physics.Raycast(_targetPos, Vector3.down, out var hitInfo, 100f, _destroyLayerMask, QueryTriggerInteraction.Ignore))
            _targetPos = hitInfo.point;
        else
            DestroyThis();
    }

    private void DestroyThis(float time = 0f)
    {
        Destroy(gameObject, time);
    }

    /// <summary>
    /// lerp(a,b,t) = x = a + (b - a) * t =>
    /// (x - a) / (b - a) = t, где х находится между a и b, а t это процент значения от a до b
    /// </summary>
    /// <param name="value">значение между sourceStart и sourceEnd</param>
    /// <param name="sourceStart">начальное значение первого промежутка</param>
    /// <param name="sourceEnd">конечное значение первого промежутка</param>
    /// <returns></returns>
    private float UnLerp(float value, float sourceStart, float sourceEnd)
    {
        value = Mathf.Clamp(value, Mathf.Min(sourceStart, sourceEnd), Mathf.Max(sourceStart, sourceEnd));
        return (value - sourceStart) / (sourceEnd - sourceStart);
    }

    /// <summary>
    /// функция преобразует значение между sourceStart и sourceEnd в значение между destinationMin и destinationMax
    /// </summary>
    /// <param name="value">значение между sourceStart и sourceEnd</param>
    /// <param name="sourceStart">начальное значение первого промежутка</param>
    /// <param name="sourceEnd">конечное значение первого промежутка</param>
    /// <param name="destinationMin">начальное значение итогового промежутка</param>
    /// <param name="destinationMax">конечное значение итогового промежутка</param>
    /// <returns></returns>
    private float ClampedLelerp(float value, float sourceStart, float sourceEnd, float destinationMin, float destinationMax)
    {
        float t = UnLerp(value,  sourceStart, sourceEnd);
        return t * (destinationMax - destinationMin) + destinationMin;
    }
}
