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

    protected bool _MissionOperate;
    public bool MissionOperate {
        get { return _MissionOperate; }
        internal set { _MissionOperate = value; }
    }

    protected virtual void Awake()
    {
        Enter = GetComponentInChildren<MissionEnter>();
        //Enter.Colliders.enabled = false;

        Exit = GetComponentInChildren<MissionExit>();
        Exit.Colliders.enabled = false;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public void OperateMission()
    {
        Debug.Log(GameManager.stageLevel);
        GameManager.stageLevel++;
        GameStatus._Instance._MissionStatus = true;

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

    // 선택지 출력 후 미션 이동 구현할 것
    public void NextMission(Transform player)
    {
        Exit.nextDungeon.SetActive(true);
        player.position = Exit.nextDungeon.GetComponentInChildren<DungeonEnter>().transform.position;

        MissionManager.Instance.CurrentMission =
            MissionManager.Instance.Mission[GameManager.stageLevel];
    }
    
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
