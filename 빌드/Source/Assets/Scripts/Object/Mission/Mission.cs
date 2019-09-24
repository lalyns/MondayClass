﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    protected MissionEnter _Enter;
    public MissionEnter Enter {
        get {
            if (_Enter == null)
            {
                _Enter = GetComponentInChildren<MissionEnter>();
            }
            return _Enter;
        }
        internal set {
            _Enter = value;
        }
    }

    protected MissionExit _Exit;
    public MissionExit Exit {
        get {
            if(_Exit == null)
            {
                _Exit = GetComponentInChildren<MissionExit>();
            }
            return _Exit;
        }
        internal set {
            _Exit = value;
        }
    }

    [SerializeField] protected MissionData _Data;
    public MissionData Data {
        get { return _Data; }
    }

    [SerializeField] protected bool _MissionOperate;
    public bool MissionOperate {
        get { return _MissionOperate; }
        internal set { _MissionOperate = value; }
    }

    public int _LimitTime = 180;
    public MissionManager.MissionType MissionType;

    protected bool MissionEnd = false;

    protected virtual void Awake()
    {
        Enter = GetComponentInChildren<MissionEnter>();
        //Enter.Colliders.enabled = false;

        Exit = GetComponentInChildren<MissionExit>();
        try
        {
            Exit.Colliders.enabled = false;
        }
        catch
        {

        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public virtual void RestMission()
    {

    }

    public virtual void OperateMission()
    {
        GameManager.stageLevel++;
        GameStatus._Instance._MissionStatus = true;
        GameStatus._Instance._LimitTime = _LimitTime;

        MissionOperate = true;
        Exit.Colliders.enabled = false;
    }

    public void ClearMission()
    {
        GameStatus._Instance._MissionStatus = false;
        GameStatus._Instance.RemoveAllActiveMonster();

        Exit._PortalEffect.SetActive(true);
        Exit.Colliders.enabled = true;
    }

    public GameObject _MissionSelector;

    #region 폐기
    //public int _CurrentMissionLevel;
    //public bool _IsMissionStart;
    //public bool _IsMissionClear;

    //[SerializeField] protected UIMissionProgress _UI;

    //protected MissionManager _MissionManager;
    //protected Dungeon _Dungeon;

    //protected virtual void Awake()
    //{
    //    _IsMissionStart = false;
    //    _MissionManager = MissionManager.GetMissionManager;
    //    _Dungeon = GetComponent<Dungeon>();
    //}

    //public virtual void MissionInitialize()
    //{
    //    // 미션 데이터 초기화
    //    _CurrentMissionLevel = 0;
    //}

    //public void MissionStart()
    //{
    //    _IsMissionStart = true;

    //}

    //public virtual void MissionClear()
    //{
    //    // 데이터 초기화
    //    // 플레이어 버프 초기화

    //    // 다음 스테이지로 향하는 포탈 활성화
    //    DungeonManager.GetDungeonManager().DungeonClear();
    //    ObjectManager._Instance.StopSpawn();
    //}

    //public virtual void MissionEnd()
    //{
    //    _IsMissionStart = false;
    //    _IsMissionClear = false;
    //}
    #endregion
}
