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

            public AK.Wwise.Event specialWinkSFX = new AK.Wwise.Event();
            #endregion

            #region SkillList
            public AK.Wwise.Event teleportSFX = new AK.Wwise.Event();

            public AK.Wwise.Event skill1SFX = new AK.Wwise.Event();

            public AK.Wwise.Event skill2SFX = new AK.Wwise.Event();
            public AK.Wwise.Event skill2LastSFX = new AK.Wwise.Event();

            public AK.Wwise.Event skill3CastSFX = new AK.Wwise.Event();
            public AK.Wwise.Event skill3LoopSFX = new AK.Wwise.Event();
            public AK.Wwise.Event skill3FinishSFX = new AK.Wwise.Event();
            public AK.Wwise.Event skill3HitSFX = new AK.Wwise.Event();
            public AK.Wwise.Event skill3CancleSFX = new AK.Wwise.Event();

            public AK.Wwise.Event specialSwingSFX = new AK.Wwise.Event();
            public AK.Wwise.Event specialGripSFX = new AK.Wwise.Event();
            public AK.Wwise.Event specialSpinSFX = new AK.Wwise.Event();
            public AK.Wwise.Event specialJumpSFX = new AK.Wwise.Event();
            public AK.Wwise.Event specialHeartSFX = new AK.Wwise.Event();
            public AK.Wwise.Event specialVioletBeamSFX = new AK.Wwise.Event();

            #endregion

            public void PlayPlayerSFX(GameObject go, AK.Wwise.Event sfx)
            {
                //Debug.Log("Play Player SFX : " + sfx.Name + MCSoundManager.SoundCall++);
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    try
                    {
                        sfx.Post(go);
                    }
                    catch
                    {
                        MCSoundManager.LoadBank();
                    }
            }
        }

        [System.Serializable]
        public class PlayerVoiceList
        {
            public AK.Wwise.Event damagedVoice = new AK.Wwise.Event();
            public AK.Wwise.Event dieVoice = new AK.Wwise.Event();
            public AK.Wwise.Event swingHardVoice = new AK.Wwise.Event();
            public AK.Wwise.Event swingNormalVoice = new AK.Wwise.Event();
            public AK.Wwise.Event swingSpeicialVoice = new AK.Wwise.Event();

            public AK.Wwise.Event emoteRageVoice = new AK.Wwise.Event();
            public AK.Wwise.Event emoteSadVoice = new AK.Wwise.Event();
            public AK.Wwise.Event emoteSmileVoice = new AK.Wwise.Event();
            public AK.Wwise.Event emoteSurprisedVoice = new AK.Wwise.Event();

            public AK.Wwise.Event sigh = new AK.Wwise.Event();
            public AK.Wwise.Event humming = new AK.Wwise.Event();
            public AK.Wwise.Event singing = new AK.Wwise.Event();
            public AK.Wwise.Event victory = new AK.Wwise.Event();

            public AK.Wwise.Event skill1Voice = new AK.Wwise.Event();
            public AK.Wwise.Event skill2Voice = new AK.Wwise.Event();
            public AK.Wwise.Event skill3CastVoice = new AK.Wwise.Event();
            public AK.Wwise.Event skill3FinishVoice = new AK.Wwise.Event();
            public AK.Wwise.Event skill4CastVoice = new AK.Wwise.Event();
            public AK.Wwise.Event skill4FinishVoice = new AK.Wwise.Event();

            public AK.Wwise.Event teleportVoice = new AK.Wwise.Event();
            public AK.Wwise.Event specialCastVoice = new AK.Wwise.Event();
            public AK.Wwise.Event specialFinishVoice = new AK.Wwise.Event();

            public void PlayPlayerVoice(GameObject go, AK.Wwise.Event voice)
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

        public PlayerSFXList sfx;
        public PlayerVoiceList voice;

    }
}
