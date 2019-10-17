using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Sound;
using MC.Mission;
using MC.SceneDirector;

public enum CurrentGameState
{
    Loading,
    Start,
    Select,
    Wait,
    Dialog,
    MissionClear,
    Dead,
    Tutorial,
}

public class GameStatus : MonoBehaviour
{
    public static GameStatus _Instance;
    public static GameStatus Instance {
        get {
            if (_Instance == null)
            {
                _Instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
            }
            return _Instance;
        }
        set {
            _Instance = value;
        }
    }

    public static bool _EditorMode = false;

    public float _LimitTime = 180;
    public bool _MissionStatus = false;

    public bool usingKeward = false;

    public GameObject _DummyLocationEffect;

    public PlayerFSMManager _PlayerInstance;

    bool dummySet = false;

    bool isPause = false;

    public static CurrentGameState currentGameState = CurrentGameState.Start;

    float timer = 0;

    public void Awake()
    {
        //_PlayerInstance = PlayerFSMManager.Instance;
    }

    public void Start()
    {
        if(_Instance == null)
        {
            Instance = GetComponent<GameStatus>();
        }
        else
        {
            return;
        }
    }

    // 게임 메뉴 정보

    // 던전 정보
    /// <summary>
    /// 활동중인 몬스터 리스트
    /// </summary>
    private List<GameObject> _ActivedMonsterList = new List<GameObject>();
    public List<GameObject> ActivedMonsterList {
        get {
            return _ActivedMonsterList;
        }
        internal set { _ActivedMonsterList = value; }
    }

    private int _StageLevel = 0;
    public int StageLevel {
        get { return _StageLevel; }
        internal set { _StageLevel = value; }
    }

    /// <summary>
    /// 활동중인 몬스터 객체 추가 메소드, 항상 추가 해줄것.
    /// </summary>
    /// <param name="monster"></param>
    public void AddActivedMonsterList(GameObject monster)
    {
        // 해당객체가 몬스터인지 판별할것

        ActivedMonsterList.Add(monster);

    }

    /// <summary>
    /// 활동중인 몬스터 객체 리스트에서 몬스터 삭제 메소드
    /// </summary>
    /// <param name="monster"></param>
    public void RemoveActivedMonsterList(GameObject monster)
    {
        if (_ActivedMonsterList.Contains(monster))
        {
            ActivedMonsterList.Remove(monster);
        }
    }

    /// <summary>
    /// 현재 활동중인 몬스터 전부를 풀로 반환시키는 매소드
    /// </summary>
    public void RemoveAllActiveMonster()
    {
        // 몬스터 타입에따라서
        // 풀로 반환한다.
        List<GameObject> active = ActivedMonsterList;
        //ActivedMonsterList.Clear();

        foreach(GameObject mob in active)
        {
            MonsterType type = mob.GetComponent<FSMManager>().monsterType;

            switch (type)
            {
                case MonsterType.RedHat:
                    MonsterPoolManager._Instance._RedHat.ItemReturnPool(mob);
                    break;
                case MonsterType.Mac:
                    MonsterPoolManager._Instance._Mac.ItemReturnPool(mob);
                    break;
                case MonsterType.Tiber:
                    MonsterPoolManager._Instance._Tiber.ItemReturnPool(mob);
                    break;
            }
        }

        if(MissionManager.Instance.CurrentMissionType == MissionType.Annihilation)
        {
            MissionA missionA = MissionManager.Instance.CurrentMission as MissionA;

            if(missionA.currentWave < missionA.totalWave)
                missionA.Invoke("MonsterCheck", 5f);
            else if(missionA.currentWave == missionA.totalWave)
                missionA.ClearMission();
        }
            

        ActivedMonsterList.Clear();
    }

