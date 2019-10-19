using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using MC.SceneDirector;
using MC.Sound;

namespace MC.UI
{
    [System.Serializable]
    public class Title
    {
        public GameObject gameObject;

        public Button start;
        public Button howToPlay;
        public Button config;
        public Button exit;

    }

    public class UITitle : MonoBehaviour
    {
        public Title title;

        public GameObject cutScene;
        public GameObject howToPlay;
        public GameObject config;

        bool nextScene = true;

        public void StartButton()
        {
            //cutScene.SetActive(true);

            if (nextScene)
            {
                var ui = MCSoundManager.Instance.objectSound.ui;
                ui.PlaySound(this.gameObject, ui.uiStart);
                MCSceneManager.Instance.NextScene(MCSceneManager.TUTORIAL);
                nextScene = false;
            }
        }

        public void HowToPlayButton()
        {
            howToPlay.SetActive(true);
        }

        public void HowToPlayExit()
        {
            howToPlay.SetActive(false);
        }

        public void ConfigButton()
        {
            config.SetActive(true);
        }

        public void ConfigExit()
        {
            config.SetActive(false);
        }

        public void ExitButton()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif
        }
    }
}