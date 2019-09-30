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



}
