using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace MC.SceneDirector
{
    public class CinemaManager : MonoBehaviour
    {
        private static CinemaManager _Instance;
        public static CinemaManager Instance {
            get => _Instance;
        }

        public int _SceneNumber;

        public PlayableDirector BossDirector;
        public GameObject CineSet;
        public GameObject PlaySet;

        public PlayableDirector currentDirector;

        private void Awake()
        {
            if (_Instance == null)
            {
                _Instance = GetComponent<CinemaManager>();
            }
            else
            {
                return;
            }
        }

        private void Start()
        {

        }

        public void SceneStart()
        {
        }

        public static void CinemaStart(PlayableDirector cinema)
        {
        }

        public static void CinemaEnd()
        {
            //Debug.Log("Directing End : " + Instance.currentDirector.transform.name);
            Instance.currentDirector.gameObject.SetActive(false);
        }
    }
}