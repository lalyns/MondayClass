using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public Transform _EnterPosition;
    public Collider _ExitPosition;

    public Transform[] _RespawnPositions;

    MissionData _MissionData;

    private void Update()
    {
        
    }

    public void SetMissionData(MissionData missionData)
    {
        Debug.Log("선택된 던전의 미션 정보를 변경합니다.");

        _MissionData = missionData;
    }

}
