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
    Pause,
    MissionClear,
    Dead,
    Tutorial,
    Product,
    EDITOR,
    End,
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


    public float _LimitTime = 180;
    public bool _MissionStatus = false;

    public bool canInput = true;

    public PlayerFSMManager _PlayerInstance;


    bool isPause = false;

    public static bool GameClear = false;

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
        if (ActivedMonsterList.Count == 0) return;

        // 몬스터 타입에따라서
        // 풀로 반환한다.
        List<GameObject> active = ActivedMonsterList;
        //ActivedMonsterList.Clear();

        foreach(GameObject mob in active)
        {
            try
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
            catch
            {

            }
        }

        if(MissionManager.Instance.CurrentMissionType == MissionType.Annihilation)
        {
            MissionA missionA = MissionManager.Instance.CurrentMission as MissionA;

            if(missionA.currentWave < missionA.totalWave)
                missionA.Invoke("MonsterCheck", 5f);
            //else if(missionA.currentWave == missionA.totalWave)
            //    missionA.ClearMission();
        }
            

        ActivedMonsterList.Clear();
    }

    public void Update()
    {
        if (!canInput) return;
        //if (Time.timeScale == 0 && Input.anyKey) return;
        
       
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
            if (currentGameState == CurrentGameState.Dialog ||
                currentGameState == CurrentGameState.Loading ||
                currentGameState == CurrentGameState.MissionClear ||
                UserInterface.Instance.MissionSelectionUICanvas.activeSelf) return;

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

                MissionManager.Instance.CurrentMission.PortalPlay();

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
            if (GameStatus.Instance.StageLevel == MissionManager.Instance.minimumLevel)
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

            if (GameStatus.Instance.StageLevel == MissionManager.Instance.maximumLevel)
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

    public void SetValue()
    {
        _PlayerInstance = PlayerFSMManager.Instance;
    }
}
