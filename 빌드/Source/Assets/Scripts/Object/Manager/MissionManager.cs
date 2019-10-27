using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.SceneDirector;
using MC.Mission;

/// <summary>
/// 미션의 종류
/// </summary>
public enum MissionType
{
    Annihilation = 0,
    Survival = 1,
    Defence = 2,
    Boss = 3,
    Tutorial = 4,
    Last,
}
public enum MissionRewardType
{
    SpecialGauge = 0,
    Str,
    Defense,
    Hp,
    Skill1Damage,
    Skill2Damage,
    Skill3Damage,
    Skill3Speed,
    Skill1Bounce,
    Last,
}
[System.Serializable]
public class MissionResources
{
    public Sprite[] types;
}

public class MissionManager : MonoBehaviour
{
    public MissionResources resources;

    private static MissionManager _Instance;
    public static MissionManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<MissionManager>();
            }
            return _Instance;
        }
    }

    [SerializeField] private MissionBase currentMission;
    public MissionBase CurrentMission
    {
        get
        {
            if (currentMission == null) currentMission = GameObject.FindObjectOfType<MissionBase>();
            return currentMission;
        }
        set
        {
            currentMission = value;
        }
    }


    public MissionType CurrentMissionType => CurrentMission.Data.MissionType;
    [SerializeField] private MissionRewardType[] currentMissionRewards;

    //public MissionRewardType CurrentMissionRewardType => //Currentmi
    private bool isFirst = true;
    public bool isChange = false;
    // For Editor Using

    public void Awake()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<MissionManager>();

        }
        else
        {
            //Destroy(gameObject);
        }

        currentMissionRewards = new MissionRewardType[2];
    }


    public GameObject MissionSelector;
    public MissionButton[] Choices;
    public RewardData rewardData;

    public GameObject MissionProgressUI;

    public static void PopUpMission()
    {
        if (Instance.isChange) return;

        GameStatus.SetCurrentGameState(CurrentGameState.Select);
        UserInterface.BlurSet(true);

        UserInterface.SetMissionSelectionUI(true);
        UserInterface.SetPointerMode(true);

        Instance.ChangeMission();
        GameManager.Instance.CharacterControl = false;
    }

    public void ChangeMission()
    {
        if (isChange) return;

        //랜덤 미션 출력하기
        foreach (MissionButton choice in UserInterface.Instance.SelectorUI.buttons)
        {
            var type = UnityEngine.Random.Range(0, 999) % ((int)(MissionType.Boss));
            choice.ChangeMission(type);
        }

        if (GameStatus.Instance.StageLevel >= 3)
        {
            UserInterface.Instance.SelectorUI.buttons[0].ChangeMission((int)MissionType.Boss);
        }

        if (GameStatus.Instance.StageLevel >= 8)
        {
            UserInterface.Instance.SelectorUI.buttons[0].ChangeMission((int)MissionType.Boss);
            UserInterface.Instance.SelectorUI.buttons[1].ChangeMission((int)MissionType.Boss);
            UserInterface.Instance.SelectorUI.buttons[2].ChangeMission((int)MissionType.Boss);
        }

        ChangeReward();

        isChange = true;
    }

    public void ChangeReward()
    {

        //랜덤 보상 출력하기
        foreach (MissionButton choice in UserInterface.Instance.SelectorUI.buttons)
        {
            if (choice.missionType == MissionType.Boss)
            {
                choice.rewardIcon.gameObject.SetActive(false);
                choice.rewardText[0].gameObject.SetActive(false);
                choice.rewardText[1].gameObject.SetActive(false);
            }
            else
            {
                choice.rewardIcon.gameObject.SetActive(true);
                choice.rewardText[0].gameObject.SetActive(true);
                choice.rewardText[1].gameObject.SetActive(true);
                var type = UnityEngine.Random.Range((int)MissionRewardType.SpecialGauge, (int)MissionRewardType.Last);
                var type2 = UnityEngine.Random.Range((int)MissionRewardType.SpecialGauge, (int)MissionRewardType.Last);

                while (type == type2)
                {
                    type2 = UnityEngine.Random.Range((int)MissionRewardType.SpecialGauge, (int)MissionRewardType.Last);
                }

                currentMissionRewards[0] = choice.ChangeReward(0, (MissionRewardType)type);
                currentMissionRewards[1] = choice.ChangeReward(1, (MissionRewardType)type2);
            }
        }
    }

    public static void SelectMission(MissionType type)
    {

        UserInterface.BlurSet(false);

        UserInterface.SetMissionSelectionUI(false);
        UserInterface.SetPointerMode(false);
        GameManager.Instance.IsPuase = false;
        //UserInterface.FullModeSetMP();

        if (type == MissionType.Boss)
        {
            MCSceneManager.Instance.NextScene(MCSceneManager.BOSS, 1f, true);
        }
        else
        {
            switch (type)
            {
                case MissionType.Annihilation:
                    MCSceneManager.Instance.NextScene(MCSceneManager.ANNIHILATION, 1f, true);
                    break;
                case MissionType.Defence:
                    MCSceneManager.Instance.NextScene(MCSceneManager.DEFENCE, 1f, true);
                    break;
                case MissionType.Survival:
                    MCSceneManager.Instance.NextScene(MCSceneManager.SURVIVAL, 1f, true);
                    break;
            }

            EnterMission();
        }

    }

    // 클리어 됬을떄 받는다면, 게이지는 클리어 직후에 100으로 차게됨
    // 다음씬에서 게이지가 0으로 초기화됨

    public void GetReward(MissionRewardType type)
    {
        switch (type)
        {
            case MissionRewardType.SpecialGauge:
                //PlayerFSMManager.Instance.SpecialGauge = 100;
                GameSetting.rewardAbillity.feverGauge = true;
                break;
            case MissionRewardType.Str:
                //4PlayerFSMManager.Instance.Stat.RewardStr(5);
                GameSetting.rewardAbillity.strLevel++;
                break;
            case MissionRewardType.Defense:
                //PlayerFSMManager.Instance.Stat.RewardDefense(3);
                GameSetting.rewardAbillity.defLevel++;
                break;
            case MissionRewardType.Hp:
                //PlayerFSMManager.Instance.Stat.RewardHP(150);
                GameSetting.rewardAbillity.hpLevel++;
                break;
            case MissionRewardType.Skill1Damage:
                //PlayerFSMManager.Instance.Stat.RewardSkill1Damage(40);
                GameSetting.rewardAbillity.skill1DMGLevel++;
                break;
            case MissionRewardType.Skill2Damage:
                //PlayerFSMManager.Instance.Stat.RewardSkill2Damage(25);
                GameSetting.rewardAbillity.skill2DMGLevel++;
                break;
            case MissionRewardType.Skill3Damage:
                //PlayerFSMManager.Instance.Stat.RewardSkill3Damage(10);
                GameSetting.rewardAbillity.skill3DMGLevel++;
                break;
            case MissionRewardType.Skill3Speed:
                //PlayerFSMManager.Instance.Skill3MouseSpeed += 10;
                GameSetting.rewardAbillity.skill3TurnLevel++;
                break;
            case MissionRewardType.Skill1Bounce:
                //PlayerFSMManager.Instance.Skill1BounceCount++;
                GameSetting.rewardAbillity.skill1BounceLevel++;
                break;
        }

    }

    public static void EnterMission()
    {
        // 캐릭터 위치변경
        //Instance.CurrentMission.gameObject.SetActive(true);

        //PlayerFSMManager.Instance.Anim.
        //    transform.position =
        //    Instance.CurrentMission.Enter.transform.position;

        //PlayerFSMManager.Instance.Anim.
        //    transform.LookAt(Instance.CurrentMission.Exit.transform);


    }

    public static void StartMission()
    {
        // 미션 시작지

        Instance.isChange = false;
        PlayerFSMManager.Instance.rigid.useGravity = true;

        Instance.CurrentMission.OperateMission();
        UserInterface.SetMissionProgressUserInterface(true);
    }

    public static void RewardMission()
    {
        // 여기서 보상에 관한 것을 처리함.
        if (GameStatus.currentGameState == CurrentGameState.Tutorial) return;

        if (Instance.currentMissionRewards[0] == Instance.currentMissionRewards[1])
        {
            Instance.currentMissionRewards[0] = MissionRewardType.Defense;
            Instance.currentMissionRewards[1] = MissionRewardType.Hp;
        }

        Instance.GetReward(Instance.currentMissionRewards[0]);
        Instance.GetReward(Instance.currentMissionRewards[1]);

        UserInterface.ClearMissionSetActive(true);
        UserInterface.Instance.ClearMission.SetClearMission(
            GameStatus.Instance._LimitTime,
            Instance.currentMissionRewards[0],
            Instance.currentMissionRewards[1]);

        Instance.currentMissionRewards[0] = MissionRewardType.Last;
        Instance.currentMissionRewards[1] = MissionRewardType.Last;
    }

    public static void ExitMission()
    {
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

        //var Maps = GameObject.FindObjectsOfType<MissionBase>();
        //Missions = Maps;
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
