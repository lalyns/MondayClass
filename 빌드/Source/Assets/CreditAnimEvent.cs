using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MC.Sound;


public class CreditAnimEvent : MonoBehaviour
{
    public Credit credit;

    public void PangYi()
    {
        credit.pangyi.Post(gameObject);
    }

    public void EndCredit()
    {
        MCSoundManager.StopBGM();
        SceneManager.LoadScene(MC.SceneDirector.MCSceneManager.TITLE);
        GameStatus.GameClear = false;
    }
}
