using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum DreamCatcherState
{
    POPUP = 0,
    CHASE,
    ATTACK,
    DEAD,
    DASH,
    HIT,
}


[RequireComponent(typeof(DreamCatcherStat))]
public class DreamCatcherFSMManager : FSMManager
{
    private bool _isInit = false;
    public DreamCatcherState startState = DreamCatcherState.POPUP;
    private Dictionary<DreamCatcherState, DreamCatcherFSMState> _States = new Dictionary<DreamCatcherState, DreamCatcherFSMState>();

    private DreamCatcherState _CurrentState;
    public DreamCatcherState CurrentState {
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
    public SkinnedMeshRenderer _MR;
    public LineRenderer _DashRoute;

    public CharacterStat _lastAttack;

    public Slider _HPSilder;

    public float HP = 100;
    public GameObject hitEffect;
    protected override void Awake()
    {
        base.Awake();

        HP = 10;
        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<DreamCatcherStat>();
        _Anim = GetComponentInChildren<Animator>();

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        DreamCatcherState[] stateValues = (DreamCatcherState[])System.Enum.GetValues(typeof(DreamCatcherState));
        foreach (DreamCatcherState s in stateValues)
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

    public void SetState(DreamCatcherState newState)
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

    public void OnHit()
    {
        Stat.TakeDamage(Stat, 400);

        //hp--;
        //카메라쉐이킹
        Shake.instance.ShakeCamera(0.3f, 0.3f, 0.7f);
        //hit스크립트로넘겨줌
        Instantiate(hitEffect, this.transform.position, Quaternion.identity);

        if (Stat.Hp > 0)
        {
            SetState(DreamCatcherState.HIT);

            //플레이어 쳐다본 후
            transform.localEulerAngles = Vector3.zero;
            transform.LookAt(InputHandler.instance.anim1.transform);
            // 뒤로 밀림
            transform.Translate(Vector3.back * 20f * Time.smoothDeltaTime, Space.Self);
            //플레이어피버게이지증가?
            InputHandler.instance.FeverGauge++;
        }
        else
        {
            SetDeadState();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {

            if (Stat.Hp > 0)
            {
                OnHit();
            }
            //ObjectManager.ReturnPoolMonster(this.gameObject, ObjectManager.MonsterType.DreamCatcher);        
        }
        if (other.transform.tag == "Ball")
        {
            if (Stat.Hp > 0)
            {
                OnHit();
            }
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Ball")
        {
            if (Stat.Hp > 0)
            {
                OnHit();
            }

            
        }
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        SetState(DreamCatcherState.DEAD);
    }

}
