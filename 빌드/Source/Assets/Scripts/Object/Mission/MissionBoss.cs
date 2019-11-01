using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;

namespace MC.Mission
{

    public class MissionBoss : MissionBase
    {
        public static MissionBoss _Instance;

        protected override void Start()
        {
            if (_Instance == null)
            {
                _Instance = GetComponent<MissionBoss>();
            }
            else
            {
                Destroy(gameObject);
            }

            var sound = MCSoundManager.Instance.objectSound;
            StartCoroutine(MCSoundManager.AmbFadeIn(1f));
            MCSoundManager.ChangeAMB(sound.ambient.stageAmbient);


        }

        private void Update()
        {

        }
    }
}