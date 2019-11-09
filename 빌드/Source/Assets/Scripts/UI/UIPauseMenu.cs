using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.UI;
using MC.Sound;

public class UIPauseMenu : MonoBehaviour
{
    public void OnEnable()
    {
        ParameterSetting();
    }

    public Slider playerSTR;
    public Text playerSTRText;
    public Slider playerDEF;
    public Text playerDEFText;
    public Slider playerHP;
    public Text playerHPText;

    public Slider playerSkill1DMG;
    public Text playerSkill1Text;
    public Slider playerSkill2DMG;
    public Text playerSkill2Text;
    public Slider playerSkill3DMG;
    public Text playerSkill3Text;

    public Image[] playerSkill1Bounce;
    public Image[] playerSkill3Turn;

    public Sprite[] gauge;

    public GameObject setting;

    void ParameterSetting()
    {
        playerSTR.value = GameSetting.rewardAbillity.strLevel;
        playerSTRText.text = "" + PlayerFSMManager.Instance.Stat.GetStr();
        playerDEF.value = GameSetting.rewardAbillity.defLevel;
        playerDEFText.text = "" + PlayerFSMManager.Instance.Stat.GetDfs();
        playerHP.value = GameSetting.rewardAbillity.hpLevel;
        playerHPText.text = "" + PlayerFSMManager.Instance.Stat.GetHP();

        playerSkill1DMG.value = GameSetting.rewardAbillity.skill1DMGLevel;
        playerSkill1Text.text = "" + PlayerFSMManager.Instance.Stat.GetSkill1Damage();
        playerSkill2DMG.value = GameSetting.rewardAbillity.skill2DMGLevel;
        playerSkill2Text.text = "" + PlayerFSMManager.Instance.Stat.GetSkill2Damage();
        playerSkill3DMG.value = GameSetting.rewardAbillity.skill3DMGLevel;
        playerSkill3Text.text = "" + PlayerFSMManager.Instance.Stat.GetSkill3Damage();

        for (int i=0; i< GameSetting.rewardAbillity.skill1BounceLevel; i++)
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

    public void PauseSetting()
    {
        setting.SetActive(true);

        UserInterface.BlurSet(true, 8f);

        var sound = MCSoundManager.Instance.objectSound.ui;
        sound.PlaySound(MCSoundManager.Instance.gameObject, sound.nextPage);
    }

    public void PauseSettingExit()
    {
        setting.SetActive(false);

        var sound = MCSoundManager.Instance.objectSound.ui;
        sound.PlaySound(MCSoundManager.Instance.gameObject, sound.nextPage);
    }

    public void PauseExit()
    {
        CanvasInfo.PauseMenuActive(false);
        GameManager.Instance.IsPuase = false;
        UserInterface.BlurSet(false, 10f);

        var sound = MCSoundManager.Instance.objectSound.ui;
        sound.PlaySound(MCSoundManager.Instance.gameObject, sound.nextPage);

        gameObject.SetActive(false);
    }

}
