using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

using MC.Sound;
using MC.UI;

public class Credit : MonoBehaviour
{
    public AkBank bgmBank;
    public AkBank sfxBank;
    public AK.Wwise.Event bgm;
    public AK.Wwise.Event pangyi;

    public PlayableDirector pd;

    public GameObject clear;
    public GameObject notClear;


    // Start is called before the first frame update
    void Start()
    {
        GameStatus.SetCurrentGameState(CurrentGameState.End);
        CanvasInfo.Instance.enemyHP.SetFalse();
        bgmBank.HandleEvent(gameObject);
        sfxBank.HandleEvent(gameObject);

        var sound = MCSoundManager.Instance.objectSound;
        MCSoundManager.ChangeBGM(sound.bgm.tutoBGM);

        if (GameStatus.GameClear) {
            clear.SetActive(true);
            pd.Play();
        }
        else
        {
            notClear.SetActive(true);
        }

        GameManager.SetFadeInOut(() => { }, 1f, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
