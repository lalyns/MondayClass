using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int curScore = 0;
    public bool IsPuase;

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
        if(MCSceneManager.currentSceneNumber == 0)
        {
            uIActive.player = false;
            uIActive.progress = false;
            uIActive.selector = false;
            UserInterface.SetPointerMode(true);
            UserInterface.SetTitleUI(true);
        }

        if (MCSceneManager.currentSceneNumber == 1 ||
            MCSceneManager.currentSceneNumber == 2)
        {
            UserInterface.SetPointerMode(false);
            UserInterface.SetTitleUI(false);
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
            UserInterface.Instance.MousePointerSpeed(1 / TimeMagnificationValue);
        }
        else
        {
            Time.timeScale = 0.01f;
            UserInterface.Instance.MousePointerSpeed(100f);
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

    public static void TempScoreAdd()
    {
        Instance.curScore += 1;
    }

    public static void SetFadeInOut(System.Action callback,  bool value)
    {
        if (value)
            Instance.StartCoroutine(UserInterface.FadeIn(callback, 20));
        else
            Instance.StartCoroutine(UserInterface.FadeOut(callback, 20));
    }

    public static void SetSceneSetting()
    {
        var num = MCSceneManager.currentSceneNumber;
        switch (num)
        {
            case 0:
                break;
            case 1:
                Debug.Log("aa");
                Instance.Scene1Setting();
                break;
            case 2:
                break;
        }
    }

    private void Scene1Setting()
    {
        CanvasInfo.Instance.SetRenderCam();

        UserInterface.Instance.SetValue();
        UserInterface.SetPointerMode(false);
        UserInterface.SetTitleUI(false);
        UserInterface.SetAllUserInterface(true);
        UserInterface.SetPlayerUserInterface(true);

        MissionManager.Instance.SetValue();
        GameStatus.Instance.SetValue();
    }
}
