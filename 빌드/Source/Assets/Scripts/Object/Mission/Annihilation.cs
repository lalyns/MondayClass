using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annihilation : MonoBehaviour
{
    private MissionManager _MissionManager;

    [SerializeField] private MissionData _MissionData;
    [SerializeField] private int _CurrentMissionLevel;

    [System.NonSerialized] private bool _IsMissionStart;

    private void Awake()
    {
        _IsMissionStart = false;
        _MissionManager = MissionManager.GetMissionManager;
    }

    private void Update()
    {
        if (!_IsMissionStart) return;

        if (CheckForClear())
        {
            MissionClear();
        }
    }

    private bool CheckForClear()
    {
        bool isClear = false;

        if (_CurrentMissionLevel != 3) return isClear;

        int activeItem =
            ObjectManager._Instance._ObjectPool[0]._ActiveItem.Count
            + ObjectManager._Instance._ObjectPool[1]._ActiveItem.Count;

        if(activeItem == 0)
        {
            isClear = true;
        }

        return isClear;
    }

    private void MissionInitialize()
    {
        // 미션 데이터 초기화
        _CurrentMissionLevel = 0;
        _IsMissionStart = true;

    }

    private void MissionClear()
    {
        // 데이터 초기화
        // 플레이어 버프 초기화
        // 
    }

}
