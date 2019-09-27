using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private static UserInterface instance;
    public static UserInterface Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<UserInterface>();
        }
    }

    private void Start()
    {
        CurrentTimer = fullModeUIs.Timer;
        CurrentGoal = fullModeUIs.Goal;
    }

    [SerializeField] private bool activeAllUI = true;
    [SerializeField] private bool activePlayerUI = true;
    [SerializeField] private bool activeMonsterUI = true;
    [SerializeField] private bool activeSystemUI = true;
    [SerializeField] private bool activeMissionProgressUI = true;
    [SerializeField] private bool activeMissionSelectionUI = true;

    public GameObject UICanvas;
    public static void SetAllUserInterface(bool isActive)
    {
        Instance.activeAllUI = isActive;
        Instance.UICanvas.SetActive(isActive);
    }

    public GameObject SystemUICanvas;
    public static void SetSystemInterface(bool isActive)
    {
        Instance.activeSystemUI = isActive;
        Instance.SystemUICanvas.SetActive(isActive);
    }

    public GameObject PlayerUICanvas;
    public static void SetPlayerUserInterface(bool isActive)
    {
        Instance.activePlayerUI = isActive;
        Instance.PlayerUICanvas.SetActive(isActive);
    }

    public GameObject MissionProgressUICanvas;
    public static void SetMissionProgressUserInterface(bool isActive)
    {
        Instance.activeMissionProgressUI = isActive;
        Instance.MissionProgressUICanvas.SetActive(isActive);
    }

    public GameObject MissionSelectionUICanvas;
    public static void SetMissionSelectionUI(bool isActive)
    {
        Instance.activeMissionSelectionUI = isActive;
        Instance.MissionSelectionUICanvas.SetActive(isActive);
    }

    private PlayerFSMManager playerFSMManager => PlayerFSMManager.Instance;
    private void Update()
    {
        if (!activeAllUI) { return; }

        if (activePlayerUI)
        {
            PlayerUI();
        }

        if (activeMissionProgressUI)
        {
            ProgressUI();
        }
    }

    #region Player User Interface
    [System.Serializable]
    public class PlayerInterface
    {
        public Image PCIcon;
        public Sprite[] PCIconSprites;
        public HPBar PlayerHpBar;
        public Image[] SkillIcons;
        public Image Special;
    }

    [Space(5)][Header("Player User Interface")]
    public PlayerInterface PCUI;

    private void PCIconImageSet(bool isSpecial)
    {
        PCUI.PCIcon.sprite = isSpecial ? PCUI.PCIconSprites[0] : PCUI.PCIconSprites[1];
    }

    private void PlayerSkillUISet(int i, float value)
    {
        var gaugeValue = Mathf.Clamp01(value / playerFSMManager.Stat.skillCTime[i]);
        PCUI.SkillIcons[i].fillAmount = gaugeValue;
    }

    private void PCSpecialGaugeSet(float value)
    {
        var gaugeValue = Mathf.Clamp01(value * 0.01f);
        PCUI.Special.fillAmount = gaugeValue;
    }

    private void PlayerUI()
    {
        // 나중에 변신에 포함시킬것
        PCIconImageSet(playerFSMManager.isNormal);

        HPChangeEffect(playerFSMManager.Stat, PCUI.PlayerHpBar);
        PCSpecialGaugeSet(playerFSMManager.SpecialGauge);

        if (playerFSMManager.isSkill1CTime) PlayerSkillUISet(0, playerFSMManager.Skill1CTime);
        if (playerFSMManager.isSkill2CTime) PlayerSkillUISet(1, playerFSMManager.Skill2CTime);
        if (playerFSMManager.isSkill3CTime) PlayerSkillUISet(2, playerFSMManager.Skill3CTime);
        if (playerFSMManager.isSkill4CTime) PlayerSkillUISet(3, playerFSMManager.Skill4CTime);
    }
    #endregion

    #region System User Interface

    #endregion

    #region MissionProgress User Interface

    [Space(5)][Header("Mission Progress User Interface")]
    // 간략모드 활성화 여부
    private bool MPSimpleMode = false;
    private static void SetMPMode(bool value)
    {
        Instance.MPSimpleMode = value;
        Instance.FullMode.SetActive(!value);
        Instance.SimpleMode.SetActive(value);
        Instance.CurrentTimer = value ? Instance.simpleModeUIs.Timer : Instance.fullModeUIs.Timer;
        instance.CurrentGoal = value ? Instance.simpleModeUIs.Goal : Instance.simpleModeUIs.Goal;
    }

    [System.Serializable]
    public class FullModeMissionUIs
    {
        public Image MissionIcon;
        public Text MissionText;

        public Text Timer;

        public Image GoalIcon;
        public Text Goal;
    }

    public GameObject FullMode;
    public FullModeMissionUIs fullModeUIs;
    public static void FullModeSetMP()
    {
        Instance.fullModeUIs.MissionIcon.sprite = MissionManager.Instance.CurrentMission.Data.MissionIcon;
        Instance.fullModeUIs.MissionText.text = MissionManager.Instance.CurrentMission.Data.MissionText;
        Instance.fullModeUIs.GoalIcon.sprite = MissionManager.Instance.CurrentMission.Data.MissionIcon;
    }

    [System.Serializable]
    public class SimpleModeMissionUIs
    {
        public Image TimeIcon;
        public Text Timer;
        public Image GoalIcon;
        public Text Goal;
    }
    public GameObject SimpleMode;
    public SimpleModeMissionUIs simpleModeUIs;

    private Text CurrentTimer;
    private void SetTimer(float value)
    {
        int min = (int)(value / 60f);
        int sec = (int)(value % 60f);

        var text = sec >= 10 ? min + "'" + sec + "''" : min + "'0" + sec + "''";
        CurrentTimer.text = text;
    }

    private Text CurrentGoal;
    private void SetGoal(MissionType type)
    {
        var text = "";
        switch (type)
        {
            case MissionType.Annihilation:
                text = "남은 몬스터 " + GameStatus.Instance.ActivedMonsterList.Count + " 마리";
                break;
            case MissionType.Defence:
                MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
                text = "남은 기둥 체력 " + mission.protectedTarget.hp +" / " + mission._ProtectedTargetHP;
                break;
            case MissionType.Survival:
                text = GameManager.Instance.curScore + " 개 / 5 개";
                break;
            case MissionType.Boss:
                text = "리리스를 처치하시오";
                break;
        }

        CurrentGoal.text = text;
    }

    private void ProgressUI()
    {
        SetTimer(GameStatus.Instance._LimitTime);
        SetGoal(MissionManager.Instance.CurrentMissionType);
    }

    #endregion

    #region User Interface Effect Support Functions

    /// <summary>
    /// HP Bar UI 변화에 효과를 주는 매소드
    /// </summary>
    /// <param name="stat"> 적용해야 하는 HP 정보 </param>
    /// <param name="hpBar"> 대상 HPBar </param>
    /// <param name="speed"> HP바가 줄어드는 속도 (기본:5) </param>
    /// <param name="barSize"> HP Bar의 가로길이 (기본:350) </param>
    private void HPChangeEffect(CharacterStat stat, HPBar hpBar, float speed = 5f, float barSize = 350f)
    {
        Transform currentTransform = hpBar.CurrentFillGround.transform;
        Transform laterTransform = hpBar.LaterFillGround.transform;
        Vector3 barLocation = new Vector3(-barSize + barSize * (stat.Hp / stat.MaxHp), 0, 0);

        currentTransform.localPosition = barLocation;

        if (hpBar.backHitMove)
        {
            laterTransform.localPosition = Vector3.Lerp(laterTransform.localPosition,
                barLocation, speed * Time.deltaTime);

            if (Vector3.Distance(currentTransform.localPosition, laterTransform.localPosition) <= 0.01f)
            {
                laterTransform.localPosition = currentTransform.localPosition;
                hpBar.backHitMove = false;
            }
        }
    }

    #endregion

}
