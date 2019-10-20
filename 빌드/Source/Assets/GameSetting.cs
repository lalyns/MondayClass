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

}
