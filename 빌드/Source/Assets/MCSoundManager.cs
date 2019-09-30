using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;

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
        SetSound();
    }

    public static void SetSound()
    {
        Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
            Instance.objectSound.ambient.stageAmbient);
        Instance.objectSound.ambient.PlayAmbient(Instance.gameObject,
            Instance.objectSound.bgm.stageBGM);
    }

}
