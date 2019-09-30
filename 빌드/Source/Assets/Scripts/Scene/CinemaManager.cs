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

        private void Awake()
        {
            if (_Instance == null)
            {
                _Instance = GetComponent<CinemaManager>();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            if (_SceneNumber == 3)
            {
                PlaySet.SetActive(false);
                BossDirector.Play();
            }

        }

        public void SceneStart()
        {
            CineSet.SetActive(false);
            PlaySet.SetActive(true);
        }


        public void CinemaStart(PlayableDirector cinema)
        {
            cinema.Play();
        }
    }
}