using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission {

    public class TutorialEventTrigger : MonoBehaviour
    {
        public TutorialEvent tutoEvent;

        public MissionTutorial mission {
            get => GetComponentInParent<MissionTutorial>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.tag == "Player")
            {
                if(tutoEvent == TutorialEvent.Attack1 && mission.currentTutorial != TutorialEvent.End)
                {
                    Invoke("Attack1Event", 0.5f);
                }
            }
        }

        public void Attack1Event()
        {
            mission.SetAttack1Event();
        }
    }
}