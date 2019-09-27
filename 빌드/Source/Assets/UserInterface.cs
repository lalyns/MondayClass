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

    public GameObject PlayerUIfaceCanvas;
    public static void SetPlayerUserInterface(bool isActive)
    {
        Instance.activePlayerUI = isActive;
        Instance.PlayerUIfaceCanvas.SetActive(isActive);
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
            PCIconImageSet(playerFSMManager.isNormal);
            PCSpecialGaugeSet(playerFSMManager.SpecialGauge);

            HPChangeEffect(playerFSMManager.Stat, PlayerHpBar);

            if (playerFSMManager.isSkill1CTime) PlayerSkillUISet(0, playerFSMManager.Skill1CTime);
            if (playerFSMManager.isSkill2CTime) PlayerSkillUISet(1, playerFSMManager.Skill2CTime);
            if (playerFSMManager.isSkill3CTime) PlayerSkillUISet(2, playerFSMManager.Skill3CTime);
            if (playerFSMManager.isSkill4CTime) PlayerSkillUISet(3, playerFSMManager.Skill4CTime);
        }

    }

    #region Player User Interface
    [Header("Player UI Set")]
    public Image PCIcon;
    public Sprite[] PCIconSprites;
    private void PCIconImageSet(bool isSpecial)
    {
        PCIcon.sprite = isSpecial ? PCIconSprites[0] : PCIconSprites[1];
    }

    public HPBar PlayerHpBar;

    public Image[] SkillIcons;
    private void PlayerSkillUISet(int i, float value)
    {
        var gaugeValue = Mathf.Clamp01(value / playerFSMManager.Stat.skillCTime[i]);
        SkillIcons[i].fillAmount = gaugeValue;
    }

    public Image Special;
    private void PCSpecialGaugeSet(float value)
    {
        var gaugeValue = Mathf.Clamp01(value * 0.01f);
        Special.fillAmount = gaugeValue;
    }
    #endregion

    #region System User Interface
    #endregion

    #region Monster User Interface
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
