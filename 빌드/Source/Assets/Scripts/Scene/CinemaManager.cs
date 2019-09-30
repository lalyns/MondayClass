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

        public PlayableDirector enterDirector;
        public Transform enterCam;
        public Vector3 enterVector3;
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
            enterVector3 = enterCam.position;
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


        public static void CinemaStart(PlayableDirector cinema)
        {
            Instance.transform.position = PlayerFSMManager.Instance.Anim.transform.position;
            Instance.transform.LookAt(MissionManager.Instance.CurrentMission.Exit.transform);
            Instance.enterCam.localPosition = Instance.enterVector3;
            PlayerFSMManager.Instance.transform.position = PlayerFSMManager.Instance.Anim.transform.position;
            PlayerFSMManager.Instance.transform.LookAt(MissionManager.Instance.CurrentMission.Exit.transform);
            cinema.gameObject.SetActive(true);
            cinema.Play();
        }
    }
}