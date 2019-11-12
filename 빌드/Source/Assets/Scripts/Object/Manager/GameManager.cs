﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

using MC.Mission;
using MC.UI;
using MC.Sound;
using MC.SceneDirector;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {
        get {
            if(instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            }
            return instance;
        }
        set {
            instance = value;
        }
    }

    public bool _EditorCursorLock = true;

    public bool TimeMagnificationMode;
    [Range(0,5)] public float TimeMagnificationValue;

    public bool CharacterControl = true;

    public bool ControlManual;

    bool _SimpleMode = false;

    public bool IsPuase;

    public bool CineMode;

    public float softDuration = 0.05f;
    public float hardDuration = 0.06f;

    [System.Serializable]
    public class UIActive
    {
        public bool all = true;
        public bool player = true;
        public bool monster = true;
        public bool system = true;
        public bool progress = false;
        public bool selector = false;
    }
    public UIActive uIActive;

    [System.Serializable]
    public class GameConfig
    {
        public SoundActive soundActive;
    }
    public GameConfig config;

    private void Awake()
    {
        if(instance == null)
        {
            instance = GetComponent<GameManager>();
        }

        if (instance.gameObject != this.gameObject)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        if(MCSceneManager.currentScene == MCSceneManager.TITLE)
        {
            UserInterface.SetPointerMode(true);
        }

        if (MCSceneManager.currentScene == MCSceneManager.ANNIHILATION ||
            MCSceneManager.currentScene == MCSceneManager.SURVIVAL     ||
            MCSceneManager.currentScene == MCSceneManager.DEFENCE)
        {
            UserInterface.SetPointerMode(false);
        }

        if(MCSceneManager.currentScene == MCSceneManager.BOSS)
        {
            if (CineMode)
            {
                BossDirector.Instance.PlayStartCine();
            }
            else
            {
                BossDirector.Instance.PlayScene();
                UserInterface.SetPointerMode(false);
            }
        }

        UserInterface.SetAllUserInterface(uIActive.all);
        UserInterface.SetPlayerUserInterface(uIActive.player);
        //UserInterface.SetSystemInterface(uIActive.system);
        UserInterface.SetMissionProgressUserInterface(uIActive.progress);
        UserInterface.SetMissionSelectionUI(uIActive.selector);
    }

    // Update is called once per frame
    void Update()
    {
        GameSpeed(IsPuase);

        // 키입력 매니저로 이동할것
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _SimpleMode = !_SimpleMode;
            UserInterface.SetMPMode(_SimpleMode);
        }
    }

    void GameSpeed(bool isPause)
    {
        if (!isPause)
        {
            Time.timeScale = TimeMagnificationMode ? TimeMagnificationValue : 1.0f;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    #region OnGUI 도구
    /// <summary>
    /// GUI 변수 및 매소드
    /// </summary>

    [Range(0, 1)] public float WidthRatio;
    [Range(0, 1)] public float HeightRatio;
    [HideInInspector] public bool OnInspectating;

    private void OnGUI()
    {
        if (OnInspectating) ViewDungeonStatus();
        if (ControlManual) ViewControlManual();

    }

    private void ViewDungeonStatus()
    {
        string discribe = "현재 던전 : " + MissionManager.Instance.CurrentMission.gameObject.name.ToString() +
                          "\n레벨 : " + GameStatus.Instance.StageLevel +
                          "\n 미션 정보 : " + MissionManager.Instance.CurrentMission.MissionOperate;

        if (GUI.Button(new Rect(Screen.width * WidthRatio, Screen.height * HeightRatio, 150, 100),discribe)) { }

    }

    private void ViewControlManual()
    {
        string discribe =
            "조작법 \n" +
            "이동 : W A S D \n" +
            "공격 : 마우스 좌클릭 \n" +
            "회피 : LeftShift \n" +
            "스킬 : F";

        if (GUI.RepeatButton(new Rect(Screen.width / 100f * 80f, Screen.height * 0.8f, 230, 85), discribe)) { }
    }

    #endregion

    public static void SetFadeInOut(System.Action callback, float duration, bool value)
    {
        if (value)
            Instance.StartCoroutine(UserInterface.FadeIn(callback, duration));
        else
            Instance.StartCoroutine(UserInterface.FadeOut(callback, duration));
    }

    public static void SetSceneSetting()
    {
        MissionButton.isPush = false;
        CanvasInfo.Instance.SetRenderCam();
        UserInterface.Instance.SetValue();

        PlayerFSMManager.Instance.Stat.SetStatValue();
    }

    public static void ScriptCheck()
    {
        if (MCSceneManager.currentScene != MCSceneManager.TITLE || 
            MCSceneManager.currentScene != MCSceneManager.TUTORIAL)
        {
            
        }

        Instance.AfterDialog();
    }

    public void AfterDialog()
    {
        var num = MCSceneManager.currentScene;

        switch (num)
        {
            case MCSceneManager.TITLE:
                Instance.TitleSet();
                break;
            case MCSceneManager.TUTORIAL:
                Instance.TutorialSet();
                break;
            case MCSceneManager.ANNIHILATION:
                GameStatus.SetCurrentGameState(CurrentGameState.Wait);
                Instance.StageSet();
                break;
            case MCSceneManager.SURVIVAL:
                GameStatus.SetCurrentGameState(CurrentGameState.Wait);
                Instance.StageSet();
                break;
            case MCSceneManager.DEFENCE:
                GameStatus.SetCurrentGameState(CurrentGameState.Wait);
                Instance.StageSet();
                break;
            case MCSceneManager.BOSS:
                GameStatus.SetCurrentGameState(CurrentGameState.Wait);
                Instance.BossSet();
                break;
        }
    }

    public void TutorialSet()
    {
        GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
        FindObjectOfType<MissionTutorial>().tutostart = true;


        UserInterface.SetPlayerUserInterface(true);
        UserInterface.SetPointerMode(false);
    }

    public void TitleSet()
    {
        UserInterface.SetPlayerUserInterface(false);

        UserInterface.SetMissionSelectionUI(false);
        UserInterface.SetMissionProgressUserInterface(false);

        UserInterface.SetPointerMode(true);

        MCSoundManager.Instance.objectSound.bgm.StopBGM(gameObject,
            MCSoundManager.Instance.objectSound.bgm.stageBGM);
        MCSoundManager.Instance.objectSound.bgm.StopBGM(gameObject,
            MCSoundManager.Instance.objectSound.bgm.bossBGM);

        MCSoundManager.Instance.objectSound.bgm.PlayBGM(gameObject,
            MCSoundManager.Instance.objectSound.bgm.lobbyBGM);

        CharacterControl = false;
    }

    public void StageSet()
    {
        GameStatus.SetCurrentGameState(CurrentGameState.Wait);

        UserInterface.SetPointerMode(false);

        UserInterface.SetAllUserInterface(true);
        UserInterface.SetPlayerUserInterface(true);

        CharacterControl = true;
    }

    public void BossSet()
    {
        GameStatus.SetCurrentGameState(CurrentGameState.Wait);
        UserInterface.SetPointerMode(false);

        if (GameManager.Instance.CineMode)
        {
            BossDirector.Instance.PlayMode = false;
            BossDirector.Instance.PlayStartCine();
        }
        else
        {
            BossDirector.Instance.PlayMode = true;
            BossDirector.Instance.PlayScene();
        }
    }

    private void BossSceneSetting()
    {
        CinemaManager.Instance.BossDirector = GameObject.FindGameObjectWithTag("Director").GetComponent<PlayableDirector>();
        CinemaManager.Instance.BossDirector.Play();
    }
}
