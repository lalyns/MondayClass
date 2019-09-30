using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Sound
{
    public class ObjectSound : MonoBehaviour
    {
        [System.Serializable]
        public class AmbientSoundList
        {
            #region AmbientList
            public AK.Wwise.Event lobbyAmbient = new AK.Wwise.Event();
            public AK.Wwise.Event stageAmbient = new AK.Wwise.Event();
            public AK.Wwise.Event bossAmbient = new AK.Wwise.Event();

            #endregion

            public void PlayAmbient(GameObject go, AK.Wwise.Event amb)
            {
                if (GameManager.Instance.config.soundActive.ambient ||
                    GameManager.Instance.config.soundActive.all)
                    amb.Post(go);

            }
            public void StopAmbient(GameObject go, AK.Wwise.Event amb)
            {
                if (GameManager.Instance.config.soundActive.ambient ||
                    GameManager.Instance.config.soundActive.all)
                    amb.Stop(go);
            }
        }

        [System.Serializable]
        public class BGMSoundList
        {
            #region BGMList
            public AK.Wwise.Event lobbyBGM = new AK.Wwise.Event();
            public AK.Wwise.Event stageBGM = new AK.Wwise.Event();
            public AK.Wwise.Event bossBGM = new AK.Wwise.Event();

            #endregion

            public void PlayBGM(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.bgm ||
                    GameManager.Instance.config.soundActive.all)
                    bgm.Post(go);
            }
            public void StopBGM(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.bgm ||
                    GameManager.Instance.config.soundActive.all)
                    bgm.Stop(go);
            }
        }

        [System.Serializable]
        public class ObjectSoundList
        {
            #region ObjectList
            public AK.Wwise.Event portalActive = new AK.Wwise.Event();
            public AK.Wwise.Event portalCreate = new AK.Wwise.Event();
            public AK.Wwise.Event portalEnter = new AK.Wwise.Event();
            public AK.Wwise.Event portalExit = new AK.Wwise.Event();

            #endregion

            public void PlayBGM(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    bgm.Post(go);
            }
        }

        public AmbientSoundList ambient;
        public BGMSoundList bgm;
        public ObjectSoundList objectSFX;
    }
}