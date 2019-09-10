using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LilithState
{
    POPUP = 0,
    PATTERNA,
    PATTERNB,
    PATTERNC,
    PATTERNEND,
}


[RequireComponent(typeof(LilithStat))]
public class LilithFSMManager : FSMManager
{
    private bool _isInit = false;
    public LilithState startState = LilithState.POPUP;
    private Dictionary<LilithState, LilithFSMState> _States = new Dictionary<LilithState, LilithFSMState>();

    private LilithState _CurrentState;
    public LilithState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public LilithFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private LilithStat _Stat;
    public LilithStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim { get { return _Anim; } }

    public Transform BulletCenter;

    [Range(0, 1)] public float[] _PhaseThreshold = new float[3];
    public int _Phase = 0;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<LilithStat>();
        _Anim = GetComponentInChildren<Animator>();

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        LilithState[] stateValues = (LilithState[])System.Enum.GetValues(typeof(LilithState));
        foreach (LilithState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Lilith" + s.ToString());
            LilithFSMState state = (LilithFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (LilithFSMState)gameObject.AddComponent(FSMType);
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

    public void SetState(LilithState newState)
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

    public void TelePortToPos(Vector3 pos)
    {
        this.transform.position = pos;
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Weapon")
        {

        }
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

    }
}
