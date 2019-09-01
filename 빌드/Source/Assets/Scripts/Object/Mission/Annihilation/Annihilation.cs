using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annihilation : Mission
{
    [SerializeField] private float _LimitTime = 180f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!_IsMissionStart || _IsMissionClear) return;


        _LimitTime -= Time.deltaTime;
        _UI.SetLeftTime(_LimitTime);

        if (CheckForClear())
        {
            MissionClear();
            _IsMissionClear = true;
        }
    }

    public override void MissionInitialize()
    {
        base.MissionInitialize();

        _UI._AnnihilationUI.SetActive(true);
        _UI.SetMissionType("섬멸 미션");
        _UI.SetMissionString("적을 모두 처치하라");
    }

    private bool CheckForClear()
    {
        bool isClear = false;

        // 현재 데이터를 가져오는 구조가없기때문에 보류
        //if (_CurrentMissionLevel != 0) return isClear;

        int activeItem =
            ObjectManager._Instance._ObjectPool[0]._ActiveItem.Count
            + ObjectManager._Instance._ObjectPool[1]._ActiveItem.Count;

        _UI.SetLeftMonster(activeItem);

        //Debug.Log(activeItem);

        if (activeItem == 0)
        {
            isClear = true;
        }

        return isClear;
    }

    public override void MissionClear()
    {
        base.MissionClear();
    }

    public override void MissionEnd()
    {
        base.MissionEnd();

        _LimitTime = 180f;
        _UI._AnnihilationUI.SetActive(false);
    }
}
