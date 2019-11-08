using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CreditAnimEvent : MonoBehaviour
{
    public Credit credit;

    public void PangYi()
    {
        credit.pangyi.Post(gameObject);
    }

    public void EndCredit()
    {
        credit.bgm.Stop(gameObject);
        SceneManager.LoadScene(MC.SceneDirector.MCSceneManager.TITLE);
        GameStatus.GameClear = false;
    }
}
