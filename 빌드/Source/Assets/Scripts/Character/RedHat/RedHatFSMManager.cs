using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using MC.UI;
using MC.Sound;
using System.Collections;
using UnityEngine.AI;

public enum RedHatState
{
    POPUP = 0,
    CHASE,
    ATTACK,
    DASH,
    HIT,
    DEAD,
}


[RequireComponent(typeof(RedHatStat))]
public class RedHatFSMManager : FSMManager
{
    private bool _isInit = false;
    public RedHatState startState = RedHatState.POPUP;
    private Dictionary<RedHatState, RedHatFSMState> _States = new Dictionary<RedHatState, RedHatFSMState>();

    private RedHatState _CurrentState;
    public RedHatState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public RedHatFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private RedHatStat _Stat;
    public RedHatStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim { get { return _Anim; } }

    private Rigidbody _RigidBody;
    public Rigidbody RigidBody { get { return _RigidBody; } }

    public Transform _AttackTransform;

    // Renderers
    public SkinnedMeshRenderer _MR;
    public SkinnedMeshRenderer _WPMR;
    public List<Material> materialList = new List<Material>();

    //public CharacterStat _lastAttack;

    public HPBar _HPBar;
    public Slider _HPSilder;
    public GameObject hitEffect;
    public GameObject hitEffect_Special;
    public GameObject hitEffect_Skill1;
    public GameObject hitEffect_Skill1_Special;
    public Transform hitLocation;

    public MonsterSound _Sound;

    public GameObject dashEffect;
    public GameObject dashEffect1;
    public GameObject dashEffect2;

    public float _DetectingRange;

    public Collider _PriorityTarget;

    public bool KnockBackFlag;
    public int KnockBackDuration;
    public float KnockBackPower;
    public float KnockBackDelay;

    //public CapsuleCollider Weapon_Collider;
    public bool isDead = false;

    public bool isNotChangeState = false;

    public AttackType CurrentAttackType = AttackType.NONE;

    public NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<RedHatStat>();
        _Anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<MonsterSound>();
        _RigidBody = GetComponent<Rigidbody>();

        CC.detectCollisions = true;

        if (!GameManager.Instance.uIActive.monster)
            _HPBar.gameObject.SetActive(false);

        materialList.AddRange(_MR.materials);
        materialList.AddRange(_WPMR.materials);
       // Weapon_Collider = gameObject.GetComponentInChildren<CapsuleCollider>();
        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        RedHatState[] stateValues = (RedHatState[])System.Enum.GetValues(typeof(RedHatState));
        foreach (RedHatState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("RedHat" + s.ToString());
            RedHatFSMState state = (RedHatFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (RedHatFSMState)gameObject.AddComponent(FSMType);
            }

            _States.Add(s, state);
            state.enabled = false;
        }

        monsterType = MonsterType.RedHat;

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    private void Start()
    {
        SetState(startState);
        _isInit = true;
    }

    public void SetState(RedHatState newState)
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

    public bool isChange;
    private void Update()
    {
        if ((PlayerFSMManager.Instance.isSpecial 
            || PlayerFSMManager.Instance.isSkill4) && !isChange)
        {
            SetState(RedHatState.HIT);
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
            SetState(RedHatState.DEAD);
            isDead = true;
        }
    }

}
