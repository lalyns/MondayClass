using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;
using MC.UI;

public enum RirisState
{
    POPUP = 0,
    PATTERNA,
    PATTERNB,
    PATTERNC,
    PATTERND,
    PATTERNEND,
    ULTIMATE,
    DEAD,
    HIT,
    PHASE,
    DIALOG
}


[RequireComponent(typeof(RirisStat))]
public class RirisFSMManager : FSMManager
{
    private bool isInit = false;
    public RirisState startState = RirisState.POPUP;
    private Dictionary<RirisState, RirisFSMState> states = new Dictionary<RirisState, RirisFSMState>();

    public static RirisFSMManager Instance;

    private RirisState currentState;
    public RirisState CurrentState {
        get {
            return currentState;
        }
    }

    public RirisFSMState CurrentStateComponent {
        get {
            return states[currentState];
        }
    }

    private CharacterController cc;
    public CharacterController CC { get { return cc; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule => PlayerFSMManager.Instance.Anim.GetComponent<CapsuleCollider>();

    private RirisStat stat;
    public RirisStat Stat { get { return stat; } }

    private Animator anim;
    public Animator Anim {
        get {
            if(anim == null) { anim = GetComponentInChildren<Animator>(); }
            return anim;
        }
    }

    public Transform BulletCenter;
    public Transform Pevis;

    public Transform _Weapon;
    public Animator _WeaponAnimator;
    public Transform _WeaponCenter;

    public AttackType CurrentAttackType = AttackType.NONE;

    [Range(0, 1)] public float[] _PhaseThreshold = new float[3];
    public int _Phase = 0;

    public Transform hitTransform;

    public SkinnedMeshRenderer[] MR;
    public List<Material> materials;

    public GameObject missingEffect;
    public GameObject missingEndEffect;

    public RirisSound sound;
    public MonsterSound sound2;

    public bool isDead = false;

    protected override void Awake()
    {
        base.Awake();

        Instance = GetComponent<RirisFSMManager>();

        cc = GetComponent<CharacterController>();
        stat = GetComponent<RirisStat>();
        anim = GetComponentInChildren<Animator>();
        sound = GetComponent<RirisSound>();
        sound2 = GetComponent<MonsterSound>();

        for (int i=0; i<MR.Length; i++)
        {
            materials.AddRange(MR[i].materials);
        }

        RirisState[] stateValues = (RirisState[])System.Enum.GetValues(typeof(RirisState));
        foreach (RirisState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Riris" + s.ToString());
            RirisFSMState state = (RirisFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (RirisFSMState)gameObject.AddComponent(FSMType);
            }

            states.Add(s, state);
            state.enabled = false;
        }
    }

    private void Start()
    {
        SetState(startState);
        isInit = true;
    }
    public bool isChange, isUlt;

    private void Update()
    {
        if(GameStatus.currentGameState == CurrentGameState.Product)
        {
            SetState(RirisState.HIT);
            if (PlayerFSMManager.Instance.isSpecial && !isChange) isChange = true;
            if (PlayerFSMManager.Instance.isSkill4 && !isUlt) isUlt = true;
        }

        if(Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Stat.TakeDamage(PlayerFSMManager.Instance.Stat, 3300);
                Invoke("AttackSupport", 0.5f);
            }

        }

        if (!isDead && Stat.Hp <= 0)
        {
            SetDeadState();
            isDead = true;
        }
    }

    public void AttackSupport()
    {
        CanvasInfo.Instance.enemyHP.hpBar.HitBackFun();
    }

    public void SetState(RirisState newState)
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
        _WeaponAnimator.SetInteger("CurrentState", (int)currentState);
        
    }

    public void TelePortToPos(Vector3 pos)
    {
        anim.Play("Warp");
        Instantiate(missingEffect, this.Pevis.transform.position, Quaternion.identity);
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        SetState(RirisState.DEAD);
    }

}
