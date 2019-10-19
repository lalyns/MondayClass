using System.Collections;
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

        public static int SoundCall = 0;

        public ObjectSound objectSound;
        [SerializeField] private AkBank sound;
        [SerializeField] private AkBank bgm;
        [SerializeField] private AkBank ambient;
        [SerializeField] private AkBank voice;

        private void Awake()
        {
            if (Instance == null)
                Instance = GetComponent<MCSoundManager>();
            else
                return;
        }
        
        public static void LoadBank()
        {
            Instance.sound.HandleEvent(Instance.gameObject);
            Instance.bgm.HandleEvent(Instance.gameObject);
            Instance.ambient.HandleEvent(Instance.gameObject);
            Instance.voice.HandleEvent(Instance.gameObject);
        }

        public static void SetSound()
        {
            Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
                Instance.objectSound.ambient.stageAmbient);
            Instance.objectSound.bgm.PlayBGM(Instance.gameObject,
                Instance.objectSound.bgm.stageBGM);
        }

        public static void SetRTPCParam(string type, float value)
        {
            AkSoundEngine.SetRTPCValue(type, value);
        }

        public static void GetRTPCParam(string type)
        {
            int rtpc = 1;
            float value = 0;
            AkSoundEngine.GetRTPCValue(type, Instance.gameObject, 0, out value, ref rtpc);
            Debug.Log("RTPC Name : " + type + " Value : " + value);
        }

        public static IEnumerator SoundFadeOut(float duration)
        {
            float startTime = Time.realtimeSinceStartup;
            float realTime = startTime;

            while (realTime <= startTime + duration)
            {
                realTime = Time.realtimeSinceStartup;

                var value = Mathf.Clamp01(1 - (realTime - startTime) / duration) * 100;
                SetRTPCParam("Fade", value);

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        public static IEnumerator SoundFadeIn(float duration)
        {
            float startTime = Time.realtimeSinceStartup;
            float realTime = startTime;

            while (realTime <= startTime + duration)
            {
                realTime = Time.realtimeSinceStartup;

                var value = Mathf.Clamp01((realTime - startTime) / duration) * 100;
                SetRTPCParam("Fade", value);

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
    }
}