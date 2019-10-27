using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Cinemachine;
using MC.UI;


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
            GameManager.SetFadeInOut(
                () =>
                {

                }, 1f, true);
        }

        public void FadeInFalse()
        {
            GameManager.SetFadeInOut(
                   () =>
                   {

                   }, 1f, false);
        }

        public void SceneEnd()
        {
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