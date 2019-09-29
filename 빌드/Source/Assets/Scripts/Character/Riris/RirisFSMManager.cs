using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RirisState
{
    POPUP = 0,
    PATTERNA,
    PATTERNB,
    PATTERNC,
    PATTERNEND,
}


[RequireComponent(typeof(RirisStat))]
public class RirisFSMManager : FSMManager
{
    private bool _isInit = false;
    public RirisState startState = RirisState.POPUP;
    private Dictionary<RirisState, RirisFSMState> _States = new Dictionary<RirisState, RirisFSMState>();

    private RirisState _CurrentState;
    public RirisState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public RirisFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private RirisStat _Stat;
    public RirisStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim {
        get {
            if(_Anim == null) { _Anim = GetComponentInChildren<Animator>(); }
            return _Anim;
        }
    }
    public static float RirithPatternALength;
    public static float WeaponPatternALength;

    public Transform BulletCenter;

    public Transform _Weapon;
    public Animator _WeaponAnimator;
    public Transform _WeaponCenter;

    [Range(0, 1)] public float[] _PhaseThreshold = new float[3];
    public int _Phase = 0;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<RirisStat>();
        _Anim = GetComponentInChildren<Animator>();

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        RirisState[] stateValues = (RirisState[])System.Enum.GetValues(typeof(RirisState));
        foreach (RirisState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Riris" + s.ToString());
            RirisFSMState state = (RirisFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (RirisFSMState)gameObject.AddComponent(FSMType);
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

    public void SetState(RirisState newState)
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
        _WeaponAnimator.SetInteger("CurrentState", (int)_CurrentState);
        
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
