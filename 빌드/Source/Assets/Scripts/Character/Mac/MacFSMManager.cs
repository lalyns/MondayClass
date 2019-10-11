using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.UI;
using MC.Sound;
using UnityEngine.AI;

public enum MacState
{
    POPUP = 0,
    CHASE,
    ATTACK,
    SKILL,
    RUNAWAY,
    HIT,
    DEAD,
}

[RequireComponent(typeof(MacStat))]
public class MacFSMManager : FSMManager
{
    private bool _isInit = false;
    public MacState startState = MacState.POPUP;
    private Dictionary<MacState, MacFSMState> _States = new Dictionary<MacState, MacFSMState>();

    private MacState _CurrentState;
    public MacState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public MacFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private MacStat _Stat;
    public MacStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim { get { return _Anim; } }

    public Transform _AttackTransform;

    // Renderers
    public SkinnedMeshRenderer _MR;
    public List<Material> materialList = new List<Material>();

    public Slider _HPSilder;
    public HPBar _HPBar;
    public GameObject hitEffect;
    public GameObject hitEffect_Special;
    public GameObject hitEffect_Skill1;
    public GameObject hitEffect_Skill1_Special;
    public Transform hitLocation;

    public GameObject _PopupEffect;

    public MonsterSound _Sound;

    public Collider _PriorityTarget;
    public float _DetectingRange;

    public bool isDead = false;

    public bool KnockBackFlag;
    public float KnockBackDuration;
    public float KnockBackPower;
    public float KnockBackDelay;

    public AttackType CurrentAttackType = AttackType.NONE;

    public NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<MacStat>();
        _Anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<MonsterSound>();

        if (!GameManager.Instance.uIActive.monster)
            _HPBar.gameObject.SetActive(false);

        materialList.AddRange(_MR.materials);

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        MacState[] stateValues = (MacState[])System.Enum.GetValues(typeof(MacState));
        foreach (MacState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Mac" + s.ToString());
            MacFSMState state = (MacFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (MacFSMState)gameObject.AddComponent(FSMType);
            }

            _States.Add(s, state);
            state.enabled = false;
        }

        monsterType = MonsterType.Mac;

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    private void Start()
    {
        SetState(startState);
        _isInit = true;
    }

    public void SetState(MacState newState)
    {
        if (_isInit)
        {
            _States[_CurrentState].EndState();
            _States[_CurrentState].enabled = false;
        }
        _CurrentState = newState;
        _States[_CurrentState].BeginState();
        _States[_CurrentState].enabled = true;
        _Anim.SetInteger("CurrentState", (int)_CurrentState);
    }
    [HideInInspector]
    public bool isChange;
    private void Update()
    {
        if ((PlayerFSMManager.Instance.isSpecial || PlayerFSMManager.Instance.isSkill4) && !isChange)
        {
            SetState(MacState.HIT);
            isChange = true;
            return;
        }

        if (Stat.Hp <= 0)
        {
            SetDeadState();
        }
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        if (!isDead)
        {
            SetState(MacState.DEAD);
            isDead = true;
        }
    }
}
