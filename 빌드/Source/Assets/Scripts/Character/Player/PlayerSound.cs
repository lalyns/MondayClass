using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Sound
{
    public class PlayerSound : MonoBehaviour
    {
        [System.Serializable]
        public class PlayerSFXList
        {
            #region ActionList
            public AK.Wwise.Event footstepSFX = new AK.Wwise.Event();
            public AK.Wwise.Event attackSFX = new AK.Wwise.Event();
            public AK.Wwise.Event hitSFX = new AK.Wwise.Event();

            #endregion

            #region SkillList
            public AK.Wwise.Event teleportSFX = new AK.Wwise.Event();

            #endregion

            public void PlayPlayerSFX(GameObject go, AK.Wwise.Event sfx)
            {
                //Debug.Log("Play Player SFX : " + sfx.Name + MCSoundManager.SoundCall++);
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    sfx.Post(go);
            }
        }

        [System.Serializable]
        public class PlayerVoiceList
        {
            public AK.Wwise.Event teleportVoice = new AK.Wwise.Event();


            public void PlayPlayerVoice(GameObject go, AK.Wwise.Event voice)
            {
                //Debug.Log("Play Player Voice");
                if (GameManager.Instance.config.soundActive.voice ||
                    GameManager.Instance.config.soundActive.all)
                    voice.Post(go);
            }
        }

        public PlayerSFXList sfx;
        public PlayerVoiceList voice;

    }
}
