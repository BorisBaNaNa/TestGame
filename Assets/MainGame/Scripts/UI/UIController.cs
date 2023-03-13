using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour, IService
{
    [SerializeField]
    private Animator UpPanelAnimator;
    [SerializeField]
    private TextMeshProUGUI GoldText;
    [SerializeField]
    private TextMeshProUGUI HeathUpBtnText;
    [SerializeField]
    private TextMeshProUGUI DamageUpBtnText;
    [SerializeField]
    private TextMeshProUGUI CooldownUpBtnText;
    [SerializeField]
    private TextMeshProUGUI HeathRecUpBtnText;
    [SerializeField]
    private TextMeshProUGUI CritProbUpBtnText;
    [SerializeField]
    private TextMeshProUGUI CritMultUpBtnText;
    [SerializeField]
    private GameObject DeadPanel;

    private string _heathUpBaseText;
    private string _damageUpBaseText;
    private string _cooldownUpBaseText;
    private string _heathRecUpBaseText;
    private string _critProbUpBaseText;
    private string _critMultUpBaseText;

    private LevelManager _levelManager;
    private PlayerUpgraderManager _playerUpManager;
    private bool _upPanelIsOpened = false;
    private int _openUpPanelAnimHash;
    private int _closeUpPanelAnimHash;

    public void Awake()
    {
        Initialize();
    }

    public void Start()
    {
        _levelManager = AllServices.GetService<LevelManager>();
        _playerUpManager = AllServices.GetService<PlayerUpgraderManager>();
    }

    public void Update()
    {
        UpgateTexts();
    }

    private void Initialize()
    {
        AllServices.RegisterService(this);

        _openUpPanelAnimHash = Animator.StringToHash("OpenController");
        _closeUpPanelAnimHash = Animator.StringToHash("CloseController");

        _heathUpBaseText = HeathUpBtnText.text;
        _damageUpBaseText = DamageUpBtnText.text;
        _cooldownUpBaseText = CooldownUpBtnText.text;
        _heathRecUpBaseText = HeathRecUpBtnText.text;
        _critProbUpBaseText = CritProbUpBtnText.text;
        _critMultUpBaseText = CritMultUpBtnText.text;
    }

    public void OpenCloseUpPanel()
    {
        if (_upPanelIsOpened)
        {
            _upPanelIsOpened = false;
            UpPanelAnimator.Play(_closeUpPanelAnimHash);
        }
        else
        {
            _upPanelIsOpened = true;
            UpPanelAnimator.Play(_openUpPanelAnimHash);
        }
    }

    public void SetGamePause(GameObject pausePannel)
    {
        if (pausePannel.activeSelf)
        {
            pausePannel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            pausePannel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void EnableDisableGameObj(GameObject gameObj)
    {
        if (gameObj.activeSelf)
            gameObj.SetActive(false);
        else
            gameObj.SetActive(true);
    }

    public void SetActiveDeadPanel(bool enabled)
    {
        DeadPanel.SetActive(enabled);
    }

    private void UpgateTexts()
    {
        GoldText.text = _levelManager.CurrentGoldCount.ToString();

        HeathUpBtnText.text = _heathUpBaseText + _playerUpManager.UpHeathInfo;
        DamageUpBtnText.text = _damageUpBaseText + _playerUpManager.UpDamageInfo;
        CooldownUpBtnText.text = _cooldownUpBaseText + _playerUpManager.UpCooldownInfo;
        HeathRecUpBtnText.text = _heathRecUpBaseText + _playerUpManager.UpHeathRecInfo;
        CritProbUpBtnText.text = _critProbUpBaseText + _playerUpManager.UpCritProbInfo;
        CritMultUpBtnText.text = _critMultUpBaseText + _playerUpManager.UpCrinMultInfo;
    }
}
