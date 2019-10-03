using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MC.UI;

namespace MC.SceneDirector
{
    public class MCSceneManager : MonoBehaviour
    {
        public const int TITLE          = 0;
        public const int TUTORIAL       = 1;
        public const int ANNIHILATION   = 2;
        public const int SURVIVAL       = 3;
        public const int DEFENCE        = 4;
        public const int BOSS           = 5;

        private static MCSceneManager _Instance;
        public static MCSceneManager Instance {
            get {
                if (_Instance == null)
                    _Instance = GameObject.FindGameObjectWithTag("GameController").
                        GetComponent<MCSceneManager>();
                return _Instance;
            }
        }

        public static int currentSceneNumber = 0;

        private bool isLoad;


        private void Awake()
        {
            if (SceneManager.GetActiveScene().name == "00.Title") {
                currentSceneNumber = TITLE;
            }
            if (SceneManager.GetActiveScene().name == "01-0.Tutorial") {
                currentSceneNumber = TUTORIAL;
            }
            if (SceneManager.GetActiveScene().name == "01-1.Stage1") {
                currentSceneNumber = ANNIHILATION;
            }
            if (SceneManager.GetActiveScene().name == "02.Stage2") {
                currentSceneNumber = SURVIVAL;
            }
            if (SceneManager.GetActiveScene().name == "03.Stage3") {
                currentSceneNumber = DEFENCE;
            }
            if (SceneManager.GetActiveScene().name == "04.Boss") {
                currentSceneNumber = BOSS;
            }
        }

        public void NextScene(int i)
        {
            GameManager.SetFadeInOut(() =>
            {
                StartCoroutine(LoadScene(i));
                
            }, false
            );
        }

        public IEnumerator LoadScene(int i)
        {
            yield return null;

            if (!isLoad)
            {
                currentSceneNumber = i;
                isLoad = true;
            }

            AsyncOperation async = SceneManager.LoadSceneAsync(i);
            while (!async.isDone)
            {
                yield return null;

            }

            if (async.isDone)
            {
                GameManager.SetSceneSetting();
                GameManager.SetFadeInOut(() =>
                {
                    MCSoundManager.LoadBank();
                    isLoad = false;
                }, true
            );
            }
        }
    }
}