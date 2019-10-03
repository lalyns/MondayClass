using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class TempDirector : MonoBehaviour
{
    public static TempDirector Instance;

    public GameObject CineSet;
    public GameObject PlaySet;

    private void Awake()
    {
        Instance = GetComponent<TempDirector>();
    }

    void Start()
    {

    }

    public void SceneStart()
    {
        CineSet.SetActive(false);
        PlaySet.SetActive(true);

        UserInterface.SetPlayerUserInterface(true);
    }
}
