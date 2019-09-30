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
                if (_Instance)
                    _Instance = GameObject.FindGameObjectWithTag("Manager").
                        GetComponentInChildren<MCSceneManager>();
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

            //Debug.Log(currentSceneNumber);
        }

        public void NextScene()
        {
            Debug.Log("Next");
            GameManager.SetFadeInOut(() =>
            {
                StartCoroutine(LoadScene());
                UserInterface.SetTitleUI(false);
                
            }, false
            );
        }

        IEnumerator LoadScene()
        {
            yield return null;

            if (!isLoad)
            {
                ++currentSceneNumber;
                isLoad = true;
            }

            AsyncOperation async = SceneManager.LoadSceneAsync(currentSceneNumber);
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