using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZoneController : MonoBehaviour
{
    public Collider AttackZoneCollider => _attackZoneCollider;

    public LayerMask EnemyLayerMask;

    private Collider _attackZoneCollider;

    private void Awake()
    {
        _attackZoneCollider = GetComponents<Collider>().FirstOrDefault(collider => collider.isTrigger);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            List<EnemyBase> enemies = GetAllEnemies();
            foreach (EnemyBase enemy in enemies)
            {
                enemy.Player = other.transform;
            }
        }
    }

    public List<EnemyBase> GetAllEnemies()
    {
        List<EnemyBase> enemies = new List<EnemyBase>();
        Collider[] colliders = Physics.OverlapBox(_attackZoneCollider.bounds.center, _attackZoneCollider.bounds.extents,
            _attackZoneCollider.transform.rotation, EnemyLayerMask);

        foreach (Collider collider in colliders)
            enemies.Add(collider.GetComponent<EnemyBase>());

        return enemies;
    }

    public void Reset()
    {
        GameObject attackPoint = new GameObject("AttackPoint");
        attackPoint.tag = "AttackPoint";

        GetAllEnemies().ForEach(enemy => Destroy(enemy.gameObject));
    }
}
