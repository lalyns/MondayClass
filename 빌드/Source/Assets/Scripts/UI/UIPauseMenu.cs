using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    public void OnEnable()
    {
        ParameterSetting();
    }

    public Slider playerSTR;
    public Slider playerDEF;
    public Slider playerHP;

    public Slider playerSkill1DMG;
    public Slider playerSkill2DMG;
    public Slider playerSkill3DMG;

    public Image[] playerSkill1Bounce;
    public Image[] playerSkill3Turn;

    public Sprite[] gauge;

    void ParameterSetting()
    {
        playerSTR.value = GameSetting.rewardAbillity.strLevel;
        playerDEF.value = GameSetting.rewardAbillity.defLevel;
        playerHP.value = GameSetting.rewardAbillity.hpLevel;

        playerSkill1DMG.value = GameSetting.rewardAbillity.skill1DMGLevel;
        playerSkill2DMG.value = GameSetting.rewardAbillity.skill2DMGLevel;
        playerSkill3DMG.value = GameSetting.rewardAbillity.skill3DMGLevel;

        for(int i=0; i< GameSetting.rewardAbillity.skill1BounceLevel; i++)
        {
            playerSkill1Bounce[i].sprite = gauge[1];
        }
        for(int i= GameSetting.rewardAbillity.skill1BounceLevel; i < playerSkill1Bounce.Length; i++)
        {
            playerSkill1Bounce[i].sprite = gauge[0];
        }

        for (int i = 0; i < GameSetting.rewardAbillity.skill3TurnLevel; i++)
        {
            playerSkill3Turn[i].sprite = gauge[1];
        }
        for (int i = GameSetting.rewardAbillity.skill3TurnLevel; i < playerSkill3Turn.Length; i++)
        {
            playerSkill3Turn[i].sprite = gauge[0];
        }
    }



}
