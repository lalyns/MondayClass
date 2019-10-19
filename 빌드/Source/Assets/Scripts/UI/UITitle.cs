﻿using System.Collections;
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
        public Button config;
        public Button exit;

    }

    public class UITitle : MonoBehaviour
    {
        public Title title;

        public TitleCutScene cutScene;
        public GameObject config;

        bool nextScene = true;

        public void StartButton()
        {
            //cutScene.SetActive(true);

            if (nextScene)
            {
                var ui = MCSoundManager.Instance.objectSound.ui;
                ui.PlaySound(this.gameObject, ui.uiStart);

                cutScene.CineStart();
                nextScene = false;
            }
        }

        public void NextScene()
        {
            MCSceneManager.Instance.NextScene(MCSceneManager.TUTORIAL, false);
            //StartCoroutine(MCSceneManager.Instance.LoadScene(MCSceneManager.TUTORIAL));
            GameManager.Instance.CharacterControl = false;

            var bgm = MCSoundManager.Instance.objectSound.bgm;
            bgm.StopBGM(MCSoundManager.Instance.gameObject, bgm.lobbyBGM);
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