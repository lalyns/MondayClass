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
        MC.Sound.MCSoundManager.SetRTPCParam("AllVolume", all.value);
        MC.Sound.MCSoundManager.SetRTPCParam("BgmVolume", bgm.value);
        MC.Sound.MCSoundManager.SetRTPCParam("SoundVolume", sfx.value);
        MC.Sound.MCSoundManager.SetRTPCParam("AmbientVolume", amb.value);
        MC.Sound.MCSoundManager.SetRTPCParam("VoiceVolume", voice.value);
    }

    public void Start()
    {
        MC.Sound.MCSoundManager.SetRTPCParam("AllVolume", all.value);
        MC.Sound.MCSoundManager.SetRTPCParam("BgmVolume", bgm.value);
        MC.Sound.MCSoundManager.SetRTPCParam("SoundVolume", sfx.value);
        MC.Sound.MCSoundManager.SetRTPCParam("AmbientVolume", amb.value);
        MC.Sound.MCSoundManager.SetRTPCParam("VoiceVolume", voice.value);
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        MC.Sound.MCSoundManager.SetRTPCParam("AllVolume", all.value);
        MC.Sound.MCSoundManager.SetRTPCParam("BgmVolume", bgm.value);
        MC.Sound.MCSoundManager.SetRTPCParam("SoundVolume", sfx.value);
        MC.Sound.MCSoundManager.SetRTPCParam("AmbientVolume", amb.value);
        MC.Sound.MCSoundManager.SetRTPCParam("VoiceVolume", voice.value);
    }

    public void Update()
    {
        MC.Sound.MCSoundManager.SetRTPCParam("AllVolume", all.value);
        MC.Sound.MCSoundManager.SetRTPCParam("BgmVolume", bgm.value);
        MC.Sound.MCSoundManager.SetRTPCParam("SoundVolume", sfx.value);
        MC.Sound.MCSoundManager.SetRTPCParam("AmbientVolume", amb.value);
        MC.Sound.MCSoundManager.SetRTPCParam("VoiceVolume", voice.value);
    }

}
