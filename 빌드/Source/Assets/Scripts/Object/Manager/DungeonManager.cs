using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _Instance;
    public int _MissionSize;

    public GameObject _UIMission;
    public MissionButton[] Choices;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<DungeonManager>();

            Choices = new MissionButton[3];
            _Instance.Choices[0] = _Instance._UIMission.transform.GetChild(0).GetComponent<MissionButton>();
            _Instance.Choices[1] = _Instance._UIMission.transform.GetChild(1).GetComponent<MissionButton>();
            _Instance.Choices[2] = _Instance._UIMission.transform.GetChild(2).GetComponent<MissionButton>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void MissionPopUp()
    {
        _Instance._UIMission.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public static void MissionDisappear()
    {
        _Instance._UIMission.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void MissionMenuChange()
    {

    }

    public static void SetMissionOnClick(int choiceNum)
    {
        SetMission((MissionType)choiceNum);
    }

    public MissionType SelectMission()
    {
        var temp = UnityEngine.Random.Range(0, 999999) % _MissionSize;
        MissionType mission = (MissionType)temp;

        return mission;
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
