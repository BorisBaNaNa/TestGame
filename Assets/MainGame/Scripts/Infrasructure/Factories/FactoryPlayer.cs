using UnityEngine;

public class FactoryPlayer : IService
{
    private PlayerController _playerPrefub;

    public FactoryPlayer(PlayerController playerPrefub)
    {
        _playerPrefub = playerPrefub;
    }

    public PlayerController BuildPlayer(Transform at) => Object.Instantiate(_playerPrefub, at.position, at.rotation);

    public PlayerController BuildPlayer(Vector3 at) => Object.Instantiate(_playerPrefub, at, Quaternion.identity);
}