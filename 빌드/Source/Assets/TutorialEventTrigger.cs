using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission {

    public class TutorialEventTrigger : MonoBehaviour
    {
        public TutorialEvent tutoEvent;
        bool eventPlay = false;

        public MissionTutorial mission {
            get => GetComponentInParent<MissionTutorial>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.tag == "Player")
            {
                if(tutoEvent == TutorialEvent.Attack && mission.currentTutorial != TutorialEvent.End
                    && !eventPlay)
                {
                    eventPlay = true;
                    Invoke("Attack1Event", 0.5f);
                }
            }
        }

        public void Attack1Event()
        {
            mission.SetAttackEvent();
        }
    }
}