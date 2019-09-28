using System;
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

    #region Instance Caching
    private PlayerFSMManager playerFSMMgr;
    private MissionManager missionMgr;
    private GameStatus gameStatus;
    private GameManager gameMgr;
    #endregion

    private void Start()
    {
        playerFSMMgr = PlayerFSMManager.Instance;
        missionMgr = MissionManager.Instance;
        gameStatus = GameStatus.Instance;
        gameMgr = GameManager.Instance;

        SetMPMode(MPSimpleMode);
    }

    #region Canvas Control Function
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
    #endregion

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

        if (cursorMode) CursorLocation();
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
        public ParticleSystem[] SpecialEffects;
        public Image[] DashCount;
    }

    [Space(5)][Header("Player User Interface")]
    public PlayerInterface PCUI;

    private void PCIconImageSet(bool isSpecial)
    {
        PCUI.PCIcon.sprite = isSpecial ? PCUI.PCIconSprites[0] : PCUI.PCIconSprites[1];
    }

    private void PlayerSkillUISet(int i, float value)
    {
        var gaugeValue = Mathf.Clamp01(value / playerFSMMgr.Stat.skillCTime[i]);
        PCUI.SkillIcons[i].fillAmount = gaugeValue;
    }

    private void PCSpecialGaugeSet(float value)
    {
        var gaugeValue = Mathf.Clamp01(value * 0.01f);
        PCUI.Special.fillAmount = gaugeValue;

        OnSpecialEffect(value);
    }

    private void OnSpecialEffect(float value)
    {
        var effectActive = value >= 1.0;
        for (int i = 0; i < PCUI.SpecialEffects.Length; i++)
            PCUI.SpecialEffects[i].gameObject.SetActive(effectActive);
    }

    private void DashCountSet()
    {
        for (int i = 0; i < 3; i++) {
            if (playerFSMMgr.isDashCTime[i])
            {
                var fillValue = Mathf.Clamp01(1f - (playerFSMMgr.DashCTime[i] / 3f));
                PCUI.DashCount[i].fillAmount = fillValue;
            }
        }
    }

    private void PlayerUI()
    {
        // 나중에 변신에 포함시킬것
        PCIconImageSet(playerFSMMgr.isNormal);

        HPChangeEffect(playerFSMMgr.Stat, PCUI.PlayerHpBar);
        PCSpecialGaugeSet(playerFSMMgr.SpecialGauge);
        DashCountSet();

        if (playerFSMMgr.isSkill1CTime) PlayerSkillUISet(0, playerFSMMgr.Skill1CTime);
        if (playerFSMMgr.isSkill2CTime) PlayerSkillUISet(1, playerFSMMgr.Skill2CTime);
        if (playerFSMMgr.isSkill3CTime) PlayerSkillUISet(2, playerFSMMgr.Skill3CTime);
        if (playerFSMMgr.isSkill4CTime) PlayerSkillUISet(3, playerFSMMgr.Skill4CTime);
    }
    #endregion

    #region System User Interface
    // 커서가 보이는지 여부
    [Space(5)][Header("System User Interface")]
    private bool cursorMode = false;
    public Transform CursorTransform;
    private Image CursorImage => CursorTransform.GetComponent<Image>();

    public static void SetCursorMode(bool mode)
    {
        Instance.cursorMode = mode;

        Cursor.lockState = mode ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = false;
        Instance.CursorImage.enabled = mode;
    }

    private void CursorLocation()
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        CursorTransform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    // Screen Effect : FadeIn FadeOut
    public Image FadeInOutImage;
    public float FadeInOutSpeed = 10.0f;
    
    public static IEnumerator FadeIn(System.Action callback, float speed = 10.0f, float delay = 0.15f)
    {
        var alpha = Instance.FadeInOutImage.color;
        for (float i = 100; i >= 0;)
        {
            i -= speed;
            alpha.a = i * 0.01f;
            Instance.FadeInOutImage.color = alpha;

            yield return new WaitForSeconds(delay);
        }

        yield return Instance.StartCoroutine(Instance.FadeInOutReturnValue(callback));
    }

    public static IEnumerator FadeOut(System.Action callback, float speed = 10.0f, float delay = 0.15f)
    {
        var alpha = Instance.FadeInOutImage.color;
        for (float i = 0; i <= 100;)
        {
            i += speed;
            alpha.a = i * 0.01f;
            Instance.FadeInOutImage.color = alpha;

            yield return new WaitForSeconds(delay);
        }

        yield return Instance.StartCoroutine(Instance.FadeInOutReturnValue(callback));
    }

    IEnumerator FadeInOutReturnValue(System.Action callback)
    {
        yield return null;
        callback();
    }

    // Screen Effect : Blur

    // 게임 설정

    #endregion

    #region MissionProgress User Interface

    [Space(5)][Header("Mission Progress User Interface")]
    // 간략모드 활성화 여부
    private bool MPSimpleMode = false;
    public static void SetMPMode(bool value)
    {
        Instance.MPSimpleMode = value;
        Instance.FullMode.SetActive(!value);
        Instance.SimpleMode.SetActive(value);
        Instance.CurrentTimer = value ? Instance.simpleModeUIs.TimeText : Instance.fullModeUIs.TimeText;
        Instance.CurrentGoal = value ? Instance.simpleModeUIs.Goal : Instance.fullModeUIs.Goal;
        Instance.CurrentTimeBack = value ? Instance.simpleModeUIs.TimeBack : Instance.fullModeUIs.TimeBack;
    }

    [System.Serializable]
    public class FullModeMissionUIs
    {
        public Image MissionIcon;
        public Text MissionText;
        public Image TimeBack;
        public Text TimeText;
        public Image GoalIcon;
        public Text Goal;
    }

    public GameObject FullMode;
    public FullModeMissionUIs fullModeUIs;
    public static void FullModeSetMP()
    {
        Instance.fullModeUIs.MissionIcon.sprite =
            Instance.missionMgr.CurrentMission.Data.MissionIcon;
        Instance.fullModeUIs.MissionText.text =
            Instance.missionMgr.CurrentMission.Data.MissionText;
        Instance.fullModeUIs.GoalIcon.sprite =
            Instance.missionMgr.CurrentMission.Data.MissionIcon;
    }

    [System.Serializable]
    public class SimpleModeMissionUIs
    {
        public Image TimeBack;
        public Text TimeText;
        public Image GoalIcon;
        public Text Goal;
    }
    public GameObject SimpleMode;
    public SimpleModeMissionUIs simpleModeUIs;

    private Text CurrentTimer;
    private Image CurrentTimeBack;
    private void SetTimer(float value)
    {
        int min = (int)(value / 60f);
        int sec = (int)(value % 60f);

        var text = sec >= 10 ? min + "'" + sec + "''" : min + "'0" + sec + "''";
        CurrentTimer.text = text;
        var timeValue = Mathf.Clamp01(value / missionMgr.CurrentMission._LimitTime);
        Debug.Log(timeValue);
        CurrentTimeBack.fillAmount = timeValue;
    }

    private Text CurrentGoal;
    private void SetGoal(MissionType type)
    {
        var text = "";
        switch (type)
        {
            case MissionType.Annihilation:
                text = "남은 몬스터 " + gameStatus.ActivedMonsterList.Count + " 마리";
                break;
            case MissionType.Defence:
                MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
                text = "남은 기둥 체력 " + mission.protectedTarget.hp +" / " + mission._ProtectedTargetHP;
                break;
            case MissionType.Survival:
                text = gameMgr.curScore + " 개 / 5 개";
                break;
            case MissionType.Boss:
                text = "리리스를 처치하시오";
                break;
        }

        CurrentGoal.text = text;
    }

    private void SetSimpleGoal(MissionType type)
    {
        var text = "";
        switch (type)
        {
            case MissionType.Annihilation:
                text = gameStatus.ActivedMonsterList.Count + " ";
                break;
            case MissionType.Defence:
                MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
                text = mission.protectedTarget.hp + " / " + mission._ProtectedTargetHP;
                break;
            case MissionType.Survival:
                text = gameMgr.curScore + " / 5";
                break;
            case MissionType.Boss:
                text = "리리스를 처치하시오";
                break;
        }

        CurrentGoal.text = text;
    }

    private void ProgressUI()
    {
        SetTimer(gameStatus._LimitTime);
        if (!MPSimpleMode)
            SetGoal(MissionManager.Instance.CurrentMissionType);
        else
            SetSimpleGoal(MissionManager.Instance.CurrentMissionType);
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
