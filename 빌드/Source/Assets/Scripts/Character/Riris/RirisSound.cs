using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Sound
{

    public class RirisSound : MonoBehaviour
    {
        [System.Serializable]
        public class RirisSFXList
        {
            #region ActionList

            #endregion

            #region SkillList

            #endregion

            public void PlayRirisSFX(GameObject go, AK.Wwise.Event sfx)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    sfx.Post(go);
            }
        }

        [System.Serializable]
        public class RirisVoiceList
        {


            public void PlayRirisVoice(GameObject go, AK.Wwise.Event voice)
            {
                if (GameManager.Instance.config.soundActive.voice ||
                    GameManager.Instance.config.soundActive.all)
                    voice.Post(go);
            }
        }

        public RirisSFXList ririsSFX;
        public RirisVoiceList ririsVoice;
    }
}