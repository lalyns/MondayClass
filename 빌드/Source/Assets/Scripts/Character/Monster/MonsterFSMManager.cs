using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    POPUP = 0,
    CHASE,
    ATTACK,
    DEAD
}

[RequireComponent(typeof(MonsterStat))]
public class MonsterFSMManager : FSMManager
{
    private bool _isInit = false;
    public MonsterState startState = MonsterState.POPUP;
    private Dictionary<MonsterState, MonsterFSMState> _States = new Dictionary<MonsterState, MonsterFSMState>();

    private MonsterState _CurrentState;
    public MonsterState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public MonsterFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private MonsterStat _Stat;
    public MonsterStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim { get { return _Anim; } }

    public Transform _AttackTransform;
    public MeshRenderer _MR;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<MonsterStat>();
        _Anim = GetComponentInChildren<Animator>();

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        MonsterState[] stateValues = (MonsterState[])System.Enum.GetValues(typeof(MonsterState));
        foreach (MonsterState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Monster" + s.ToString());
            MonsterFSMState state = (MonsterFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (MonsterFSMState)gameObject.AddComponent(FSMType);
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
