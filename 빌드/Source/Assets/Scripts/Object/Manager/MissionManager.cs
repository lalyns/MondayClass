using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public MissionData[] _MissionDatas;
    public RewardData[] _RewardDatas;

    public GameObject _UIMission;

    public static MissionManager _Instance;
    public MissionButton[] _Choices;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<MissionManager>();

            _Choices = new MissionButton[3];
            _Instance._Choices[0] = _Instance._UIMission.transform.GetChild(0).GetComponent<MissionButton>();
            _Instance._Choices[1] = _Instance._UIMission.transform.GetChild(1).GetComponent<MissionButton>();
            _Instance._Choices[2] = _Instance._UIMission.transform.GetChild(2).GetComponent<MissionButton>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 미션 정보창을 화면에 표시하는 매소드
    /// </summary>
    public static void MissionMenuPopUp()
    {
        Debug.Log("미션의 정보창을 화면에 표기합니다.");
        _Instance._UIMission.SetActive(true);
        _Instance.MissionMenuChange();

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// 미션 정보창을 화면에서 지워주는 매소드
    /// </summary>
    public static void MissionDisappear()
    {
        Debug.Log("미션의 정보창을 화면에서 지웁니다.");
        _Instance._UIMission.SetActive(false);
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// 미션 정보창의 정보창 내용을 변경하는 매소드
    /// </summary>
    public void MissionMenuChange()
    {
        Debug.Log("미션 선택창의 정보를 변경합니다.");

        for (int i = 0; i < _Choices.Length; i++)
        {
            MissionType curMission = SelectMission();
            _Choices[i].MissionChange(_MissionDatas[(int)curMission]);
            _Choices[i].RewardChange(_RewardDatas[SetReward()]);
        }
    }

    /// <summary>
    /// 버튼 클릭시 미션의 내용을 설정하는 매소드
    /// </summary>
    /// <param name="choiceNum"> 버튼의 숫자 </param>
    public static void SetMissionOnClick(int choiceNum)
    {
        Debug.Log("미션을 선택합니다.");
        MissionData missionData = SetMission((MissionType)choiceNum);

        Dungeon dungeon = DungeonManager.CreateDungeon(missionData);

        MissionDisappear();
        DungeonManager.ChangeDungeon(dungeon);
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
    public static MissionData SetMission(MissionType dungeonType)
    {
        MissionData missionData;
        missionData = _Instance._MissionDatas[(int)dungeonType];

        //if (dungeonType == MissionType.Annihilation)
        //{
        //    missionData = _Instance._MissionDatas[(int)dungeonType];
        //}

        //else if (dungeonType == MissionType.Defence)
        //{
        //    missionData = _Instance._MissionDatas[(int)dungeonType];
        //}

        //else if (dungeonType == MissionType.Survive)
        //{
        //    missionData = _Instance._MissionDatas[(int)dungeonType];
        //}

        return missionData;
    }

    /// <summary>
    /// 미션의 종류
    /// </summary>
    public enum MissionType
    {
        Annihilation = 0,
        Defence = 1,
        Survive = 2,
    }

    public void MissionAnnihilation()
    {
        
    }

}
