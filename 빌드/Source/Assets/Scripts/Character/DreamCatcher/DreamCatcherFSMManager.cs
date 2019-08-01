﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DreamCatcherStat))]
public class DreamCatcherFSMManager : FSMManager
{
    private bool _isInit = false;
    public MonsterState startState = MonsterState.POPUP;
    private Dictionary<MonsterState, DreamCatcherFSMState> _States = new Dictionary<MonsterState, DreamCatcherFSMState>();

    private MonsterState _CurrentState;
    public MonsterState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public DreamCatcherFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private DreamCatcherStat _Stat;
    public DreamCatcherStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim { get { return _Anim; } }

    public Transform _AttackTransform;
    public MeshRenderer _MR;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<DreamCatcherStat>();
        _Anim = GetComponentInChildren<Animator>();

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        MonsterState[] stateValues = (MonsterState[])System.Enum.GetValues(typeof(MonsterState));
        foreach (MonsterState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("DreamCatcher" + s.ToString());
            DreamCatcherFSMState state = (DreamCatcherFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (DreamCatcherFSMState)gameObject.AddComponent(FSMType);
            }

            _States.Add(s, state);
            state.enabled = false;
        }
    }

    private void Start()
    {
        SetState(startState);
        _isInit = true;
    }

    public void SetState(MonsterState newState)
    {
        if (_isInit)
        {
            _States[_CurrentState].enabled = false;
            _States[_CurrentState].EndState();
        }
        _CurrentState = newState;
        _States[_CurrentState].BeginState();
        _States[_CurrentState].enabled = true;
        _Anim.SetInteger("CurrentState", (int)_CurrentState);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Weapon")
        {
            ObjectManager.ReturnPoolMonster(this.gameObject, Stat.monsterData._IsRagne);
            
        }
    }
}
