using UnityEngine;
using UnityEngine.UI;

public class FactoryHeathBar : IService
{
    private Image _heathBar;
    private GameObject _gui;

    public FactoryHeathBar(Image heathBar, GameObject gui)
    {
        _heathBar = heathBar;
        _gui = gui;
    }

    public Image BuildHeathBar()
    {
        Image hpBar = Object.Instantiate(_heathBar);
        hpBar.transform.SetParent(_gui.transform, true);
        return hpBar;
    }
}