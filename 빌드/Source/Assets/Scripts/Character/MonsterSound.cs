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
            public AK.Wwise.Event attackSFX = new AK.Wwise.Event();

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