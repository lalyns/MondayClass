﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;


namespace MC.SceneDirector
{

    public class CineAnimEvent : MonoBehaviour
    {
        public void SceneStart()
        {
            CinemaManager.Instance.SceneStart();
        }

        public void SceneEnd()
        {
            CinemaManager.CinemaEnd();
        }

        public void EnterMissionNotify()
        {
            GameManager.SetFadeInOut(() =>
            {

                GameManager.Instance.CharacterControl = true;
                MissionManager.Instance.isChange = false;

                // RigidBody 활성화
            }, 
            true);
        }

    }
}