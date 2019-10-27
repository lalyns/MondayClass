using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.SceneDirector;
using MC.UI;



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

        MC.UI.UserInterface.BlurSet(true, 10f);
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
        MC.UI.UserInterface.BlurSet(false, 10f);

        if (MCSceneManager.currentScene == MCSceneManager.TITLE)
        {
            var titleUI = FindObjectOfType<UITitle>().GetComponent<UITitle>();
            titleUI.title.start.interactable = true;
            titleUI.title.developer.interactable = true;
            titleUI.title.exit.interactable = true;

        }
    }

}
