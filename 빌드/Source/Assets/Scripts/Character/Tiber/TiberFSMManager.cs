using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.UI;
using MC.Sound;

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

    protected override void Awake()
    {
        base.Awake();
        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<TiberStat>();
        _Anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<MonsterSound>();

        //materialList.AddRange(_MR.materials);

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
    }

    public override void OnHitForMonster(AttackType attackType)
    {
        base.OnHitForMonster(attackType);

        if ((attackType == AttackType.ATTACK1
            || attackType == AttackType.ATTACK2
            || attackType == AttackType.ATTACK3)
            && ((int)CurrentAttackType & (int)attackType) != 0)
        {
            return;
        }

        if (CurrentState == TiberState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            EffectPoolManager._Instance._PlayerEffectPool[0].ItemSetActive(hitLocation, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            EffectPoolManager._Instance._PlayerEffectPool[1].ItemSetActive(hitLocation, "Effect");

        CurrentAttackType = attackType;
        int value = TransformTypeToInt(attackType);
        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

        Stat.TakeDamage(playerStat, playerStat.Str * playerStat.dmgCoefficient[value]);
        //SetKnockBack(playerStat, value);
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
        //if (attackType == AttackType.SKILL3)
        //    StartCoroutine(Shake.instance.ShakeCamera(0.01f, 0.01f, 0.01f));

        if (Stat.Hp > 0)
        {
            if (CurrentState == TiberState.HIT) return;

            SetState(TiberState.HIT);

            //플레이어 쳐다본 후
            try
            {
                transform.localEulerAngles = Vector3.zero;
                transform.LookAt(PlayerFSMManager.Instance.Anim.transform);
                //플레이어피버게이지증가?
            }
            catch
            {

            }
        }
        else
        {
            SetDeadState();
        }
    }

    public void AttackSupport()
    {
        _HPBar.HitBackFun();
    }

    //public void SetKnockBack(PlayerStat stat, int attackType)
    //{
    //    KnockBackFlag = stat.KnockBackFlag[attackType];
    //    KnockBackDuration = stat.KnockBackDuration[attackType];
    //    KnockBackPower = stat.KnockBackPower[attackType];
    //    KnockBackDelay = stat.KnockBackDelay[attackType];
    //}

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon" && !PlayerFSMManager.Instance.isSkill3)
        {
            if (Stat.Hp > 0)
                OnHitForMonster(PlayerFSMManager.Instance.attackType);
        }

        if (other.transform.tag == "Ball")
        {
            if (PlayerFSMManager.Instance.isNormal)
                Instantiate(hitEffect_Skill1, hitLocation.transform.position, Quaternion.identity);
            if (!PlayerFSMManager.Instance.isNormal)
                Instantiate(hitEffect_Skill1_Special, hitLocation.transform.position, Quaternion.identity);

            other.transform.gameObject.SetActive(false);

            if (Stat.Hp > 0)
            {
                //OnHit();
                OnHitForMonster(AttackType.SKILL1);
            }
        }
        if (other.transform.tag == "Skill2" && PlayerFSMManager.Instance.isSkill2)
        {
            StartCoroutine("Skill2Timer");

            SetState(TiberState.HIT);
        }
        if (other.transform.tag == "Weapon" && PlayerFSMManager.Instance.isSkill3)
        {
            StartCoroutine("Skill3Timer");
        }
    }


    public override IEnumerator Skill3Timer()
    {
        return base.Skill3Timer();
    }
    public override IEnumerator Skill2Timer()
    {
        return base.Skill2Timer();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Skill2")
        {
            if (Stat.Hp > 0)
            {
                try
                {
                    OnHitForMonster(AttackType.SKILL2);
                }
                catch
                {

                }
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
