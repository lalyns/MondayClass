using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.SceneDirector;
using MC.UI;
using MC.Sound;


public class GameSetting : MonoBehaviour
{
    public Slider all;
    public Slider bgm;
    public Slider sfx;
    public Slider amb;
    public Slider voice;

    public GameObject setting;

    [System.Serializable]
    public class RewardAbillity
    {
        [Range(0, 8)] public int strLevel;
        [Range(0, 8)] public int defLevel;
        [Range(0, 8)] public int hpLevel;

        [Range(0, 8)] public int skill1DMGLevel;
        [Range(0, 8)] public int skill1BounceLevel;

        [Range(0, 8)] public int skill2DMGLevel;

        [Range(0, 8)] public int skill3DMGLevel;
        [Range(0, 8)] public int skill3TurnLevel;

        public bool feverGauge;

        public RewardAbillity()
        {
            strLevel = 0;
            defLevel = 0;
            hpLevel = 0;

            skill1DMGLevel = 0;
            skill1BounceLevel = 0;
            skill2DMGLevel = 0;

            skill3DMGLevel = 0;
            skill3TurnLevel = 0;

            feverGauge = false;
        }

        public RewardAbillity(int a, int b, int c, int d, int e, int f, int g, int h, bool i)
        {
            strLevel = a;
            defLevel = b;
            hpLevel = c;

            skill1DMGLevel = d;
            skill1BounceLevel = e;
            skill2DMGLevel = f;

            skill3DMGLevel = g;
            skill3TurnLevel = h;

            feverGauge = i;
        }
    }

    public static float allSoundValue = 100f;
    public static float bgmSoundValue = 100f;
    public static float ambSoundValue = 100f;
    public static float voiceSoundValue = 100f;
    public static float sfxSoundValue = 100f;

    public static RewardAbillity rewardAbillity = new RewardAbillity();


    public void OnEnable()
    {
        all.value = allSoundValue;
        bgm.value = bgmSoundValue;
        amb.value = ambSoundValue;
        sfx.value = voiceSoundValue;
        voice.value = sfxSoundValue;

        MC.Sound.MCSoundManager.SetRTPCParam("All_Volume", allSoundValue);
        MC.Sound.MCSoundManager.SetRTPCParam("Bgm_Volume", bgmSoundValue);
        MC.Sound.MCSoundManager.SetRTPCParam("Sound_Volume", ambSoundValue);
        MC.Sound.MCSoundManager.SetRTPCParam("Ambient_Volume", voiceSoundValue);
        MC.Sound.MCSoundManager.SetRTPCParam("Voice_Volume", sfxSoundValue);

        //MC.UI.UserInterface.BlurSet(true, 10f);
    }

    public void SetAllValue()
    {
        allSoundValue = all.value;
        MC.Sound.MCSoundManager.SetRTPCParam("All_Volume", allSoundValue);
    }

    public void SetBGMValue()
    {
        bgmSoundValue = bgm.value;
        MC.Sound.MCSoundManager.SetRTPCParam("Bgm_Volume", bgmSoundValue);
    }

    public void SetAMBValue()
    {
        ambSoundValue = amb.value;
        MC.Sound.MCSoundManager.SetRTPCParam("Ambient_Volume", voiceSoundValue);
    }

    public void SetSFXValue()
    {
        voiceSoundValue = sfx.value;
        MC.Sound.MCSoundManager.SetRTPCParam("Sound_Volume", ambSoundValue);
    }

    public void SetVoiceValue()
    {
        sfxSoundValue = voice.value;
        MC.Sound.MCSoundManager.SetRTPCParam("Voice_Volume", sfxSoundValue);
    }

    public void SettingExit()
    {
        setting.SetActive(false);
        MC.UI.UserInterface.BlurSet(false);

        var sound = MCSoundManager.Instance.objectSound.ui;
        sound.PlaySound(MCSoundManager.Instance.gameObject, sound.nextPage);

        if (MCSceneManager.currentScene == MCSceneManager.TITLE)
        {
            var titleUI = FindObjectOfType<UITitle>().GetComponent<UITitle>();
            titleUI.title.start.interactable = true;
            titleUI.title.developer.interactable = true;
            titleUI.title.exit.interactable = true;
            titleUI.title.setting.interactable = true;

        }
    }

}
