using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.UI;
using MC.Sound;
using UnityEngine.AI;

public enum TiberState
{
    POPUP = 0,
    CHASE,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    HIT,
    DEAD,    
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

    private Rigidbody _RigidBody;
    public Rigidbody RigidBody { get { return _RigidBody; } }

    public Transform _AttackTransform;

    //렌더
    //public MeshRenderer _MR;
    //public List<Material> materialList = new List<Material>();


    public HPBar _HPBar;
    public Slider _HPSilder;
    public GameObject hitEffect;
    public GameObject hitEffect_Special;
    public GameObject hitEffect_Skill1;
    public GameObject hitEffect_Skill1_Special;
    public Transform hitLocation;

    public MonsterSound _Sound;

    public float _DetectingRange;

    public Collider _PriorityTarget;

    public bool KnockBackFlag;
    public int KnockBackDuration;
    public float KnockBackPower;
    public float KnockBackDelay;


    public bool isDead = false;

    public AttackType CurrentAttackType = AttackType.NONE;

    public GameObject Attack1Effect, Attack2Effect, Attack3Effect;
    public bool isAttack1, isAttack2;

    public NavMeshAgent agent;
    protected override void Awake()
    {
        base.Awake();
        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<TiberStat>();
        _Anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<MonsterSound>();
        _RigidBody = GetComponent<Rigidbody>();

        if (!GameManager.Instance.uIActive.monster)
            _HPBar.gameObject.SetActive(false);

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

        CC.detectCollisions = true;

        monsterType = MonsterType.Tiber;
        Attack1Effect.SetActive(false);
        Attack2Effect.SetActive(false);
        Attack3Effect.SetActive(false);

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
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
    //[HideInInspector]
    public bool isChange;
    private void Update()
    {
        if ((PlayerFSMManager.Instance.isSpecial || PlayerFSMManager.Instance.isSkill4) && !isChange)
        {
            SetState(TiberState.HIT);
            isChange = true;
            return;
        }

        if (Stat.Hp <= 0)
        {
            SetDeadState();
        }

        if (RigidBody.velocity.sqrMagnitude > 0) {
            RigidBody.velocity = Vector3.Lerp(RigidBody.velocity, Vector3.zero, 0.15f);

            if (RigidBody.velocity.sqrMagnitude <= 0.1f) {
                RigidBody.velocity = Vector3.zero;
            }
        }
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        if (!isDead)
        {
            SetState(TiberState.DEAD);
            isDead = true;
        }
    }
}
