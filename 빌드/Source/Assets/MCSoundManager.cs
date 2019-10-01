using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;
using MC.SceneDirector;
using AK.Wwise;

public class MCSoundManager : MonoBehaviour
{
    public static MCSoundManager Instance;

    public ObjectSound objectSound;
    public AkBank Sound;
    public AkBank Bgm;
    public AkBank Ambient;


    private void Awake()
    {
        if (Instance == null)
            Instance = GetComponent<MCSoundManager>();
    }

    public void Start()
    {
        if(MCSceneManager.currentSceneNumber != 0)
            SetSound();
    }

    public static void LoadBank()
    {
        Instance.Sound.HandleEvent(Instance.gameObject);
        Instance.Bgm.HandleEvent(Instance.gameObject);
        Instance.Ambient.HandleEvent(Instance.gameObject);
    }

    public static void SetSound()
    {
        Debug.Log("SoundStart");

        Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
            Instance.objectSound.ambient.stageAmbient);
        Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
            Instance.objectSound.bgm.stageBGM);
    }

}
