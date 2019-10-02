using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MC.Mission;

public class Dungeon : MonoBehaviour
{
    public bool _IsLobby;
    public Transform _EnterPosition;
    public Collider _ExitPosition;
    public Transform _RespawnParent;
    public MissionEnter _Trigger;
    public MissionBase _Mission;

    [System.NonSerialized] public Transform[] _RespawnPositions;

    MissionData _MissionData;

    private void Awake()
    {
        if(!_IsLobby)
            _Mission = GetComponent<MissionBase>();

        Transform[] temp = _RespawnParent.GetComponentsInChildren<Transform>();
        var tempLength = temp.Length - 1;
        _RespawnPositions = new Transform[tempLength];

        int i = 0;
        foreach (Transform respawn in temp)
        {
            if (respawn != _RespawnParent)
            {
                _RespawnPositions[i] = respawn;
                i++;
            }
        }

        if(!_IsLobby)
            _ExitPosition.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    //public void SetMissionData(MissionData missionData)
    //{
    //    Debug.Log("선택된 던전의 미션 정보를 변경합니다.");

    //    _MissionData = missionData;
    //}
}
