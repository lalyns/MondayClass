using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.SceneDirector;

/// <summary>
/// 미션의 종류
/// </summary>
public enum MissionType
{
    Annihilation = 0,
    Defence = 1,
    Survival = 2,
    Boss = 3,
    Last,
}

public class MissionManager : MonoBehaviour
{
    private static MissionManager _Instance;
    public static MissionManager Instance {
        get {
            if(_Instance == null)
            {
                _Instance = FindObjectOfType<MissionManager>();
            }
            return _Instance;
        }
    }

    [SerializeField] private Mission[] _Missions;
    public Mission[] Missions {
        get {
            return _Missions;
        }
        set
        {
            _Missions = value;
        }
    }

    private Mission currentMission;
    public Mission CurrentMission {
        get {
            if (currentMission == null) currentMission = GameObject.FindObjectOfType<Mission>();
            return currentMission;
        }
        set {
            currentMission = value;
        }
    }
    public MissionType CurrentMissionType => CurrentMission.Data.MissionType;

    private bool isFirst = true;
    public bool isChange = false;
    // For Editor Using

    public void Awake()
    {
        if(_Instance == null)
        {
            _Instance = GetComponent<MissionManager>();
            
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public GameObject MissionSelector;
    public MissionButton[] Choices;

    public GameObject MissionProgressUI;

    public static void PopUpMission() {
        if (Instance.isChange) return;

        UserInterface.BlurSet(true);


        Instance.MissionSelector.SetActive(true);
        UserInterface.SetPointerMode(true);
        UserInterface.Instance.MousePointerSpeed(100f);

        Instance.ChangeMission();

        GameManager.Instance.CharacterControl = false;
        GameManager.Instance.IsPuase = true;
    }

    public void ChangeMission()
    {
        if (isChange) return;

        //랜덤 미션 출력하기
        foreach (MissionButton choice in Instance.Choices)
        {
            var type = UnityEngine.Random.Range(0, 999) % ((int)(MissionType.Last) - 1);
            choice.ChangeMission(Instance.Missions[type]);
        }

        if (GameStatus.Instance.StageLevel >= 3)
        {
            Instance.Choices[0].ChangeMission(Instance.Missions[3]);
        }

        isChange = true;
    }

    public static void SelectMission(Mission mission) {
        
        Instance.CurrentMission = mission;
        Instance.MissionSelector.SetActive(false);

        if (Instance.CurrentMissionType == MissionType.Boss)
        {
            UserInterface.SetPlayerUserInterface(false);
            Instance.StartCoroutine(MCSceneManager.Instance.LoadScene(2));
        }
        else
        {
            UserInterface.SetPointerMode(false);
            GameManager.Instance.IsPuase = false;
            UserInterface.FullModeSetMP();

            // 페이드 Out
            GameManager.SetFadeInOut(() =>
            {

                MissionManager.EnterMission();
                UserInterface.BlurSet(false);
                // RigidBody Gravity => false
                PlayerFSMManager.Instance.rigid.useGravity = false;
            }, false);
        }
        //EnterMission();
    }

    public static void EnterMission() {
        // 캐릭터 위치변경
        Instance.CurrentMission.gameObject.SetActive(true);

        PlayerFSMManager.Instance.Anim.
            transform.position =
            Instance.CurrentMission.Enter.transform.position;

        PlayerFSMManager.Instance.Anim.
            transform.LookAt(Instance.CurrentMission.Exit.transform);


        CinemaManager.CinemaStart(Instance.CurrentMission.enterDirector);
        Instance.isChange = true;
    }

    public static void StartMission() {
        // 미션 시작지

        PlayerFSMManager.Instance.rigid.useGravity = true;

        Instance.CurrentMission.OperateMission();
        UserInterface.SetMissionProgressUserInterface(true);
    }

    public static void RewardMission() {

    }

    public static void ExitMission() {
        Input.ResetInputAxes();

        PlayerFSMManager.Instance._v = 0; //SetState(PlayerState.IDLE);
        PlayerFSMManager.Instance._h = 0;
        PlayerFSMManager.Instance.SetState(PlayerState.IDLE);

        if (Instance.isFirst) { Instance.isFirst = false; return; }
        Instance.CurrentMission.RestMission();
    }

    public void SetValue()
    {
        MissionSelector = UserInterface.Instance.MissionSelectionUICanvas;
        MissionProgressUI = UserInterface.Instance.MissionProgressUICanvas;

        var Maps = GameObject.FindObjectsOfType<Mission>();
        Missions = Maps;
        Choices = UserInterface.Instance.SelectorUI.buttons;
    }


    #region 폐기
    //private void Awake()
    //{
    //    //if (_Instance == null)
    //    //{
    //    //    _Instance = GetComponent<MissionManager>();

    //    //    if (GameManager._Instance._IsDummyScene)
    //    //        return;

    //    //    _Choices = new MissionButton[3];
    //    //    _Instance._Choices[0] = _Instance._UIMission.transform.GetChild(1).GetComponent<MissionButton>();
    //    //    _Instance._Choices[1] = _Instance._UIMission.transform.GetChild(2).GetComponent<MissionButton>();
    //    //    _Instance._Choices[2] = _Instance._UIMission.transform.GetChild(3).GetComponent<MissionButton>();

    //    //}
    //    //else
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //}

    /// <summary>
    /// 미션 정보창을 화면에 표시하는 매소드
    /// </summary>
    //public static void PopUpMissionMenu()
    //{
    //    //Debug.Log("미션의 정보창을 화면에 표기합니다.");
    //    _Instance._UIMission.SetActive(true);
    //    _Instance.ChangeMissionMenu();
    //    GameManager.CursorMode(true);
    //    GameManager._Instance._CharacterControl = false;

    //    Time.timeScale = 0.0f;
    //}

    ///// <summary>
    ///// 미션 정보창을 화면에서 지워주는 매소드
    ///// </summary>
    //public static void DisappearMissionMenu()
    //{
    //    //Debug.Log("미션의 정보창을 화면에서 지웁니다.");
    //    _Instance._UIMission.SetActive(false);
    //    GameManager.CursorMode(false);
    //    GameManager._Instance._CharacterControl = true;
    //    GameManager.isPopUp = false;
    //    Time.timeScale = 1.0f;
    //}

    ///// <summary>
    ///// 미션 정보창의 정보창 내용을 변경하는 매소드
    ///// </summary>
    //public void ChangeMissionMenu()
    //{
    //    //Debug.Log("미션 선택창의 정보를 변경합니다.");

    //    for (int i = 0; i < _Choices.Length; i++)
    //    {
    //        MissionType newMission = SelectMission();
    //        _Choices[i]._MissionType = newMission;
    //        _Choices[i].ChangeMission(_MissionDatas[0], newMission);
    //        _Choices[i].ChangeReward(_RewardDatas[SetReward()]);
    //    }
    //}

    ///// <summary>
    ///// 버튼 클릭시 미션의 내용을 설정하는 매소드
    ///// </summary>
    ///// <param name="choiceNum"> 버튼의 숫자 </param>
    //public static void SetMissionOnClick(int choiceNum)
    //{
    //    //_Instance._CurrentMission = _Instance._Choices[choiceNum]._MissionType;
    //    Dungeon dungeon = DungeonManager.CreateDungeon(MissionType.Annihilation);
    //    //MissionData missionData = GetMissionData(_Instance._CurrentMission);

    //    /// <summary>
    //    /// 플레이어의 위치변경 매소드 필요
    //    /// </summary>
    //    GameObject.FindGameObjectWithTag("Player").transform.position
    //        = dungeon._EnterPosition.position;
    //    MissionManager.DisappearMissionMenu();

    //    /// <summary>
    //    /// 게임 로딩
    //    /// </summary>

    //    DungeonManager.SetCurrentDungeon(dungeon);
    //    DungeonManager.GetCurrentDungeon()._Mission.MissionInitialize();
    //    ObjectManager.SetSpawnPosition(dungeon._RespawnPositions);
    //    dungeon._ExitPosition.gameObject.SetActive(false);
    //}

    ///// <summary>
    ///// 미션의 종류를 랜덤으로 설정하는 매소드
    ///// </summary>
    ///// <returns> 미션의 종류 </returns>
    //public MissionType SelectMission()
    //{
    //    var temp = UnityEngine.Random.Range(0, 999999) % (int)MissionType.Last;
    //    MissionType mission = (MissionType)temp;

    //    return mission;
    //}

    //public void StartMission()
    //{
    //    Debug.Log("미션 시작");
    //    DungeonManager.GetCurrentDungeon()._Mission.MissionStart();
    //    ObjectManager._Instance.CallSpawn();
    //}

    ///// <summary>
    ///// 보상의 종류를 설정하는 매소드
    ///// </summary>
    ///// <returns> 보상의 종류 </returns>
    //public int SetReward()
    //{
    //    var temp = UnityEngine.Random.Range(0, 999999) % _RewardDatas.Length;
    //    return temp;
    //}

    ///// <summary>
    ///// 미션을 확정합니다
    ///// </summary>
    ///// <param name="dungeonType"> 결정된 미션 </param>
    //public static MissionData GetMissionData(MissionType dungeonType)
    //{
    //    MissionData missionData;
    //    missionData = _Instance._MissionDatas[0];

    //    return missionData;
    //}

    ///// <summary>
    ///// 미션 종료를 알려주는 메소드. 여기서 연출을 처리
    ///// </summary>
    //public static void MissionClear()
    //{
    //    //Debug.Log("미션 종료");

    //    DungeonManager.GetCurrentDungeon()._Trigger.isStart = false;
    //    try
    //    {
    //        DungeonManager.GetCurrentDungeon()._Mission.MissionEnd();
    //    }
    //    catch
    //    {

    //    }

    //    MissionManager.PopUpMissionMenu();

    //}
    #endregion
}
