using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Cinemachine;
using MC.UI;


namespace MC.SceneDirector
{

    public class CineAnimEvent : MonoBehaviour
    {

        public Transform target;

        public void SceneStart()
        {
            TempDirector.Instance.SceneStart();
        }

        public void SceneEnd()
        {
            //TempDirector.Instance.CinemaEnd();
        }

        public void LookAtCamDefine() 
        {
            GetComponent<CinemachineVirtualCamera>().LookAt = target;
        }

        public void LookAtCamNull() {
            GetComponent<CinemachineVirtualCamera>().LookAt = null;
        }

        public void EnterMissionNotify()
        {
            GameManager.SetFadeInOut(() =>
            {

                GameManager.Instance.CharacterControl = true;
                MissionManager.Instance.isChange = false;

            }, 
            true);
        }

    }
}