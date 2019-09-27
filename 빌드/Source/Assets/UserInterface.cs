using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private static UserInterface instance;
    public static UserInterface Instance 
    {
        get => instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<UserInterface>();
        }
    }

    private bool activeAllUI;
    private bool activePlayerUI;
    private bool activeMonsterUI;
    private bool activeSystemUI;
    private bool activeMissionProgressUI;
    private bool activeMissionSelectionUI;

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

    private PlayerFSMManager playerFSMManager => PlayerFSMManager.instance;
    private void Update()
    {
        if (!activeAllUI) { return; }

        if (activePlayerUI)
        {
            PCIconImageSet(playerFSMManager.isNormal);
            PCSpecialGaugeSet(playerFSMManager.SpecialGauge);

            if (playerFSMManager.isSkill1CTime) SkillUISet(0, playerFSMManager.Skill1CTime);
            //if (playerFSMManager.isSkill2CTime) SkillUISet(1, playerFSMManager.Skill1CTime);
            //if (playerFSMManager.isSkill3CTime) SkillUISet(2, playerFSMManager.Skill1CTime);
            //if (playerFSMManager.isSkill4CTime) SkillUISet(3, playerFSMManager.Skill1CTime);
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

    public Image HpBar;

    public Image[] Skills;
    private void SkillUISet(int i, float value)
    {
        var gaugeValue = Mathf.Clamp01(value * 0.01f);
        Skills[i].fillAmount = gaugeValue;
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

}
