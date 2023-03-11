using UnityEngine;

public class FactoryGround : IService
{
    private GroundController _groundPrefub;

    public FactoryGround(GroundController groundPrefub)
    {
        _groundPrefub = groundPrefub;
    }

    public GroundController BuildGround(Vector3 at) => Object.Instantiate(_groundPrefub, at, Quaternion.identity);
}
