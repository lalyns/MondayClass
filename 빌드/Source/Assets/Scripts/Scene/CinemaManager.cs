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
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {

        }

        public void SceneStart()
        {
            CineSet.SetActive(false);
            PlaySet.SetActive(true);
        }

        public static void CinemaStart(PlayableDirector cinema)
        {
            PlayerFSMManager.Instance.transform.position = PlayerFSMManager.Instance.Anim.transform.position;
            PlayerFSMManager.Instance.transform.LookAt(MissionManager.Instance.CurrentMission.Exit.transform);

            cinema.gameObject.SetActive(true);
            Instance.currentDirector = cinema;
            cinema.Play();
        }

        public static void CinemaEnd()
        {
            //Debug.Log("Directing End : " + Instance.currentDirector.transform.name);
            Instance.currentDirector.gameObject.SetActive(false);
        }
    }
}