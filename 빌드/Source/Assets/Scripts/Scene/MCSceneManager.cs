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
        public const int CREDIT         = 6;
        public const int EDITOR         = 7;

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
           
        }

        public void NextScene(int sceneNumber, float duration, bool fading)
        {
            //if ((currentScene != TITLE || currentScene != BOSS || currentScene != CREDIT)
            //    && GameStatus.Instance.ActivedMonsterList.Count != 0)
            //    GameStatus.Instance.RemoveAllActiveMonster();

            Instance.StartCoroutine(MCSoundManager.BGMFadeOut(duration));
            Instance.StartCoroutine(MCSoundManager.AmbFadeOut(duration));
            
            if (fading)
            {
                GameManager.SetFadeInOut(() =>
                {
                    StartCoroutine(LoadScene(sceneNumber, duration));
                    GameStatus.SetCurrentGameState(CurrentGameState.Loading);
                }, duration, false
                );
            }
            else
            {
                StartCoroutine(LoadScene(sceneNumber, duration));
                GameStatus.SetCurrentGameState(CurrentGameState.Loading);
            }
        }

        public IEnumerator LoadScene(int sceneNumber, float duration)
        {
            yield return null;

            MCSoundManager.StopAMB();

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
            }

            if (async.isDone)
            {
                var bgm = MCSoundManager.Instance.objectSound.bgm;

                GameManager.SetSceneSetting();
                GameManager.SetFadeInOut(() =>
                {
                    //GameStatus.SetCurrentGameState(CurrentGameState.Wait);
                    GameManager.ScriptCheck();
                    UserInterface.BlurSet(false);
                    MCSoundManager.LoadBank();
                    isLoad = false;
                }, duration, true
            );
            }
        }
    }
}