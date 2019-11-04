using MC.Sound;
using MC.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum RedHatState
{
    POPUP = 0,
    CHASE,
    ATTACK,
    DASH,
    HIT,
    DEAD,
    DISSOLVE,
}


[RequireComponent(typeof(RedHatStat))]
public class RedHatFSMManager : FSMManager
{
    private bool isInit = false;
    public RedHatState startState = RedHatState.POPUP;
    private Dictionary<RedHatState, RedHatFSMState> states = new Dictionary<RedHatState, RedHatFSMState>();

    private RedHatState currentState;
    public RedHatState CurrentState {
        get {
            return currentState;
        }
    }

    public RedHatFSMState CurrentStateComponent {
        get {
            return states[currentState];
        }
    }

    private CharacterController cc;
    public CharacterController CC { get { return cc; } }

    private CapsuleCollider playerCapsule;
    public CapsuleCollider PlayerCapsule { get { return playerCapsule; } }

    private RedHatStat stat;
    public RedHatStat Stat { get { return stat; } }

    private Animator anim;
    public Animator Anim { get { return anim; } }

    private Rigidbody rigidBody;
    public Rigidbody RigidBody { get { return rigidBody; } }

    public Transform _AttackTransform;

    // Renderers
    public SkinnedMeshRenderer mr;
    public SkinnedMeshRenderer wpmr;
    public List<Material> materialList = new List<Material>();

    //public CharacterStat _lastAttack;

    public Transform hitLocation;

    public MonsterSound sound;

    public GameObject dashEffect;
    public GameObject dashEffect1;
    public GameObject dashEffect2;

    public float detectingRange;
    public Collider priorityTarget;

    //public CapsuleCollider Weapon_Collider;
    public bool isDead = false;

    public bool isNotChangeState = false;

    public AttackType CurrentAttackType = AttackType.NONE;

    public NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();

        cc = GetComponent<CharacterController>();
        stat = GetComponent<RedHatStat>();
        anim = GetComponentInChildren<Animator>();
        sound = GetComponent<MonsterSound>();
        rigidBody = GetComponent<Rigidbody>();
        sound = GetComponent<MonsterSound>();

        CC.detectCollisions = true;

        materialList.AddRange(mr.materials);
        materialList.AddRange(wpmr.materials);
        playerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        RedHatState[] stateValues = (RedHatState[])System.Enum.GetValues(typeof(RedHatState));
        foreach (RedHatState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("RedHat" + s.ToString());
            RedHatFSMState state = (RedHatFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (RedHatFSMState)gameObject.AddComponent(FSMType);
            }

            states.Add(s, state);
            state.enabled = false;
        }

        monsterType = MonsterType.RedHat;

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    private void Start()
    {
        SetState(startState);
        isInit = true;
    }

    public void SetState(RedHatState newState)
    {

        if (isInit)
        {
            states[currentState].enabled = false;
            states[currentState].EndState();
        }
        currentState = newState;
        states[currentState].BeginState();
        states[currentState].enabled = true;
        anim.SetInteger("CurrentState", (int)currentState);
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

        if (Stat.Hp <= 0 || PlayerFSMManager.Instance.isDead)
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
