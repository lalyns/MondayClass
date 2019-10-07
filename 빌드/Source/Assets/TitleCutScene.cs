using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.SceneDirector;
using UnityEngine.UI;

public class TitleCutScene : MonoBehaviour
{
    public Sprite[] cutSceneSprite;
    public Image cutScene;

    public int currentCutSceneNumber = 0;

    public void CutScene()
    {
        if(currentCutSceneNumber == cutSceneSprite.Length)
        {
            MCSceneManager.Instance.NextScene(MCSceneManager.ANNIHILATION);
        }
        else
        {
            cutScene.sprite = cutSceneSprite[currentCutSceneNumber++];
        }
    }
    
}
