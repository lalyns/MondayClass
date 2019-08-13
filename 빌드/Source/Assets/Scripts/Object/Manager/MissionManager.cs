﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public MissionData[] _MissionDatas;
    public RewardData[] _RewardDatas;

    public GameObject _UIMission;

    public static MissionManager _Instance;
    public static MissionManager GetMissionManager {
        get { return _Instance; }
    }
    public MissionButton[] _Choices;

    public MissionType _CurrentMission;

    public static bool _IsMissionStart = false;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<MissionManager>();

            _Choices = new MissionButton[3];
            _Instance._Choices[0] = _Instance._UIMission.transform.GetChild(1).GetComponent<MissionButton>();
            _Instance._Choices[1] = _Instance._UIMission.transform.GetChild(2).GetComponent<MissionButton>();
            _Instance._Choices[2] = _Instance._UIMission.transform.GetChild(3).GetComponent<MissionButton>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 미션 정보창을 화면에 표시하는 매소드
    /// </summary>
    public static void PopUpMissionMenu()
    {
        //Debug.Log("미션의 정보창을 화면에 표기합니다.");
        _Instance._UIMission.SetActive(true);
        _Instance.ChangeMissionMenu();
        GameManager.CursorMode(false);

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// 미션 정보창을 화면에서 지워주는 매소드
    /// </summary>
    public static void DisappearMissionMenu()
    {
        //Debug.Log("미션의 정보창을 화면에서 지웁니다.");
        _Instance._UIMission.SetActive(false);
        GameManager.CursorMode(true);

        GameManager.isPopUp = false;
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// 미션 정보창의 정보창 내용을 변경하는 매소드
    /// </summary>
    public void ChangeMissionMenu()
    {
        //Debug.Log("미션 선택창의 정보를 변경합니다.");

        for (int i = 0; i < _Choices.Length; i++)
        {
            MissionType newMission = SelectMission();
            _Choices[i]._MissionType = newMission;
            _Choices[i].ChangeMission(_MissionDatas[(int)newMission]);
            _Choices[i].ChangeReward(_RewardDatas[SetReward()]);
        }
    }

    /// <summary>
    /// 버튼 클릭시 미션의 내용을 설정하는 매소드
    /// </summary>
    /// <param name="choiceNum"> 버튼의 숫자 </param>
    public static void SetMissionOnClick(int choiceNum)
    {
        //Debug.Log("미션을 선택합니다.");
        _Instance._CurrentMission = _Instance._Choices[choiceNum]._MissionType;

        Dungeon dungeon = DungeonManager.CreateDungeon(_Instance._CurrentMission);

        MissionData missionData = GetMissionData(_Instance._CurrentMission);

        //Debug.Log(missionData.name);


        /// <summary>
        /// 플레이어의 위치변경 매소드 필요
        /// </summary>
        GameObject.FindGameObjectWithTag("Player").transform.position
            = dungeon._EnterPosition.position;
        
        MissionManager.DisappearMissionMenu();

        /// <summary>
        /// 게임 로딩
        /// </summary>
        
        DungeonManager.SetCurrentDungeon(dungeon);

        DungeonManager.GetCurrentDungeon()._Mission.MissionInitialize();

        ObjectManager.SetSpawnPosition(dungeon._RespawnPositions);
        dungeon._ExitPosition.gameObject.SetActive(false);
    }

    /// <summary>
    /// 미션의 종류를 랜덤으로 설정하는 매소드
    /// </summary>
    /// <returns> 미션의 종류 </returns>
    public MissionType SelectMission()
    {
        var temp = UnityEngine.Random.Range(0, 999999) % _MissionDatas.Length;
        MissionType mission = (MissionType)temp;

        return mission;
    }

    public void StartMission()
    {
        DungeonManager.GetCurrentDungeon()._Mission.MissionStart();
        ObjectManager._Instance.CallSpawn();
    }

    /// <summary>
    /// 보상의 종류를 설정하는 매소드
    /// </summary>
    /// <returns> 보상의 종류 </returns>
    public int SetReward()
    {
        var temp = UnityEngine.Random.Range(0, 999999) % _RewardDatas.Length;
        return temp;
    }

    /// <summary>
    /// 미션을 확정합니다
    /// </summary>
    /// <param name="dungeonType"> 결정된 미션 </param>
    public static MissionData GetMissionData(MissionType dungeonType)
    {
        MissionData missionData;
        missionData = _Instance._MissionDatas[(int)dungeonType];

        return missionData;
    }

    /// <summary>
    /// 미션의 종류
    /// </summary>
    public enum MissionType
    {
        Annihilation = 0,
        Survive = 1,
        Defence = 2,
        Last,
    }

    /// <summary>
    /// 미션 종료를 알려주는 메소드. 여기서 연출을 처리
    /// </summary>
    public static void MissionClear()
    {
        //Debug.Log("미션 종료");

        DungeonManager.GetCurrentDungeon()._Trigger.isStart = false;
        try
        {
            DungeonManager.GetCurrentDungeon()._Mission.MissionEnd();
        }
        catch
        {

        }

        MissionManager.PopUpMissionMenu();

    }

}
