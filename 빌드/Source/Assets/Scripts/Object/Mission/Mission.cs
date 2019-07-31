using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public int _CurrentMissionLevel;
    public bool _IsMissionStart;
    public bool _IsMissionClear;

    protected MissionManager _MissionManager;
    protected Dungeon _Dungeon;

    protected virtual void Awake()
    {
        _IsMissionStart = false;
        _MissionManager = MissionManager.GetMissionManager;
        _Dungeon = GetComponent<Dungeon>();
    }

    public void MissionInitialize()
    {
        // 미션 데이터 초기화
        _CurrentMissionLevel = 0;
    }

    public void MissionStart()
    {
        _IsMissionStart = true;

    }

    public virtual void MissionClear()
    {
        // 데이터 초기화
        // 플레이어 버프 초기화

        // 다음 스테이지로 향하는 포탈 활성화
        DungeonManager.GetDungeonManager().DungeonClear();
        ObjectManager._Instance.StopSpawn();
    }

    public virtual void MissionEnd()
    {
        _IsMissionStart = false;
        _IsMissionClear = false;
    }

}
