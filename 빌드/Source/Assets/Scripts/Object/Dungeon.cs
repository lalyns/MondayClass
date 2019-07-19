using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public Transform _EnterPosition;
    public Collider _ExitPosition;
    public Transform _RespawnParent;

    [System.NonSerialized] public Transform[] _RespawnPositions;

    MissionData _MissionData;

    private void Awake()
    {
        _RespawnPositions = _RespawnParent.GetComponentsInChildren<Transform>();
        foreach(Transform respawn in _RespawnPositions)
        {
            Debug.Log(respawn.name);
        }
    }

    private void Update()
    {
        
    }

    public void SetMissionData(MissionData missionData)
    {
        Debug.Log("선택된 던전의 미션 정보를 변경합니다.");

        _MissionData = missionData;
    }

    public void DungeonClear()
    {
        DungeonManager manager = DungeonManager.GetDungeonManager();
        manager._ClearCallback += manager.DungeonClearCallBack;
        manager._ClearCallback();
    }

}
