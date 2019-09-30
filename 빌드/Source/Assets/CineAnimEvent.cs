using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MC.SceneDirector
{

    public class CineAnimEvent : MonoBehaviour
    {
        public void SceneStart()
        {
            CinemaManager.Instance.SceneStart();
        }


    }
}