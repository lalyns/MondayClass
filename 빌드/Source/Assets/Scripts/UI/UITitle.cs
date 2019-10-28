using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MC.SceneDirector;
using MC.Sound;

namespace MC.UI
{
    [System.Serializable]
    public class Title
    {
        public GameObject gameObject;

        public Button start;
        public Button setting;
        public Button developer;
        public Button exit;

    }

    public class UITitle : MonoBehaviour
    {
        public Title title;

        public TitleCutScene cutScene;
        public GameObject Setting => CanvasInfo.Instance.setting;

        bool nextScene = true;

        public void OnEnable()
        {
            MCSoundManager.LoadBank();
            var sound = MCSoundManager.Instance.objectSound;
            MCSoundManager.ChangeBGM(sound.bgm.lobbyBGM);
            MCSoundManager.ChangeAMB(sound.ambient.lobbyAmbient);
        }

        public void StartButton()
        {
            //cutScene.SetActive(true);

            if (nextScene)
            {
                var ui = MCSoundManager.Instance.objectSound.ui;
                ui.PlaySound(this.gameObject, ui.uiStart);

                StartCoroutine(MCSoundManager.BGMFadeOut(1f));
                StartCoroutine(MCSoundManager.AmbFadeOut(1f));

                title.start.interactable = false;
                title.developer.interactable = false;
                title.exit.interactable = false;

                cutScene.CineStart();
                GameStatus.SetCurrentGameState(CurrentGameState.Product);
                nextScene = false;
            }
        }

        public void NextScene()
        {
            MCSceneManager.Instance.NextScene(MCSceneManager.TUTORIAL, 1f, false);
            //StartCoroutine(MCSceneManager.Instance.LoadScene(MCSceneManager.TUTORIAL));
            GameManager.Instance.CharacterControl = false;

            var bgm = MCSoundManager.Instance.objectSound.bgm;
            bgm.StopBGM(MCSoundManager.Instance.gameObject, bgm.lobbyBGM);
        }

        public void SettingButton()
        {
            Setting.SetActive(true);
            title.start.interactable = false;
            title.developer.interactable = false;
            title.exit.interactable = false;

            var sound = MCSoundManager.Instance.objectSound.ui;
            sound.PlaySound(MCSoundManager.Instance.gameObject, sound.nextPage);
        }

        public void Developer()
        {
            Debug.Log("개발자 : ??");

            MCSoundManager.StopAMB();
            MCSoundManager.StopBGM();
            SceneManager.LoadScene(MCSceneManager.CREDIT);
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