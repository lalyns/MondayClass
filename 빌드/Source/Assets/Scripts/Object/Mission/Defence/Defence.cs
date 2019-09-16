//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Defence : Mission
//{
    

//    public float _LimitTime = 30f;
//    public float _CurrentLeftTime;

//    public ProtectedTarget _ProtectedTarget;
//    public int _ProtectedTargetHP;

//    protected override void Awake()
//    {
//        base.Awake();
//    }

//    private void Update()
//    {
//        if (!_IsMissionStart || _IsMissionClear) return;

//        _CurrentLeftTime -= Time.deltaTime;

//        _ProtectedTargetHP = _ProtectedTarget.hp;

//        _UI.SetProtectedTargetHP(_ProtectedTargetHP);
//        _UI.SetLeftDefenceTime(_CurrentLeftTime);

//        if (CheckForClear())
//        {
//            MissionClear();
//            _IsMissionClear = true;
//        }
//    }

//    public override void MissionInitialize()
//    {
//        base.MissionInitialize();
//        _UI._DefenceUI.SetActive(true);
//        _UI.SetMissionType("방어 미션");
//        _UI.SetMissionString("남은 시간동안 대상을 보호하십시오");
//        _CurrentLeftTime = _LimitTime;
//        _ProtectedTarget.SetProtectedHP();
//    }

//    private bool CheckForClear()
//    {
//        bool isClear = false;

//        if (_CurrentLeftTime <= 0f)
//        {
//            if(_ProtectedTarget.hp > 0)
//            {
//                isClear = true;
//            }
//        }

//        return isClear;
//    }

//    public override void MissionClear()
//    {
//        base.MissionClear();

//        ObjectManager.ReturnPoolAllMonster();

//        GameObject[] balls = GameObject.FindGameObjectsWithTag("Bullet");
//        foreach (GameObject ball in balls)
//        {
//            Destroy(ball);
//        }

//    }

//    public override void MissionEnd()
//    {
//        base.MissionEnd();

//        _ProtectedTarget.hp = 100;
//        _CurrentLeftTime = _LimitTime;
//        _UI._DefenceUI.SetActive(false);
//    }
//}
