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
            UserInterface.SetPointerMode(true);

            MCSoundManager.LoadBank();
            var sound = MCSoundManager.Instance.objectSound;
            MCSoundManager.ChangeBGM(sound.bgm.lobbyBGM);
            MCSoundManager.ChangeAMB(sound.ambient.lobbyAmbient);
        }

        public void StartButton()
        {
            //cutScene.SetActive(true);

            title.start.interactable = false;
            title.setting.interactable = false;
            title.developer.interactable = false;
            title.exit.interactable = false;

            GameSetting.rewardAbillity.strLevel = 0;
            GameSetting.rewardAbillity.defLevel = 0;
            GameSetting.rewardAbillity.hpLevel = 0;
            GameSetting.rewardAbillity.skill1DMGLevel = 0;
            GameSetting.rewardAbillity.skill1BounceLevel = 0;
            GameSetting.rewardAbillity.skill2DMGLevel = 0;
            GameSetting.rewardAbillity.skill3DMGLevel = 0;
            GameSetting.rewardAbillity.skill3TurnLevel = 0;
            GameStatus.Instance.StageLevel = 0;

            if (nextScene)
            {
                var ui = MCSoundManager.Instance.objectSound.ui;
                ui.PlaySound(this.gameObject, ui.uiStart);

                StartCoroutine(MCSoundManager.BGMFadeOut(1f));
                StartCoroutine(MCSoundManager.AmbFadeOut(1f));


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
            title.setting.interactable = false;
            title.developer.interactable = false;
            title.exit.interactable = false;

            UserInterface.BlurSet(true, 8f);

            var sound = MCSoundManager.Instance.objectSound.ui;
            sound.PlaySound(MCSoundManager.Instance.gameObject, sound.nextPage);
        }

        public void Developer()
        {
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