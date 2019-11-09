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
            public AK.Wwise.Event teleportSFX = new AK.Wwise.Event();
            public AK.Wwise.Event falldownSFX = new AK.Wwise.Event();

            public AK.Wwise.Event changePhase = new AK.Wwise.Event();

            public AK.Wwise.Event jumpSFX = new AK.Wwise.Event();
            public AK.Wwise.Event dropSFX = new AK.Wwise.Event();
            public AK.Wwise.Event stompSFX = new AK.Wwise.Event();

            public AK.Wwise.Event dashSFX = new AK.Wwise.Event();
            public AK.Wwise.Event dashLoopSFX = new AK.Wwise.Event();

            public AK.Wwise.Event batSwarmFirstSFX = new AK.Wwise.Event();
            public AK.Wwise.Event batSwarmLoopSFX = new AK.Wwise.Event();

            public AK.Wwise.Event tornaedoCastSFX = new AK.Wwise.Event();
            public AK.Wwise.Event tornaedoFirstSFX = new AK.Wwise.Event();
            public AK.Wwise.Event tornaedoLastSFX = new AK.Wwise.Event();

            public AK.Wwise.Event ultGate = new AK.Wwise.Event();
            public AK.Wwise.Event ultSmallGate = new AK.Wwise.Event();
            public AK.Wwise.Event ultBeam = new AK.Wwise.Event();
            public AK.Wwise.Event ultBlast = new AK.Wwise.Event();

            public AK.Wwise.Event ririsPowerUp = new AK.Wwise.Event();
            public AK.Wwise.Event ririsPowerUpEnd = new AK.Wwise.Event();
            public AK.Wwise.Event ririsPowerUpLoop = new AK.Wwise.Event();

            #endregion

            #region SkillList

            #endregion

            public void PlayRirisSFX(GameObject go, AK.Wwise.Event sfx)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    sfx.Post(go);
            }

            public void StopRirisSFX(GameObject go, AK.Wwise.Event sfx)
            {
                if (GameManager.Instance.config.soundActive.sfx ||
                    GameManager.Instance.config.soundActive.all)
                    sfx.Stop(go);
            }
        }

        [System.Serializable]
        public class RirisVoiceList
        {
            #region VoiceList
            public AK.Wwise.Event dead = new AK.Wwise.Event();
            public AK.Wwise.Event smile = new AK.Wwise.Event();
            public AK.Wwise.Event surprise1 = new AK.Wwise.Event();
            public AK.Wwise.Event surprise2 = new AK.Wwise.Event();
            public AK.Wwise.Event batswarm = new AK.Wwise.Event();

            public AK.Wwise.Event jump = new AK.Wwise.Event();
            public AK.Wwise.Event stomp = new AK.Wwise.Event();

            public AK.Wwise.Event darkblast = new AK.Wwise.Event();

            public AK.Wwise.Event dash = new AK.Wwise.Event();

            public AK.Wwise.Event special = new AK.Wwise.Event();
            public AK.Wwise.Event phase3init = new AK.Wwise.Event();
            #endregion

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