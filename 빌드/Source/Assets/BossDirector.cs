using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using MC.UI;
using MC.SceneDirector;
using MC.Sound;

public class BossDirector : MonoBehaviour
{
    public static BossDirector Instance;

    public bool PlayMode = false;

    public GameObject PlaySet;

    public GameObject cam;

    public GameObject startCine;
    public GameObject phaseChangeCine;
    
    public GameObject deadCine;
    
    public PlayableDirector startDirector;
    public PlayableDirector phaseChangeDIrector;
    public PlayableDirector deadDirector;

    private void Awake()
    {
        Instance = GetComponent<BossDirector>();
    }

    void Start()
    {
    }


    public void PlayStartCine()
    {
        GameStatus.SetCurrentGameState(CurrentGameState.Product);
        cam.SetActive(true);
        startCine.SetActive(true);
        PlaySet.SetActive(false);

        startDirector.Play();
    }

    public void PlayPhaseChangeCine()
    {
        GameStatus.SetCurrentGameState(CurrentGameState.Product);
        cam.SetActive(true);
        phaseChangeCine.SetActive(true);
        PlaySet.SetActive(false);

        phaseChangeDIrector.Play();
    }

    public void PlayDeadCine()
    {
        GameStatus.SetCurrentGameState(CurrentGameState.End);
        cam.SetActive(true);
        deadCine.SetActive(true);
        PlaySet.SetActive(false);

        deadDirector.Play();
    }

    public void PlayScene()
    {
        GameStatus.SetCurrentGameState(CurrentGameState.Start);
        cam.SetActive(false);
        startCine.SetActive(false);
        phaseChangeCine.SetActive(false);
        deadCine.SetActive(false);
        PlaySet.SetActive(true);

        GameManager.Instance.CharacterControl = true;
        UserInterface.SetPlayerUserInterface(true);
        UserInterface.SetPointerMode(false);

        CamLayerReset();
    }

    public void CamLayerReset()
    {
        CanvasInfo.Instance.SetRenderCam();
        CanvasInfo.Instance.Layers[1].planeDistance = 10f;
        GameManager.SetFadeInOut(
            () => { }, 1f, true);
    }

    public void DeadEnd()
    {

    }

    public void CineEnd()
    {

    }

    public void OnStartBoss()
    {

    }
}