    public void Update()
    {
        // 유니티 에디터에서 작동하는 에디터 기능
        if (Input.GetKey(KeyCode.LeftAlt) /*&& currentGameState == CurrentGameState.Start*/)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SummonReady();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                usingKeward = true;
                RemoveAllActiveMonster();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                MissionManager.Instance.CurrentMission.ClearMission();
                MissionManager.Instance.CurrentMission.missionEnd = true;
            }

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                MCSceneManager.Instance.NextScene(MCSceneManager.ANNIHILATION);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MCSceneManager.Instance.NextScene(MCSceneManager.SURVIVAL);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                MCSceneManager.Instance.NextScene(MCSceneManager.DEFENCE);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                GameManager.Instance.OnInspectating = !GameManager.Instance.OnInspectating;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerFSMManager.Instance.SpecialGauge = 100.0f;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {

                UserInterface.SetPlayerUserInterface(false);
                MCSceneManager.Instance.NextScene(MCSceneManager.BOSS);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                MCSoundManager.SetSound();
            }

            //if(Input.GetKeyDown(KeyCode.Mouse0) ||
            //    Input.GetKeyDown(KeyCode.Space))
            //{


            //    UserInterface.Instance.Dialog.SetDialog(UserInterface.Instance.Dialog.dialog.dialog[dialogNum++]);

            //    if(dialogNum >= 3)
            //    {
            //        dialogNum = 0;
            //    }
            //}
        }

        timer += Time.deltaTime;
        if(timer >= 3f)
        {
            GameStatusCheck();
            timer = 0;
        }

        if(currentGameState == CurrentGameState.Dialog)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            {
                UserInterface.Instance.Dialog.NextDialog();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) &&
            MCSceneManager.currentScene != MCSceneManager.TITLE &&
            currentGameState != CurrentGameState.Loading &&
            currentGameState != CurrentGameState.Dialog)
        {
            isPause = !isPause;
            CanvasInfo.PauseMenuActive(isPause);
        }

        if (UserInterface.Instance.ClearMission.gameObject.activeSelf &&
            (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Mouse0)))
        {

            UserInterface.Instance.ClearMission.gameObject.SetActive(false);
            currentGameState = CurrentGameState.Wait;

            PlayerFSMManager.Instance.GetComponent<PlayerCLEAR>().CMSet.gameObject.SetActive(false);
            PlayerFSMManager.Instance.SetState(PlayerState.IDLE);
            PlayerFSMManager.Instance.mainCamera.gameObject.SetActive(true);
        }

        if (dummySet)
        {
            SummonEffect();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SummonMonster();
            }
        }

#if UNITY_STANDALONE

#endif
        if (currentGameState == CurrentGameState.Start &&
            MissionManager.Instance.CurrentMission.MissionOperate && 
            !MissionManager.Instance.CurrentMission.missionEnd)
        {
            _LimitTime -= Time.deltaTime;
        }
    }

    void GameStatusCheck()
    {

        Debug.Log(GameStatus.currentGameState.ToString());
    }

    public static void SetCurrentGameState(CurrentGameState state)
    {
        currentGameState = state;
    }

    public void SummonReady()
    {
        //Debug.Log("지정소환준비");
        dummySet = true;
        _EditorMode = true;
        _DummyLocationEffect.SetActive(true);
        UserInterface.SetPointerMode(true);
    }

    public void SummonEffect()
    {
        Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z);
        Vector3 cameraForward = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));

        Ray ray = Camera.main.ScreenPointToRay(mousePoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, 1 << 17))
        {
            _DummyLocationEffect.transform.position = hit.point;
            _DummyLocationEffect.transform.position += new Vector3(0, 0.1f, 0);
        }
    }

    // 몬스터 지정소환
    public void SummonMonster()
    {
        MonsterPoolManager._Instance._Mac.ItemSetActive(_DummyLocationEffect.transform, "monster");
        dummySet = false;
        _DummyLocationEffect.SetActive(false);
        UserInterface.SetPointerMode(false);
    }

    public void SetValue()
    {
        _PlayerInstance = PlayerFSMManager.Instance;
    }
}
