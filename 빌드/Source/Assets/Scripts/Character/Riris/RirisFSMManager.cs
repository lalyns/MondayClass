using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public enum RirisState
{
    POPUP = 0,
    PATTERNA,
    PATTERNB,
    PATTERNC,
    PATTERNEND,
    DEAD,
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

    public HPBar hpBar;

    public AttackType CurrentAttackType = AttackType.NONE;

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

    private void Update()
    {
        HPUI();
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
        if(other.transform.tag == "Weapon" || other.transform.tag == "Skill2")
        {
            if (Stat.Hp > 0)
                OnHitForBoss(PlayerFSMManager.Instance.attackType);
        }
    }

    public void HPUI()
    {
        UserInterface.Instance.HPChangeEffect(Stat, hpBar);
    }

    public void OnHitForBoss(AttackType attackType)
    {
        Debug.Log(string.Format("Current Attack : {0}, Current HP: {1}, Current Phase: {2} ",
            attackType.ToString(), Stat.Hp, _Phase));

        if (CurrentState == RirisState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            EffectPoolManager._Instance._PlayerEffectPool[0].ItemSetActive(this.transform, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            EffectPoolManager._Instance._PlayerEffectPool[1].ItemSetActive(this.transform, "Effect");


        CurrentAttackType = attackType;
        int value = TransformTypeToInt(attackType);
        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

        Stat.TakeDamage(playerStat, playerStat.DMG[value]);
        Invoke("AttackSupport", 0.5f);

        if (attackType == AttackType.ATTACK1)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.15f, 0.1f));
        if (attackType == AttackType.ATTACK2)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.18f, 0.1f));
        if (attackType == AttackType.ATTACK3)
            StartCoroutine(Shake.instance.ShakeCamera(0.1f, 0.3f, 0.1f));
        if (attackType == AttackType.SKILL1)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.1f, 0.1f));
        if (attackType == AttackType.SKILL2)
            StartCoroutine(Shake.instance.ShakeCamera(0.15f, 0.1f, 0.1f));
    }

    public void AttackSupport()
    {
        hpBar.HitBackFun();
    }

    public int TransformTypeToInt(AttackType type)
    {
        switch (type)
        {
            case AttackType.ATTACK1:
                return 0;

            case AttackType.ATTACK2:
                return 1;

            case AttackType.ATTACK3:
                return 2;

            case AttackType.SKILL1:
                return 3;

            case AttackType.SKILL2:
                return 4;

            case AttackType.SKILL3:
                return 5;

            case AttackType.SKILL4:
                return 6;

            default:
                return -1;
        }
    }
    public override void SetDeadState()
    {
        base.SetDeadState();

    }

}
