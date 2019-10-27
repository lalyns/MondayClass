﻿using System.Collections;
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
    Pause,
    MissionClear,
    Dead,
    Tutorial,
    Product,
    EDITOR,
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
    public bool canInput = true;

    public GameObject _DummyLocationEffect;

    public PlayerFSMManager _PlayerInstance;

    bool dummySet = false;

    bool isPause = false;

    public static CurrentGameState currentGameState = CurrentGameState.Start;
    public static CurrentGameState prevState = CurrentGameState.Start;

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

    MonsterType summonType;
    public void Update()
    {
        if (!canInput) return;
        //if (Time.timeScale == 0 && Input.anyKey) return;
        
        // 유니티 에디터에서 작동하는 에디터 기능
        if (Input.GetKey(KeyCode.LeftAlt) /*&& currentGameState == CurrentGameState.Start*/)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SummonReady(MonsterType.RedHat);
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                SummonReady(MonsterType.Mac);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SummonReady(MonsterType.Tiber);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                usingKeward = true;
                RemoveAllActiveMonster();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                PlayerFSMManager.Instance.CurrentClear = Random.Range((int)0, (int)2);
                PlayerFSMManager.Instance.SetState(PlayerState.CLEAR);
                MissionManager.Instance.CurrentMission.ClearMission();
                MissionManager.Instance.CurrentMission.missionEnd = true;
            }

            if(Input.GetKeyDown(KeyCode.U))
            {
                MCSceneManager.Instance.NextScene(MCSceneManager.ANNIHILATION, 1f, true);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                MCSceneManager.Instance.NextScene(MCSceneManager.SURVIVAL, 1f, true);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                MCSceneManager.Instance.NextScene(MCSceneManager.DEFENCE, 1f, true);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                MCSceneManager.Instance.NextScene(MCSceneManager.BOSS, 1f, true);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GameManager.Instance.OnInspectating = !GameManager.Instance.OnInspectating;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerFSMManager.Instance.SpecialGauge = 100.0f;
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                MissionManager.ExitMission();
                MissionManager.PopUpMission();
            }

            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    MCSoundManager.SetSound();
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Press");
            if (
            MCSceneManager.currentScene != MCSceneManager.TITLE &&
            currentGameState != CurrentGameState.Loading &&
            currentGameState != CurrentGameState.Dialog &&
            currentGameState != CurrentGameState.Product && 
            currentGameState != CurrentGameState.MissionClear)
            {
                isPause = !isPause;
                Debug.Log("Press Escape : " + isPause);
                CanvasInfo.PauseMenuActive(isPause);
                GameManager.Instance.IsPuase = isPause;
                UserInterface.BlurSet(isPause, 10f);
                SetCurrentGameState(isPause ? CurrentGameState.Pause : prevState);
            }

            if(currentGameState == CurrentGameState.Product)
            {
                if (MCSceneManager.currentScene == MCSceneManager.BOSS)
                {
                    BossDirector.Instance.PlayScene();
                }
                if (MCSceneManager.currentScene == MCSceneManager.TITLE)
                {
                    FindObjectOfType<TitleCutScene>().CineEnd();
                }
            }
        }


        if (GameStatus.currentGameState != CurrentGameState.Product)
        {
            if (UserInterface.Instance.ClearMission.gameObject.activeSelf &&
                (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Mouse0)))
            {

                UserInterface.Instance.ClearMission.gameObject.SetActive(false);
                currentGameState = CurrentGameState.Wait;

                PlayerFSMManager.Instance.GetComponent<PlayerCLEAR>().CMSet.gameObject.SetActive(false);
                PlayerFSMManager.Instance.SetState(PlayerState.IDLE);
                PlayerFSMManager.Instance.mainCamera.gameObject.SetActive(true);

                Invoke("DialogCheck", 0.5f);

            }
        }

        if (dummySet)
        {
            SummonEffect();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SummonMonster(summonType);
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

    void DialogCheck()
    {
        if (MCSceneManager.currentScene == MCSceneManager.ANNIHILATION ||
            MCSceneManager.currentScene == MCSceneManager.DEFENCE ||
            MCSceneManager.currentScene == MCSceneManager.SURVIVAL)
        {
            if (GameStatus.Instance.StageLevel == 3)
            {
                GameStatus.SetCurrentGameState(CurrentGameState.Dialog);

                var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();
                UserInterface.DialogSetActive(true);

                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[5], () =>
                {
                    GameStatus.SetCurrentGameState(CurrentGameState.Wait);
                    GameManager.Instance.CharacterControl = true;
                });
            }

            if (GameStatus.Instance.StageLevel == 8)
            {
                GameStatus.SetCurrentGameState(CurrentGameState.Dialog);

                var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();
                UserInterface.DialogSetActive(true);
                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[6], () =>
                {
                    GameStatus.SetCurrentGameState(CurrentGameState.Wait);
                    GameManager.Instance.CharacterControl = true;
                });
            }
        }
    }

    void GameStatusCheck()
    {

        Debug.Log(GameStatus.currentGameState.ToString());
    }

    public static void SetCurrentGameState(CurrentGameState state)
    {
        prevState = currentGameState;
        currentGameState = state;
    }

    public void SummonReady(MonsterType type)
    {
        //Debug.Log("지정소환준비");
        summonType = type;
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
    public void SummonMonster(MonsterType type)
    {
        switch(type)
        {
            case MonsterType.RedHat:
                MonsterPoolManager._Instance._RedHat.ItemSetActive(
                    _DummyLocationEffect.transform.position,
                    MonsterType.RedHat);
                break;
            case MonsterType.Mac:
                MonsterPoolManager._Instance._Mac.ItemSetActive(
                    _DummyLocationEffect.transform.position,
                    MonsterType.Mac);
                break;
            case MonsterType.Tiber:
                MonsterPoolManager._Instance._Tiber.ItemSetActive(
                    _DummyLocationEffect.transform.position,
                    MonsterType.Tiber);
                break;
        }
        dummySet = false;
        _DummyLocationEffect.SetActive(false);
        UserInterface.SetPointerMode(false);
    }

    public void SetValue()
    {
        _PlayerInstance = PlayerFSMManager.Instance;
    }
}
