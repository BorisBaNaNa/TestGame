using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    public Image HeathBar => _heathBar;
    public Transform HeathBarPoint_ => HeathBarPoint;

    [SerializeField]
    private Transform HeathBarPoint;

    private Image _heathBar;
    private Camera _camera;
    private EnemyBase _enemy;

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
        _enemy = GetComponent<EnemyBase>();
    }

    private void UpdateValUIElements()
    {
        TextMeshProUGUI textGUI = _heathBar.GetComponentInChildren<TextMeshProUGUI>();

        textGUI.text = Mathf.FloorToInt(_enemy.CurrentHeath).ToString();
        _heathBar.fillAmount = _enemy.CurrentHeath / _enemy.MaxHeath;
    }

    private void UpdatePosUIElements()
    {
        Vector3 screenPos = _camera.WorldToScreenPoint(HeathBarPoint.transform.position);

        _heathBar.transform.position = screenPos;
    }
}
