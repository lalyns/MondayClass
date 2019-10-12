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
            Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
                Instance.objectSound.bgm.stageBGM);
        }

    }
}