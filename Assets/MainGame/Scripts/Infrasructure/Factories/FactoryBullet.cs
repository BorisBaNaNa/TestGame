using UnityEngine;

public class FactoryBullet : IService
{
    private BulletController _bulletPrefub;

    public FactoryBullet(BulletController bulletPrefub)
    {
        _bulletPrefub = bulletPrefub;
    }

    public BulletController BuildBullet(Vector3 from, Transform to, float damage)
    {
        Vector3 dir = to.position - from;
        BulletController bullet = Object.Instantiate(_bulletPrefub, from, Quaternion.LookRotation(dir));
        bullet.Target = to;
        bullet.Damage = damage;
        return bullet;
    }

    public BulletController BuildBullet(Vector3 at) => Object.Instantiate(_bulletPrefub, at, Quaternion.identity);
}
