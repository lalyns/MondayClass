using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Cinemachine;
using MC.UI;
using MC.Sound;
using UnityEngine.SceneManagement;


namespace MC.SceneDirector
{

    public class CineAnimEvent : MonoBehaviour
    {
        public Camera cineCam;
        public Transform target;
        public GameObject whiteOut;

        public void SceneStart()
        {
            BossDirector.Instance.PlayScene();
            whiteOut.gameObject.SetActive(false);
        }

        public void BlackScreen()
        {
            CanvasInfo.Instance.Layers[1].worldCamera = cineCam;
            CanvasInfo.Instance.Layers[1].planeDistance = 0.3f;
            Color color = UserInterface.Instance.ScreenEffect.fading.image.color;
            color.a = 1f;
            UserInterface.Instance.ScreenEffect.fading.image.color = color;
        }

        public void FadeInTrue()
        {
            UserInterface.Instance.ScreenEffect.fading.image.color = Color.black;
            GameManager.SetFadeInOut(
                () =>
                {

                }, 1f, true);
        }

        public void FadeInFalse()
        {
            UserInterface.Instance.ScreenEffect.fading.image.color = Color.black;
            GameManager.SetFadeInOut(
                   () =>
                   {

                   }, 1f, false);
        }

        public void SceneEnd()
        {
            SceneManager.LoadScene(MCSceneManager.CREDIT);
        }

        public void WhiteFadeIn()
        {
            UserInterface.Instance.ScreenEffect.fading.image.color = Color.white;
            Color color = UserInterface.Instance.ScreenEffect.fading.image.color;
            color.a = 1f;
            UserInterface.Instance.ScreenEffect.fading.image.color = color;
            UserInterface.Instance.ScreenEffect.fading.image.color = Color.white;
            GameManager.SetFadeInOut(
                () =>
                {

                }, 1f, true);
        }

        public void WhiteFadeOut()
        {
            UserInterface.Instance.ScreenEffect.fading.image.color = Color.white;
            GameManager.SetFadeInOut(
                () =>
                {

                }, 1f, false);
        }

        public void StartSoundPlay()
        {
            MC.Sound.MCSoundManager.LoadBank();
            var sound = MCSoundManager.Instance.objectSound;
            MCSoundManager.ChangeBGM(sound.bgm.bossBGM);
            StartCoroutine(MCSoundManager.BGMFadeIn(0.7f));
        }

        public void PortalOpenPlay()
        {
            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.PlaySound(gameObject, sound.portalCreate);
        }

        public void PortalExit()
        {
            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.PlaySound(gameObject, sound.portalExit);
        }

        public void GalaxySurprise()
        {
            var sound = MCSoundManager.Instance.objectSound.dialogVoice;
            sound.PlaySound(gameObject, sound.voice[6]);
        }

        public void RirisSurprise()
        {
            var sound = MCSoundManager.Instance.objectSound.dialogVoice;
            sound.PlaySound(gameObject, sound.voice[13]);
        }

        public void RirisLaugh()
        {
            var sound = MCSoundManager.Instance.objectSound.dialogVoice;
            sound.PlaySound(gameObject, sound.voice[12]);
        }

        public void EnterLastSFX()
        {
            var sound = MCSoundManager.Instance.objectSound.cinema;
            sound.PlaySound(gameObject, sound.bossEnterLast);
        }

        public void PhaseChangeVoice()
        {
            var sound = RirisFSMManager.Instance.sound.ririsVoice;
            sound.PlayRirisVoice(RirisFSMManager.Instance.gameObject, sound.phase3init);
        }

        public void PhageChange()
        {
            var sound = RirisFSMManager.Instance.sound.ririsSFX;
            sound.PlayRirisSFX(RirisFSMManager.Instance.gameObject, sound.changePhase);
        }

        public void RirisFalldown()
        {
            var sound = RirisFSMManager.Instance.sound.ririsSFX;
            sound.PlayRirisSFX(RirisFSMManager.Instance.gameObject, sound.falldownSFX);
        }

        public void RirisDeadVoice()
        {
            var sound = RirisFSMManager.Instance.sound.ririsVoice;
            sound.PlayRirisVoice(RirisFSMManager.Instance.gameObject, sound.dead);
        }

        public void RirisDeadDissolveSound()
        {
            var sound = RirisFSMManager.Instance.sound2.monsterSFX;
            sound.PlayMonsterSFX(RirisFSMManager.Instance.gameObject, sound.monsterDisAppear);
        }

        public void DeadEndSound()
        {
            var sound = MCSoundManager.Instance.objectSound.cinema;
            sound.PlaySound(RirisFSMManager.Instance.gameObject, sound.storyWind);
        }

        public void LookAtCamDefine() 
        {
            GetComponent<CinemachineVirtualCamera>().LookAt = target;
        }

        public void LookAtCamNull() {
            GetComponent<CinemachineVirtualCamera>().LookAt = null;
        }

        public void EnterMissionNotify()
        {
            GameManager.SetFadeInOut(() =>
            {

                GameManager.Instance.CharacterControl = true;
                MissionManager.Instance.isChange = false;

            }, 1f,
            true);
        }

    }
}