using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.SceneDirector;
using UnityEngine.UI;
using UnityEngine.Playables;

using MC.Sound;

namespace MC.UI
{

    public class TitleCutScene : MonoBehaviour
    {
        bool cinePlay = false;
        public PlayableDirector playableDirector;

        UITitle uiTitle => GetComponentInParent<UITitle>();

        public void CineStart()
        {
            GameManager.SetFadeInOut(() =>
            {
                playableDirector.gameObject.SetActive(true);
                Invoke("PlayDirector", 2f);
            }, "Bgm_Start_Fade_In", 1f, false);
        }

        public void PlayDirector()
        {
            playableDirector.Play();
        }

        public void CineStop()
        {
            playableDirector.Pause();
            cinePlay = false;
        }

        public void CineNext()
        {
            playableDirector.Resume();

            var sound = MCSoundManager.Instance.objectSound.ui;
            sound.PlaySound(this.gameObject, sound.nextPage);
        }

        public void PlayCrowd()
        {
            var sound = MCSoundManager.Instance.objectSound.cinema;
            sound.PlaySound(this.gameObject, sound.storyCrowd);
        }

        public void PlayDive()
        {

            var sound = MCSoundManager.Instance.objectSound.cinema;
            sound.PlaySound(this.gameObject, sound.storyDive);
        }

        public void PlayPhone()
        {

            var sound = MCSoundManager.Instance.objectSound.cinema;
            sound.PlaySound(this.gameObject, sound.storyPhone);
        }
        
        public void PlayPangYi()
        {

            var sound = MCSoundManager.Instance.objectSound.dialogVoice;
            sound.PlaySound(this.gameObject, sound.voice[9]);
        }

        public void GalaxySurprised()
        {

            var sound = MCSoundManager.Instance.objectSound.dialogVoice;
            sound.PlaySound(this.gameObject, sound.voice[6]);
        }

        public void PlayWind()
        {

            var sound = MCSoundManager.Instance.objectSound.cinema;
            sound.PlaySound(this.gameObject, sound.storyWind);
        }

        public void CineEnd()
        {
            playableDirector.gameObject.SetActive(false);
            uiTitle.NextScene();
        }

        public void Update()
        {
            if (Input.anyKeyDown && !cinePlay)
            {
                CineNext();
                cinePlay = true;
            }
        }


    }
}