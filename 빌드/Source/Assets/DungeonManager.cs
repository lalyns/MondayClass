using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _Instance;
    public int _MissionSize;

    public Canvas Mission;
    public Button Choice1;
    public Button Choice2;
    public Button Choice3;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<DungeonManager>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MissionPopUp()
    {
        Mission.gameObject.SetActive(true);
    }

    public void MissionDisappear()
    {
        Mission.gameObject.SetActive(false);
    }

    public void MissionMenu()
    {

    }

    public MissionType SelectMission()
    {
        var temp = UnityEngine.Random.Range(0, 999999) % _MissionSize;
        MissionType mission = (MissionType)temp;

        return mission;
    }

    /// <summary>
    /// 미션을 결정합니다
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
        Annihilation = 1,
        Defence = 2,
        Survive = 3,
    }
}
