using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MC.UI;
using MC.Sound;

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


        int prevScene = -1;
        public static int currentScene = 0;

        private bool isLoad;

        private bool isPlay;

        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Screen.SetResolution(1920, 1080, true);

            if (SceneManager.GetActiveScene().name == "00.Title") {
                GameStatus.currentGameState = CurrentGameState.Wait;
                currentScene = TITLE;
            }
            if (SceneManager.GetActiveScene().name == "01-0.Tutorial") {
                GameStatus.currentGameState = CurrentGameState.Tutorial;
                currentScene = TUTORIAL;
            }
            if (SceneManager.GetActiveScene().name == "01-1.Stage1") {
                currentScene = ANNIHILATION;
            }
            if (SceneManager.GetActiveScene().name == "02.Stage2") {
                currentScene = SURVIVAL;
            }
            if (SceneManager.GetActiveScene().name == "03.Stage3") {
                currentScene = DEFENCE;
            }
            if (SceneManager.GetActiveScene().name == "04.Boss") {
                currentScene = BOSS;
            }
        }

        private void Update()
        {
            if (!isPlay)
            {
                try
                {
                    MCSoundManager.LoadBank();
                    var bgm = MCSoundManager.Instance.objectSound.bgm;

                    if (currentScene == TITLE)
                    {
                        bgm.PlayBGM(MCSoundManager.Instance.gameObject, bgm.lobbyBGM);
                    }

                    if (currentScene == TUTORIAL ||
                        currentScene == ANNIHILATION ||
                        currentScene == SURVIVAL ||
                        currentScene == DEFENCE ||
                        currentScene == BOSS)
                    {
                        bgm.PlayBGM(MCSoundManager.Instance.gameObject, bgm.stageBGM);
                    }

                    isPlay = true;
                }
                catch
                {

                }
            }
        }

        public void NextScene(int i)
        {
            var bgm = MCSoundManager.Instance.objectSound.bgm;

            GameManager.SetFadeInOut(() =>
            {
                StartCoroutine(LoadScene(i));
                GameStatus.SetCurrentGameState(CurrentGameState.Loading);
                GameManager.Instance.CharacterControl = false;
                Debug.Log(GameStatus.currentGameState.ToString());
                bgm.StopBGM(MCSoundManager.Instance.gameObject, bgm.lobbyBGM);
            }, false
            );
        }

        public IEnumerator LoadScene(int i)
        {
            yield return null;


            if (!isLoad)
            {
                prevScene = currentScene;
                currentScene = i;
                isLoad = true;
            }

            AsyncOperation async = SceneManager.LoadSceneAsync(i);
            while (!async.isDone)
            {
                yield return null;

            }

            if (async.isDone)
            {
                if (prevScene == TITLE)
                {
                    var bgm = MCSoundManager.Instance.objectSound.bgm;
                    bgm.PlayBGM(MCSoundManager.Instance.gameObject, bgm.stageBGM);
                }

                GameManager.SetSceneSetting();
                GameManager.SetFadeInOut(() =>
                {
                    //GameStatus.currentGameState = CurrentGameState.Wait;
                    GameManager.ScriptCheck();
                    MCSoundManager.LoadBank();
                    isLoad = false;
                }, true
            );
            }
        }
    }
}