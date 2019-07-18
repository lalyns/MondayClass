using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _Instance;

    public GameObject _UIMission;

    public MissionButton[] _Choices;
    public MissionManager _MissionManager;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<DungeonManager>();

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

    public static void MissionPopUp()
    {
        _Instance._UIMission.SetActive(true);
        _Instance.MissionMenuChange();

        Time.timeScale = 0.0f;
    }

    public static void MissionDisappear()
    {
        _Instance._UIMission.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void MissionMenuChange()
    {
        for (int i = 0; i < _Choices.Length; i++)
        {
            MissionType curMission = SelectMission();
            _Choices[i].MissionChange(_MissionManager._MissionDatas[(int)curMission]);
            _Choices[i].RewardChange(_MissionManager._RewardDatas[SetReward()]);
        }
    }

    public static void SetMissionOnClick(int choiceNum)
    {
        SetMission((MissionType)choiceNum);
        DungeonManager.MissionDisappear();
    }

    public MissionType SelectMission()
    {
        var temp = UnityEngine.Random.Range(0, 999999) % _MissionManager._MissionDatas.Length;
        MissionType mission = (MissionType)temp;

        return mission;
    }

    public int SetReward()
    {
        var temp = UnityEngine.Random.Range(0, 999999) % _MissionManager._RewardDatas.Length;
        return temp;
    }

    /// <summary>
    /// 미션을 확정합니다
    /// </summary>
    /// <param name="dungeonType"> 결정된 미션 </param>
    public static void SetMission(MissionType dungeonType)
    {
        if (dungeonType == MissionType.Annihilation) {

        }

        else if (dungeonType == MissionType.Defence) {

        }

        else if (dungeonType == MissionType.Survive) {

        }
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
}
