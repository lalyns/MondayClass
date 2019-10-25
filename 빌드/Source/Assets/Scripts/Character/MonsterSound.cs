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

            public AK.Wwise.Event redhatDash = new AK.Wwise.Event();
            public AK.Wwise.Event redhatDashReady = new AK.Wwise.Event();
            public AK.Wwise.Event redhatAttack = new AK.Wwise.Event();
            #endregion

            #region SkillList

            #endregion

            public void PlayMonsterSFX(GameObject go, AK.Wwise.Event sfx)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    sfx.Post(go);
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