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

    public bool _ActiveAllUI;
    public bool _ActivePlayerUI;
    public bool _ActiveMonsterUI;
    public bool _ActiveSystemUI;

    public bool TimeMagnificationMode;
    [Range(0,5)] public float TimeMagnificationValue;

    public bool CharacterControl = true;

    public bool ControlManual;

    bool _SimpleMode = false;
    public GameObject _MissionSimple;
    public GameObject _MissionFull;
    public Image _cursurImage;

    public int curScore = 0;
    public bool IsPuase;

    public Image FadeInOutPanel;
    public float fadeInOutSpeed = 10.0f;
    public Animator FadeInOutAnim;

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

    }



    // Update is called once per frame
    void Update()
    {
        if (!IsPuase) Time.timeScale = TimeMagnificationMode ? TimeMagnificationValue : 1.0f;
        else Time.timeScale = 0;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!_SimpleMode)
            {
                _MissionSimple.SetActive(true);
                _MissionFull.SetActive(false);
                _SimpleMode = !_SimpleMode;
            }
            else
            {
                _MissionSimple.SetActive(false);
                _MissionFull.SetActive(true);
                _SimpleMode = !_SimpleMode;
            }
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

    /// <summary>
    /// 마우스 커서가 보이게 하는 매소드
    /// </summary>
    /// <param name="isLock"></param>
    public static void CursorMode(bool isLock)
    {
        if (Instance._EditorCursorLock)
        {
            if (isLock)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                try
                {
                    Instance._cursurImage.enabled = true;
                }
                catch
                {

                }


            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                try
                {
                    Instance._cursurImage.enabled = false;
                }
                catch
                {

                }
            }
        }
    }

    public static void TempScoreAdd()
    {
        Instance.curScore += 1;
    }


    public void SetFadeInOut(bool value)
    {
        if (value)
            StartCoroutine(FadeIn(fadeInOutSpeed));
        else
            StartCoroutine(FadeOut(fadeInOutSpeed));
    }

    public IEnumerator FadeOut(float speed)
    {
        float i = 0;
        int callWrite = 0;

        Color temp = FadeInOutPanel.color;
        for(i=0; i<= 100; )
        {
            i += speed;
            temp.a = i / 100f;
            FadeInOutPanel.color = temp;

            yield return new WaitForSeconds(0.15f);
        }
        MissionManager.EnterMission();
    }

    public IEnumerator FadeIn(float speed)
    {
        float i = 100;

        Color tempColor = FadeInOutPanel.color;
        for (i = 100; i >= 0; )
        {
            i -= speed;
            tempColor.a = i / 100f;
            FadeInOutPanel.color = tempColor;
            
            yield return new WaitForSeconds(0.15f);
        }
        //FadeInOutAnim.Play("FadeInOut");
        CharacterControl = true;

    }
}
