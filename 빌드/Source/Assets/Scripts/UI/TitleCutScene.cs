using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.SceneDirector;
using UnityEngine.UI;
using UnityEngine.Playables;

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