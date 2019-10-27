using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MC.UI;

public class Credit : MonoBehaviour
{
    public AkBank bgmBank;
    public AkBank sfxBank;
    public AK.Wwise.Event bgm;
    public AK.Wwise.Event pangyi;

    // Start is called before the first frame update
    void Start()
    {
        bgmBank.HandleEvent(gameObject);
        sfxBank.HandleEvent(gameObject);
        bgm.Post(gameObject);

        GameManager.SetFadeInOut(() => { }, 1f, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PangYi()
    {
        pangyi.Post(gameObject);
    }

    public void EndCredit()
    {
        bgm.Stop(gameObject);
        SceneManager.LoadScene(MC.SceneDirector.MCSceneManager.TITLE);
    }
}
