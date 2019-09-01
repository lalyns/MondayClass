using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TiberState
{
    POPUP = 0,
    CHASE,
    ATTACK,
    DEAD,
    HIT
}
[RequireComponent(typeof(TiberStat))]
public class TiberFSMManager : FSMManager
{
    private bool _isInit = false;
    public TiberState startState = TiberState.POPUP;
    private Dictionary<TiberState, TiberFSMState> _States = new Dictionary<TiberState, TiberFSMState>();

    private TiberState _CurrentState;
    public TiberState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public TiberFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private TiberStat _Stat;
    public TiberStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim { get { return _Anim; } }

    public Transform _AttackTransform;
    public MeshRenderer _MR;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<TiberStat>();
        _Anim = GetComponentInChildren<Animator>();

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        TiberState[] stateValues = (TiberState[])System.Enum.GetValues(typeof(TiberState));
        foreach (TiberState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Tiber" + s.ToString());
            TiberFSMState state = (TiberFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (TiberFSMState)gameObject.AddComponent(FSMType);
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

    public void SetState(TiberState newState)
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
        if (other.transform.tag == "Weapon")
        {

        }
    }

    public override void SetDeadState()
    {
        base.SetDeadState();
    }
}
