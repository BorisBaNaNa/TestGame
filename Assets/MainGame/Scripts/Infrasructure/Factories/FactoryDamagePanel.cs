using UnityEngine;

public class FactoryDamagePanel : IService
{
    private DamagePanel _damagePanelPrefub;
    private GameObject _gui;

    public FactoryDamagePanel(DamagePanel damagePanelPrefub, GameObject gui)
    {
        _damagePanelPrefub = damagePanelPrefub;
        _gui = gui;
    }

    public DamagePanel BuildDamagePanel(Vector3 at)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(at);

        DamagePanel damagePanel = Object.Instantiate(_damagePanelPrefub, screenPos, Quaternion.identity);
        damagePanel.transform.SetParent(_gui.transform, true);
        return damagePanel;
    }
}
