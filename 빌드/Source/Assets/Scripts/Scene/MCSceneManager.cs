using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MC.UI;

namespace MC.SceneDirector
{
    public class MCSceneManager : MonoBehaviour
    {
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
            if (SceneManager.GetActiveScene().name == "Title") { currentSceneNumber = 0; }
            if (SceneManager.GetActiveScene().name == "BuildScene") { currentSceneNumber = 1; }
            if (SceneManager.GetActiveScene().name == "Boss_Room_Cinemachine_Intro") { currentSceneNumber = 2; }

        }

        public void NextScene()
        {
            GameManager.SetFadeInOut(() =>
            {
                StartCoroutine(LoadScene(1));
                UserInterface.SetTitleUI(false);
                
            }, false
            );
        }

        public IEnumerator LoadScene(int i)
        {
            yield return null;

            if (!isLoad)
            {
                //++currentSceneNumber;
                currentSceneNumber = i;
                isLoad = true;
            }

            Debug.Log( "Scene Number : " + currentSceneNumber);
            AsyncOperation async = SceneManager.LoadSceneAsync(i);
            while (!async.isDone)
            {
                yield return null;

                Debug.Log(async.progress);
            }

            if (async.isDone)
            {
                GameManager.SetFadeInOut(() =>
                {
                    GameManager.SetSceneSetting();
                    isLoad = false;
                }, true
            );
            }
        }
    }
}