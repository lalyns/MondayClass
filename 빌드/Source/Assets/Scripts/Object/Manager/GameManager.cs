using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    private static GameManager _Instance;
    public static GameManager Instance {
        get {
            if(_Instance == null)
            {
                _Instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            }
            return _Instance;
        }
        set {
            _Instance = value;
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


    private void Awake()
    {
        if(_Instance == null)
        {
            _Instance = GetComponent<GameManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (_EditorCursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
        if (!IsPuase) Time.timeScale = TimeMagnificationMode ? TimeMagnificationValue : 1.0f;
        else Time.timeScale = 0;

        // 키입력 매니저로 이동할것
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _SimpleMode = !_SimpleMode;
            UserInterface.SetMPMode(_SimpleMode);
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

    public void SetFadeInOut(bool value)
    {
        if (value)
            StartCoroutine(UserInterface.FadeIn(() =>
            {
                CharacterControl = true;
            }
            , 20));
        else
            StartCoroutine(UserInterface.FadeOut(() =>
            {
                MissionManager.EnterMission();
            }
            , 20));
    }
}
