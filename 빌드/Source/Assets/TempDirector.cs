using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using MC.UI;

public class TempDirector : MonoBehaviour
{
    public static TempDirector Instance;

    public bool PlayMode = false;

    public GameObject CineSet;
    public GameObject PlaySet;

    public PlayableDirector director;

    private void Awake()
    {
        Instance = GetComponent<TempDirector>();
        director = GetComponent<PlayableDirector>();
    }

    void Start()
    {
        if (PlayMode) SceneStart();
        else CineStart();
    }

    public void CineStart()
    {
        CineSet.SetActive(true);
        PlaySet.SetActive(false);

        director.Play();
    }

    public void SceneStart()
    {
        CineSet.SetActive(false);
        PlaySet.SetActive(true);


        UserInterface.SetPlayerUserInterface(true);
    }

    public void OnStartBoss()
    {

    }
}
