using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public class PlayerSound : MonoBehaviour
    {
        [System.Serializable]
        public class PlayerSFXList
        {
            #region ActionList
            public AK.Wwise.Event footstepSFX = new AK.Wwise.Event();
            public AK.Wwise.Event attackSFX = new AK.Wwise.Event();
            public AK.Wwise.Event hitSFX = new AK.Wwise.Event();

            public void PlayFootstep(GameObject gameObject)
            {
                footstepSFX.Post(gameObject);
            }
            public void PlayAttack(GameObject gameObject)
            {
                attackSFX.Post(gameObject);
            }
            public void PlayHit(GameObject gameObject)
            {
                hitSFX.Post(gameObject);
            }
            #endregion

            #region SkillList
            public AK.Wwise.Event teleportSFX = new AK.Wwise.Event();

            public void PlayTeleport(GameObject gameObject)
            {
                teleportSFX.Post(gameObject);
            }
            #endregion
        }

        [System.Serializable]
        public class PlayerVoiceList
        {
            public AK.Wwise.Event teleportVoice = new AK.Wwise.Event();

            public void PlayTeleportVoice(GameObject gameObject)
            {
                teleportVoice.Post(gameObject);
            }
        }

        public PlayerSFXList sfxList;
        public PlayerVoiceList voiceList;



    }
}
