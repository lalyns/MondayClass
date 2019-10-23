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
        public const int EDITOR         = 6;

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
                GameStatus.SetCurrentGameState(CurrentGameState.Wait);
                currentScene = TITLE;
            }
            else if (SceneManager.GetActiveScene().name == "01-0.Tutorial") { 
                GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
                currentScene = TUTORIAL;
            }
            else if (SceneManager.GetActiveScene().name == "01-1.Stage1") {
                currentScene = ANNIHILATION;
            }
            else if (SceneManager.GetActiveScene().name == "02.Stage2") {
                currentScene = SURVIVAL;
            }
            else if (SceneManager.GetActiveScene().name == "03.Stage3") {
                currentScene = DEFENCE;
            }
            else if (SceneManager.GetActiveScene().name == "04.Boss") {
                currentScene = BOSS;
            }
            else
            {
                currentScene = EDITOR;
                GameStatus.SetCurrentGameState(CurrentGameState.EDITOR);
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
                        currentScene == DEFENCE)
                    {
                        bgm.PlayBGM(MCSoundManager.Instance.gameObject, bgm.stageBGM);
                    }

                    if(currentScene == BOSS)
                    {
                        bgm.PlayBGM(MCSoundManager.Instance.gameObject, bgm.bossBGM);
                    }

                    isPlay = true;
                }
                catch
                {

                }
            }
        }

        public void NextScene(int sceneNumber, string soundType, float duration, bool fading)
        {
            if(currentScene != TITLE && GameStatus.Instance.ActivedMonsterList.Count != 0)
                GameStatus.Instance.RemoveAllActiveMonster();

            if (fading)
            {
                GameManager.SetFadeInOut(() =>
                {
                    StartCoroutine(LoadScene(sceneNumber, soundType, duration));
                    GameStatus.SetCurrentGameState(CurrentGameState.Loading);
                }, soundType, duration, false
                );
            }
            else
            {
                StartCoroutine(LoadScene(sceneNumber, soundType, duration));
                GameStatus.SetCurrentGameState(CurrentGameState.Loading);
            }
        }

        public IEnumerator LoadScene(int sceneNumber, string soundType, float duration)
        {
            yield return null;

            if (!isLoad)
            {
                prevScene = currentScene;
                currentScene = sceneNumber;
                isLoad = true;
            }

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNumber);
            while (!async.isDone)
            {
                yield return null;

                Debug.Log("Loading");
            }

            if (async.isDone)
            {
                var bgm = MCSoundManager.Instance.objectSound.bgm;
                Debug.Log("Load Done");
                if (prevScene == TITLE && 
                    (prevScene == ANNIHILATION ||
                    prevScene == SURVIVAL ||
                    prevScene == DEFENCE)
                    )
                {
                    bgm.PlayBGM(MCSoundManager.Instance.gameObject, bgm.stageBGM);
                }

                if((prevScene == ANNIHILATION ||
                    prevScene == SURVIVAL ||
                    prevScene == DEFENCE) &&
                    currentScene == BOSS)
                {
                    bgm.PlayBGM(MCSoundManager.Instance.gameObject, bgm.bossBGM);
                }

                GameManager.SetSceneSetting();
                GameManager.SetFadeInOut(() =>
                {
                    Debug.Log("Fade In Load After");
                    //GameStatus.currentGameState = CurrentGameState.Wait;
                    GameManager.ScriptCheck();
                    MCSoundManager.LoadBank();
                    isLoad = false;
                }, soundType, duration, true
            );
            }
        }
    }
}