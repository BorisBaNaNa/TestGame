using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public Image HeathBar => _heathBar;

    [SerializeField]
    private Transform HeathBarPoint;

    private Image _heathBar;
    private Camera _camera;
    private PlayerController _player;
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _heathBar = AllServices.GetService<FactoryHeathBar>().BuildHeathBar();
    }

    private void Update()
    {
        UpdatePosUIElements();
        UpdateValUIElements(); 
    }

    private void OnDestroy()
    {
        if (!_heathBar.IsDestroyed())
            Destroy(_heathBar.gameObject);
    }

    private void Init()
    {
        _camera = Camera.main;
        _player = GetComponent<PlayerController>();
    }

    private void UpdateValUIElements()
    {
        _heathBar.fillAmount = _player.CurrentHeath / _player.MaxHeath;
        TextMeshProUGUI textGUI = _heathBar.GetComponentInChildren<TextMeshProUGUI>();
        textGUI.text = Mathf.FloorToInt(_player.CurrentHeath).ToString();
    }

    private void UpdatePosUIElements()
    {
        Vector3 screenPos = _camera.WorldToScreenPoint(HeathBarPoint.transform.position);

        _heathBar.transform.position = screenPos;
    }
}
