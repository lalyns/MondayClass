using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

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
        public bool selector = true;
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

            MCSoundManager.Instance.objectSound.ambient.PlayAmbient(this.gameObject,
                MCSoundManager.Instance.objectSound.ambient.lobbyAmbient);
        }

        if (MCSceneManager.currentScene == MCSceneManager.ANNIHILATION ||
            MCSceneManager.currentScene == MCSceneManager.SURVIVAL     ||
            MCSceneManager.currentScene == MCSceneManager.DEFENCE)
        {
            UserInterface.SetPointerMode(false);
        }

        if(MCSceneManager.currentScene == MCSceneManager.BOSS)
        {
            TempDirector.Instance.SceneStart();
            UserInterface.SetPointerMode(false);
        }

        UserInterface.SetAllUserInterface(uIActive.all);
        UserInterface.SetPlayerUserInterface(uIActive.player);
        UserInterface.SetSystemInterface(uIActive.system);
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

    public static void SetFadeInOut(System.Action callback,  bool value)
    {
        if (value)
            Instance.StartCoroutine(UserInterface.FadeIn(callback, 20));
        else
            Instance.StartCoroutine(UserInterface.FadeOut(callback, 20));
    }

    public static void SetSceneSetting()
    {
        MissionButton.isPush = false;
        CanvasInfo.Instance.SetRenderCam();
        UserInterface.Instance.SetValue();

        Instance.Invoke("ScriptCheck", 5f);
    }

    private void ScriptCheck()
    {
        if (MCSceneManager.currentScene != MCSceneManager.TITLE)
        {
            if (GameStatus.Instance.StageLevel == 0)
            {
                var dialogEvent = GetComponent<DialogEvent>();
                UserInterface.DialogSetActive(true);
                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[4]);
                GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
                return;
            }

            if (MCSceneManager.currentScene == MCSceneManager.BOSS)
            {
                var dialogEvent = GetComponent<DialogEvent>();
                UserInterface.DialogSetActive(true);
                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[7]);
                GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
                return;
            }
        }

        AfterDialog();
    }

    public void AfterDialog()
    {
        var num = MCSceneManager.currentScene;
        GameStatus.SetCurrentGameState(CurrentGameState.Wait);

        switch (num)
        {
            case 0:
                Instance.TitleSet();
                break;
            case 1:
                break;
            case 2:
                Instance.StageSet();
                break;
            case 3:
                Instance.StageSet();
                break;
            case 4:
                Instance.StageSet();
                break;
            case 5:
                Instance.BossSet();
                break;
        }
    }

    public void TitleSet()
    {
        UserInterface.SetPlayerUserInterface(false);

        UserInterface.SetMissionSelectionUI(false);
        UserInterface.SetMissionProgressUserInterface(false);


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
        CanvasInfo.Instance.PlayStartAnim();

        UserInterface.SetPointerMode(false);

        UserInterface.SetAllUserInterface(true);
        UserInterface.SetPlayerUserInterface(true);

        CharacterControl = true;
    }

    public void SetBank()
    {
        MCSoundManager.SetSound();
    }

    public void BossSet()
    {
        UserInterface.SetPointerMode(false);

        if (GameManager.Instance.CineMode)
        {
            TempDirector.Instance.PlayMode = false;
            TempDirector.Instance.CineStart();
        }
        else
        {
            TempDirector.Instance.PlayMode = true;
            TempDirector.Instance.SceneStart();
        }
    }

    private void BossSceneSetting()
    {
        CinemaManager.Instance.BossDirector = GameObject.FindGameObjectWithTag("Director").GetComponent<PlayableDirector>();
        CinemaManager.Instance.BossDirector.Play();
    }
}
