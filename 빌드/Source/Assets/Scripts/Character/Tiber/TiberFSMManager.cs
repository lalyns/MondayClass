using MC.Sound;
using MC.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum TiberState
{
    POPUP = 0,
    CHASE,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    HIT,
    DEAD,    
    DISSOLVE,
}

[RequireComponent(typeof(TiberStat))]
public class TiberFSMManager : FSMManager
{
    private bool isInit = false;
    public TiberState startState = TiberState.POPUP;
    private Dictionary<TiberState, TiberFSMState> states = new Dictionary<TiberState, TiberFSMState>();

    private TiberState currentState;
    public TiberState CurrentState {
        get {
            return currentState;
        }
    }

    public TiberFSMState CurrentStateComponent {
        get {
            return states[currentState];
        }
    }

    private CharacterController cc;
    public CharacterController CC { get { return cc; } }

    private CapsuleCollider playerCapsule;
    public CapsuleCollider PlayerCapsule { get { return playerCapsule; } }

    private TiberStat stat;
    public TiberStat Stat { get { return stat; } }

    private Animator anim;
    public Animator Anim { get { return anim; } }

    private Rigidbody rigidBody;
    public Rigidbody RigidBody { get { return rigidBody; } }

    public Transform attackTransform;

    //렌더
    public SkinnedMeshRenderer mr;
    public List<Material> materialList = new List<Material>();

    public Transform hitLocation;

    public MonsterSound sound;

    public float detectingRange;
    public Collider priorityTarget;

    public bool isDead = false;

    public AttackType CurrentAttackType = AttackType.NONE;

    public GameObject Attack1Effect, Attack2Effect, Attack3Effect;
    public bool isAttack1, isAttack2;

    public NavMeshAgent agent;
    public CapsuleCollider capsule;

    protected override void Awake()
    {
        base.Awake();
        cc = GetComponent<CharacterController>();
        stat = GetComponent<TiberStat>();
        anim = GetComponentInChildren<Animator>();
        sound = GetComponent<MonsterSound>();
        rigidBody = GetComponent<Rigidbody>();

        materialList.AddRange(mr.materials);

        playerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        TiberState[] stateValues = (TiberState[])System.Enum.GetValues(typeof(TiberState));
        foreach (TiberState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Tiber" + s.ToString());
            TiberFSMState state = (TiberFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (TiberFSMState)gameObject.AddComponent(FSMType);
            }

            states.Add(s, state);
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
        isInit = true;
    }

    public void SetState(TiberState newState)
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

        if (Stat.Hp <= 0 || PlayerFSMManager.Instance.isDead)
        {
            SetDeadState();
        }

        if (RigidBody.velocity.sqrMagnitude > 0) {
            RigidBody.velocity = Vector3.Lerp(RigidBody.velocity, Vector3.zero, 5f);

            if (RigidBody.velocity.sqrMagnitude <= 0.1f) {
                //RigidBody.velocity = Vector3.zero;
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
