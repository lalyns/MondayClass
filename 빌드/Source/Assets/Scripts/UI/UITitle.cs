using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.SceneDirector;

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

        public void StartButton()
        {
            MCSceneManager.Instance.NextScene(MCSceneManager.SURVIVAL);
        }

        public void HowtoplayButton()
        {

        }

        public void ConfigButton()
        {

        }

        public void ExitButton()
        {
#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE

            Application.Quit();

#endif
        }

    }
}