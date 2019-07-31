using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annihilation : Mission
{
    [SerializeField] private MissionData _MissionData;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!_IsMissionStart || _IsMissionClear) return;

        if (CheckForClear())
        {
            MissionClear();
            _IsMissionClear = true;
        }
    }

    private bool CheckForClear()
    {
        bool isClear = false;

        // 현재 데이터를 가져오는 구조가없기때문에 보류
        //if (_CurrentMissionLevel != 0) return isClear;

        int activeItem =
            ObjectManager._Instance._ObjectPool[0]._ActiveItem.Count
            + ObjectManager._Instance._ObjectPool[1]._ActiveItem.Count;

        //Debug.Log(activeItem);

        if(activeItem == 0)
        {
            isClear = true;
        }

        return isClear;
    }


}
