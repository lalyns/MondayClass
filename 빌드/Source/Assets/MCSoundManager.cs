﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.SceneDirector;
using AK.Wwise;

namespace MC.Sound
{
    [System.Serializable]
    public class SoundActive
    {
        public bool all = false;
        public bool sfx = false;
        public bool voice = false;
        public bool bgm = false;
        public bool ambient = false;
    }

    public class MCSoundManager : MonoBehaviour
    {
        public static MCSoundManager Instance;

        public static float SoundCall = 0;
        public const float SoundSkill3Break = 0.4f;

        public ObjectSound objectSound;
        [SerializeField] private AkBank sound;
        [SerializeField] private AkBank bgm;
        [SerializeField] private AkBank ambient;
        [SerializeField] private AkBank voice;

        public AK.Wwise.Event preBGM;
        public AK.Wwise.Event curBGM;
        public AK.Wwise.Event preAMB;
        public AK.Wwise.Event curAMB;

        private void Awake()
        {
            if (Instance == null)
                Instance = GetComponent<MCSoundManager>();
            else
                return;
        }

        private void Update()
        {
            SoundCall += Time.deltaTime;
        }

        public static void LoadBank()
        {
            Instance.sound.HandleEvent(Instance.gameObject);
            Instance.bgm.HandleEvent(Instance.gameObject);
            Instance.ambient.HandleEvent(Instance.gameObject);
            Instance.voice.HandleEvent(Instance.gameObject);
        }

        public static void ChangeBGM(AK.Wwise.Event bgm)
        {
            if(bgm != Instance.preBGM)
            {
                Instance.objectSound.bgm.StopBGM(Instance.gameObject, Instance.preBGM);
                Instance.preBGM = bgm;
                Instance.curBGM = bgm;
                Instance.objectSound.bgm.PlayBGM(Instance.gameObject, bgm);
            }
        }

        public static void StopAMB()
        {
            Instance.objectSound.ambient.StopAmbient(Instance.gameObject, Instance.curAMB);
        }

        public static void ChangeAMB(AK.Wwise.Event amb)
        {
            if (amb != Instance.preAMB)
            {
                Instance.objectSound.ambient.StopAmbient(Instance.gameObject, Instance.preAMB);
                Instance.preAMB = amb;
                Instance.curAMB = amb;
                Instance.objectSound.ambient.PlayAmbient(Instance.gameObject, amb);
            }
        }

        public static void SetRTPCParam(string type, float value)
        {
            AkSoundEngine.SetRTPCValue(type, value);
        }

        public static float GetRTPCParam(string type)
        {
            int rtpc = 1;
            float value = 0;
            AkSoundEngine.GetRTPCValue(type, Instance.gameObject, 0, out value, ref rtpc);

            return value;
        }

        public static IEnumerator AmbFadeOut(float duration, float target = 0)
        {
            float startTime = Time.realtimeSinceStartup;
            float realTime = startTime;

            while (realTime <= startTime + duration)
            {
                realTime = Time.realtimeSinceStartup;

                var value = Mathf.Clamp(Mathf.Clamp01(1 - (realTime - startTime) / duration) * 100 + target, target, 100f);
                value = value > GetRTPCParam("Ambient_Volume") ?
                    GetRTPCParam("Ambient_Volume") : value;

                SetRTPCParam("Ambient_Volume", value);

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        public static IEnumerator AmbFadeIn(float duration, float start = 0)
        {
            float startTime = Time.realtimeSinceStartup;
            float realTime = startTime;

            while (realTime <= startTime + duration)
            {
                realTime = Time.realtimeSinceStartup;

                var value = Mathf.Clamp( Mathf.Clamp01((realTime - startTime) / duration) * 100 + start, start, 100f);
                SetRTPCParam("Ambient_Volume", value);

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        public static IEnumerator BGMFadeOut(float duration, float target = 0)
        {
            float startTime = Time.realtimeSinceStartup;
            float realTime = startTime;

            while (realTime <= startTime + duration)
            {
                realTime = Time.realtimeSinceStartup;

                var value = Mathf.Clamp(Mathf.Clamp01(1 - (realTime - startTime) / duration) * 100 + target, target, 100f);
                value = value > GetRTPCParam("Bgm_SceneSwitch_Fade_Out") ?
                    GetRTPCParam("Bgm_SceneSwitch_Fade_Out") : value;

                SetRTPCParam("Bgm_SceneSwitch_Fade_Out", value);

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        public static IEnumerator BGMFadeIn(float duration, float start = 0)
        {
            float startTime = Time.realtimeSinceStartup;
            float realTime = startTime;

            while (realTime <= startTime + duration)
            {
                realTime = Time.realtimeSinceStartup;

                var value = Mathf.Clamp(Mathf.Clamp01((realTime - startTime) / duration) * 100 + start, start, 100f);
                SetRTPCParam("Bgm_SceneSwitch_Fade_Out", value);

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
    }
}