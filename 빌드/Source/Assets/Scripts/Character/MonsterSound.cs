using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Sound
{
    public class MonsterSound : MonoBehaviour
    {
        [System.Serializable]
        public class MonsterSFXList
        {
            #region ActionList
            public AK.Wwise.Event[] attackSFX = new AK.Wwise.Event[7];

            public AK.Wwise.Event monsterAppear = new AK.Wwise.Event();
            public AK.Wwise.Event monsterDisAppear = new AK.Wwise.Event();

            public AK.Wwise.Event redhatDash = new AK.Wwise.Event();
            public AK.Wwise.Event redhatDashHit = new AK.Wwise.Event();
            public AK.Wwise.Event redhatAttackReady = new AK.Wwise.Event();
            public AK.Wwise.Event redhatAttack = new AK.Wwise.Event();
            public AK.Wwise.Event redhatAttackHit = new AK.Wwise.Event();

            public AK.Wwise.Event macBigBall = new AK.Wwise.Event();
            public AK.Wwise.Event macBigBallHit = new AK.Wwise.Event();
            public AK.Wwise.Event macBigBallMove = new AK.Wwise.Event();
            public AK.Wwise.Event macSmallBall = new AK.Wwise.Event();
            public AK.Wwise.Event macSmallBallHit = new AK.Wwise.Event();

            public AK.Wwise.Event tiberSpinInit = new AK.Wwise.Event();
            public AK.Wwise.Event tiberSpin = new AK.Wwise.Event();
            public AK.Wwise.Event tiberSpinHit = new AK.Wwise.Event();
            public AK.Wwise.Event tiberStamp = new AK.Wwise.Event();
            public AK.Wwise.Event tiberStampDrop = new AK.Wwise.Event();
            public AK.Wwise.Event tiberStampBoom = new AK.Wwise.Event();
            #endregion

            public void PlayMonsterSFX(GameObject go, AK.Wwise.Event sfx)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all ||
                    GameStatus.currentGameState != CurrentGameState.Dead)
                    sfx.Post(go);
            }

            public void StopMonsterSFX(GameObject go, AK.Wwise.Event sfx)
            {
                sfx.Stop(go);
            }
        }

        [System.Serializable]
        public class MonsterVoiceList
        {
            #region
            public AK.Wwise.Event redhatDeadVoice = new AK.Wwise.Event();
            public AK.Wwise.Event redhatDamegedVoice = new AK.Wwise.Event();
            public AK.Wwise.Event redhatDashVoice = new AK.Wwise.Event();
            public AK.Wwise.Event redhatAttackVoice = new AK.Wwise.Event();

            public AK.Wwise.Event macBigBallVoice = new AK.Wwise.Event();
            public AK.Wwise.Event macDamageVoice = new AK.Wwise.Event();
            public AK.Wwise.Event macDieVoice = new AK.Wwise.Event();

            public AK.Wwise.Event tiberDamageVoice = new AK.Wwise.Event();
            public AK.Wwise.Event tiberDieVoice = new AK.Wwise.Event();
            public AK.Wwise.Event tiberSpinVoice = new AK.Wwise.Event();
            public AK.Wwise.Event tiberStompVoice = new AK.Wwise.Event();
            #endregion

            public void PlayMonsterVoice(GameObject go, AK.Wwise.Event voice)
            {
                if (GameManager.Instance.config.soundActive.voice ||
                    GameManager.Instance.config.soundActive.all)
                    voice.Post(go);
            }
        }

        public MonsterSFXList monsterSFX;
        public MonsterVoiceList monsterVoice;
    }
}