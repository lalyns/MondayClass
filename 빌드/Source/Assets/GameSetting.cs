using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
            strLevel = 8;
            defLevel = 6;
            hpLevel = 7;

            skill1DMGLevel = 3;
            skill1BounceLevel = 3;
            skill2DMGLevel = 2;

            skill3DMGLevel = 3;
            skill3TurnLevel = 3;

            feverGauge = false;
        }
    }

    public static RewardAbillity rewardAbillity = new RewardAbillity();
    // finalStr = (BaseSTR + perStr * strLevel)

    // 게임오버되거나 씬이 로드될때 능력치 계산

    public void Awake()
    {
        MC.Sound.MCSoundManager.SetRTPCParam("All_Volume", all.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Bgm_Volume", bgm.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Sound_Volume", sfx.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Ambient_Volume", amb.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Voice_Volume", voice.value);
    }

    public void Start()
    {
        MC.Sound.MCSoundManager.SetRTPCParam("All_Volume", all.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Bgm_Volume", bgm.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Sound_Volume", sfx.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Ambient_Volume", amb.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Voice_Volume", voice.value);
        gameObject.SetActive(false);


    }

    public void OnEnable()
    {
        MC.Sound.MCSoundManager.SetRTPCParam("All_Volume", all.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Bgm_Volume", bgm.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Sound_Volume", sfx.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Ambient_Volume", amb.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Voice_Volume", voice.value);
    }

    public void Update()
    {
        MC.Sound.MCSoundManager.SetRTPCParam("All_Volume", all.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Bgm_Volume", bgm.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Sound_Volume", sfx.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Ambient_Volume", amb.value);
        MC.Sound.MCSoundManager.SetRTPCParam("Voice_Volume", voice.value);
    }

    public void SettingExit()
    {
        setting.SetActive(false);
    }

}
