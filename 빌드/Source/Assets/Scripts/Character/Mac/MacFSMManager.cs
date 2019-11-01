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
    DISSOLVE,
}

[RequireComponent(typeof(MacStat))]
public class MacFSMManager : FSMManager
{
    private bool isInit = false;
    public MacState startState = MacState.POPUP;
    private Dictionary<MacState, MacFSMState> states = new Dictionary<MacState, MacFSMState>();

    private MacState currentState;
    public MacState CurrentState {
        get {
            return currentState;
        }
    }

    public MacFSMState CurrentStateComponent {
        get {
            return states[currentState];
        }
    }

    private CharacterController cc;
    public CharacterController CC { get { return cc; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private MacStat stat;
    public MacStat Stat { get { return stat; } }

    private Animator anim;
    public Animator Anim { get { return anim; } }

    public Transform attackTransform;

    // Renderers
    public SkinnedMeshRenderer mr;
    public List<Material> materialList = new List<Material>();

    public Transform hitLocation;

    public GameObject popupEffect;

    public MonsterSound sound;

    public Collider priorityTarget;
    public float detectingRange;

    public bool isDead = false;

    public AttackType currentAttackType = AttackType.NONE;
    public NavMeshAgent agent;
    
    protected override void Awake()
    {
        base.Awake();

        cc = GetComponent<CharacterController>();
        stat = GetComponent<MacStat>();
        anim = GetComponentInChildren<Animator>();
        sound = GetComponent<MonsterSound>();

        CC.detectCollisions = true;

        materialList.AddRange(mr.materials);

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

            states.Add(s, state);
            state.enabled = false;
        }

        monsterType = MonsterType.Mac;

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    private void Start()
    {
        SetState(startState);
        isInit = true;
    }

    public void SetState(MacState newState)
    {
        if (isInit)
        {
            states[currentState].EndState();
            states[currentState].enabled = false;
        }
        currentState = newState;
        states[currentState].BeginState();
        states[currentState].enabled = true;
        anim.SetInteger("CurrentState", (int)currentState);
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

        if (Stat.Hp <= 0 || PlayerFSMManager.Instance.isDead)
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
