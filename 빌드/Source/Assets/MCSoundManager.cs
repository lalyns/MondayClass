using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;
using MC.SceneDirector;
<<<<<<< HEAD
=======
using AK.Wwise;
>>>>>>> b8bac9f6b5d1ca776ada11fa24994691a298c627

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
<<<<<<< HEAD
=======
    }

    public static void LoadBank()
    {
        Instance.Sound.HandleEvent(Instance.gameObject);
        Instance.Bgm.HandleEvent(Instance.gameObject);
        Instance.Ambient.HandleEvent(Instance.gameObject);
>>>>>>> b8bac9f6b5d1ca776ada11fa24994691a298c627
    }

    public static void SetSound()
    {
<<<<<<< HEAD
        Debug.Log("Music Start");
=======
        Debug.Log("SoundStart");

>>>>>>> b8bac9f6b5d1ca776ada11fa24994691a298c627
        Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
            Instance.objectSound.ambient.stageAmbient);
        Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
            Instance.objectSound.bgm.stageBGM);
    }

}
