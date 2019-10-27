using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Sound
{
    public class ObjectSound : MonoBehaviour
    {
        [System.Serializable]
        public class AmbientList
        {
            #region AmbientList
            public AK.Wwise.Event lobbyAmbient = new AK.Wwise.Event();
            public AK.Wwise.Event tutoAmbient = new AK.Wwise.Event();
            public AK.Wwise.Event stageAmbient = new AK.Wwise.Event();
            public AK.Wwise.Event bossAmbient = new AK.Wwise.Event();

            #endregion
            public void PlayAmbient(GameObject go, AK.Wwise.Event amb)
            {
                if (GameManager.Instance.config.soundActive.ambient ||
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        amb.Post(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }

            }
            public void StopAmbient(GameObject go, AK.Wwise.Event amb)
            {
                    if (GameManager.Instance.config.soundActive.ambient ||
                        GameManager.Instance.config.soundActive.all)
                        try
                        {
                            amb.Stop(go);
                        }
                        catch
                        {
                            MCSoundManager.LoadBank();
                        }
            }
        
        }

        [System.Serializable]
        public class BGMList
        {
            #region BGMList
            public AK.Wwise.Event lobbyBGM = new AK.Wwise.Event();
            public AK.Wwise.Event tutoBGM = new AK.Wwise.Event();
            public AK.Wwise.Event stageBGM = new AK.Wwise.Event();
            public AK.Wwise.Event bossBGM = new AK.Wwise.Event();

            #endregion

            public void PlayBGM(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.bgm &&
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        bgm.Post(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }
            }
            public void StopBGM(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.bgm &&
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        bgm.Stop(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }
            }
        }

        [System.Serializable]
        public class ObjectSoundList
        {
            #region ObjectList
            public AK.Wwise.Event portalActive = new AK.Wwise.Event();
            public AK.Wwise.Event portalLoop = new AK.Wwise.Event();

            public AK.Wwise.Event portalCreate = new AK.Wwise.Event();
            public AK.Wwise.Event portalEnter = new AK.Wwise.Event();
            public AK.Wwise.Event portalExit = new AK.Wwise.Event();

            public AK.Wwise.Event startCreate = new AK.Wwise.Event();
            public AK.Wwise.Event starDrop = new AK.Wwise.Event();
            public AK.Wwise.Event starGet = new AK.Wwise.Event();

            public AK.Wwise.Event pillarActive = new AK.Wwise.Event();
            public AK.Wwise.Event pillarDestroy = new AK.Wwise.Event();
            #endregion

            public void PlaySound(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        bgm.Post(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }
            }

            public void StopSound(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        bgm.Stop(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }
            }
        }

        [System.Serializable]
        public class UISoundList
        {
            #region UI
            public AK.Wwise.Event uiStart = new AK.Wwise.Event();
            public AK.Wwise.Event nextPage = new AK.Wwise.Event();
            public AK.Wwise.Event dead = new AK.Wwise.Event();
            #endregion

            public void PlaySound(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        bgm.Post(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }
            }

        }

        [System.Serializable]
        public class CinemaSoundList
        {
            #region CinemaSound

            public AK.Wwise.Event storyCrowd = new AK.Wwise.Event();
            public AK.Wwise.Event storyDive = new AK.Wwise.Event();
            public AK.Wwise.Event storyPhone = new AK.Wwise.Event();
            public AK.Wwise.Event storyWind = new AK.Wwise.Event();

            public AK.Wwise.Event bossEnterLast = new AK.Wwise.Event();
            #endregion

            public void PlaySound(GameObject go, AK.Wwise.Event bgm)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        bgm.Post(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }
            }
        }

        [System.Serializable]
        public class DialogVoice
        {
            #region DialogVoice
            public AK.Wwise.Event[] voice;

            #endregion

            public void PlaySound(GameObject go, AK.Wwise.Event voice)
            {
                if (GameManager.Instance.config.soundActive.voice ||
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        voice.Post(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }
            }
        }

        public AmbientList ambient;
        public BGMList bgm;
        public ObjectSoundList objectSFX;
        public UISoundList ui;
        public DialogVoice dialogVoice;
        public CinemaSoundList cinema;
    }
}