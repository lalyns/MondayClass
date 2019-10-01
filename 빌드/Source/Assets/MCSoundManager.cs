using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;
using MC.SceneDirector;

public class MCSoundManager : MonoBehaviour
{
    public static MCSoundManager Instance;

    public ObjectSound objectSound;

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

    public static void SetSound()
    {
        Debug.Log("Music Start");
        Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
            Instance.objectSound.ambient.stageAmbient);
        Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
            Instance.objectSound.bgm.stageBGM);
    }

}
