using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.SceneDirector;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TitleCutScene : MonoBehaviour
{
    public Sprite[] cutSceneSprite;
    public Image cutScene;

    public int currentCutSceneNumber = 0;

    bool nextScene = true;

    public void CutScene()
    {
        if(currentCutSceneNumber == cutSceneSprite.Length)
        {
            if (nextScene)
            {
                MCSceneManager.Instance.NextScene(MCSceneManager.ANNIHILATION);
                nextScene = false;
            }
        }
        else
        {
            cutScene.sprite = cutSceneSprite[currentCutSceneNumber++];
        }
    }

    public PlayableDirector playableDirector;

    public void CineStop()
    {
        playableDirector.Pause();
    }

    public void Update()
    {
        if(Input.anyKeyDown)
        {
            CineNext();
        }
    }

    public void CineNext()
    {
        playableDirector.Resume();
    }

}
