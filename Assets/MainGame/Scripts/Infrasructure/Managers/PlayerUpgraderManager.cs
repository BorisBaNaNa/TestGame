using UnityEngine;

public class PlayerUpgraderManager : MonoBehaviour, IService
{
    public string UpHeathInfo => BuildInfoStr(Mathf.Floor(_player.MaxHeath),
        Mathf.Floor(_player.MaxHeath * PercentToMultiply(HeathUpgradePercent)),
        HeathUpgradeCost);
    public string UpDamageInfo => BuildInfoStr(Mathf.Floor(_player.Damage),
        Mathf.Floor(_player.Damage * PercentToMultiply(DamageUpgradePercent)),
        DamageUpgradeCost);
    public string UpCooldownInfo => BuildInfoStr(_player.AttackCooldown,
        _player.AttackCooldown - AttackCooldownUpgradeVal,
        AttackCooldownUpgradeCost);
    public string UpHeathRecInfo => BuildInfoStr(_player.HeathRecoveryPerSec,
        _player.HeathRecoveryPerSec + HeathRecoveryUpgradeVal,
        HeathRecoveryUpgradeCost);
    public string UpCritProbInfo => BuildInfoStr(_player.CutCritProbability * 100f,
        Mathf.Floor((_player.CritProbability + CritProbabilityUpgradeVal) * 100f),
        CritProbabilityUpgradeCost);
    public string UpCrinMultInfo => BuildInfoStr(Mathf.Floor(_player.Damage * _player.CritMultiply),
        Mathf.Floor(_player.Damage * (_player.CritMultiply + CritMultiplyUpgradeVal)),
        CritMultiplyUpgradeCost);

    [Header("Settings")]
    public float InceasCostPercent = 50f;

    [Header("Heath")]
    public float HeathUpgradePercent = 5f;
    public int HeathUpgradeCost = 10;

    [Header("Damage")]
    public float DamageUpgradePercent = 1f;
    public int DamageUpgradeCost = 10;

    [Header("AttackCooldown")]
    public float AttackCooldownUpgradeVal = 0.01f;
    public int AttackCooldownUpgradeCost = 10;

    [Header("HeathRecovery")]
    public float HeathRecoveryUpgradeVal = 0.1f;
    public int HeathRecoveryUpgradeCost = 10;

    [Header("CritProbability")]
    public float CritProbabilityUpgradeVal = 0.001f;
    public int CritProbabilityUpgradeCost = 10;

    [Header("CritMultiply")]
    public float CritMultiplyUpgradeVal = 0.1f;
    public int CritMultiplyUpgradeCost = 10;

    private PlayerController _player;
    private LevelManager _levelManager;

    private void Awake()
    {
        AllServices.RegisterService(this);
    }

    private void Start()
    {
        _levelManager = AllServices.GetService<LevelManager>();
        _player = _levelManager.Player;
    }

    public void HeathUpgrade()
    {
        if (_levelManager.ReduceGold(HeathUpgradeCost))
        {
            HeathUpgradeCost = Mathf.FloorToInt(HeathUpgradeCost * PercentToMultiply(InceasCostPercent));
            float mult = PercentToMultiply(HeathUpgradePercent);
            _player.MaxHeath *= mult;
            _player.CurrentHeath *= mult;
        }
    }

    public void DamageUpgrade()
    {
        if (_levelManager.ReduceGold(DamageUpgradeCost))
        {
            DamageUpgradeCost = Mathf.FloorToInt(DamageUpgradeCost * PercentToMultiply(InceasCostPercent));
            float mult = PercentToMultiply(DamageUpgradePercent);
            _player.Damage *= mult;
        }
    }

    public void AttackCooldownUpgrade()
    {
        if (_player.AttackCooldown < 0.1) return;

        if (_levelManager.ReduceGold(AttackCooldownUpgradeCost))
        {
            AttackCooldownUpgradeCost = Mathf.FloorToInt(AttackCooldownUpgradeCost * PercentToMultiply(InceasCostPercent));
            _player.AttackCooldown -= AttackCooldownUpgradeVal;
        }
    }

    public void HeathRecoveryUpgrade()
    {
        if (_levelManager.ReduceGold(HeathRecoveryUpgradeCost))
        {
            HeathRecoveryUpgradeCost = Mathf.FloorToInt(HeathRecoveryUpgradeCost * PercentToMultiply(InceasCostPercent));
            _player.HeathRecoveryPerSec += HeathRecoveryUpgradeVal;
        }
    }

    public void CritProbabilityUpgrade()
    {
        if (_levelManager.ReduceGold(CritProbabilityUpgradeCost))
        {
            CritProbabilityUpgradeCost = Mathf.FloorToInt(CritProbabilityUpgradeCost * PercentToMultiply(InceasCostPercent));
            _player.CritProbability += CritProbabilityUpgradeVal;
        }
    }

    public void CritMultiplyUpgrade()
    {
        if (_levelManager.ReduceGold(CritMultiplyUpgradeCost))
        {
            CritMultiplyUpgradeCost = Mathf.FloorToInt(CritMultiplyUpgradeCost * PercentToMultiply(InceasCostPercent));
            _player.CritMultiply += CritMultiplyUpgradeVal;
        }
    }

    private float PercentToMultiply(float percent) => 1 + percent / 100;

    private string BuildInfoStr(float valBefor, float valAfter, int cost) => $"\nСейчас: {valBefor}\nДалее: {valAfter}\nЦена: {cost}";
}
