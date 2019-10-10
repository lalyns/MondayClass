using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using MC.UI;
using MC.Sound;
using System.Collections;

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

    public float _DetectingRange;

    public Collider _PriorityTarget;

    public bool KnockBackFlag;
    public int KnockBackDuration;
    public float KnockBackPower;
    public float KnockBackDelay;

    //public CapsuleCollider Weapon_Collider;
    public bool isDead = false;

    public AttackType CurrentAttackType = AttackType.NONE;


    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<RedHatStat>();
        _Anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<MonsterSound>();

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
    //[HideInInspector]
    public bool isChange;
    private void Update()
    {
        if ((PlayerFSMManager.Instance.isSpecial || PlayerFSMManager.Instance.isSkill4) && !isChange)
        {
            SetState(RedHatState.HIT);
            isChange = true;
            return;
        }
    }

    //public override void OnHitForMonster(AttackType attackType)
    //{
    //    base.OnHitForMonster(attackType);

    //    if((attackType == AttackType.ATTACK1 
    //        || attackType == AttackType.ATTACK2 
    //        || attackType == AttackType.ATTACK3) 
    //        && ((int)CurrentAttackType & (int)attackType) != 0)
    //    {
    //        return;
    //    }

    //    if (CurrentState == RedHatState.DEAD) return;

    //    if (PlayerFSMManager.Instance.isNormal)
    //        EffectPoolManager._Instance._PlayerEffectPool[0].ItemSetActive(hitLocation, "Effect");

    //    if (!PlayerFSMManager.Instance.isNormal)
    //        EffectPoolManager._Instance._PlayerEffectPool[1].ItemSetActive(hitLocation, "Effect");

    //    CurrentAttackType = attackType;
    //    int value = GameLib.TransformTypeToInt(attackType);
    //    PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

    //    float damage = (playerStat.Str * playerStat.dmgCoefficient[value] * 0.01f) -Stat.Defense;
    //    CharacterStat.ProcessDamage(playerStat, Stat, damage);

    //    //SetKnockBack(playerStat, value);
    //    Invoke("AttackSupport", 0.5f);

    //    if (attackType == AttackType.ATTACK1)
    //        StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.15f, 0.1f));
    //    if (attackType == AttackType.ATTACK2)
    //        StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.18f, 0.1f));
    //    if (attackType == AttackType.ATTACK3)
    //        StartCoroutine(Shake.instance.ShakeCamera(0.1f, 0.3f, 0.1f));
    //    if (attackType == AttackType.SKILL1)
    //        StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.1f, 0.1f));
    //    if (attackType == AttackType.SKILL2)
    //        StartCoroutine(Shake.instance.ShakeCamera(0.15f, 0.1f, 0.1f));
    //    //if (attackType == AttackType.SKILL3)
    //    //    StartCoroutine(Shake.instance.ShakeCamera(0.01f, 0.01f, 0.01f));
        
    //    if (Stat.Hp > 0)
    //    {
    //        if (CurrentState == RedHatState.HIT) return;

    //        SetState(RedHatState.HIT);

    //        //플레이어 쳐다본 후
    //        //try
    //        //{
    //        //    transform.localEulerAngles = Vector3.zero;
    //        //    transform.LookAt(PlayerFSMManager.Instance.Anim.transform);
    //        //    //플레이어피버게이지증가?
    //        //}
    //        //catch
    //        //{

    //        //}
    //    }
    //    else
    //    {
    //        SetDeadState();
    //    }
    //}

    //public void AttackSupport()
    //{
    //    _HPBar.HitBackFun();
    //}

    ////public void SetKnockBack(PlayerStat stat, int attackType)
    ////{
    ////    KnockBackFlag = stat.KnockBackFlag[attackType];
    ////    KnockBackDuration = stat.KnockBackDuration[attackType];
    ////    KnockBackPower = stat.KnockBackPower[attackType];
    ////    KnockBackDelay = stat.KnockBackDelay[attackType];
    ////}

    //public int TransformTypeToInt(AttackType type)
    //{
    //    switch (type)
    //    {
    //        case AttackType.ATTACK1:
    //            return 0;

    //        case AttackType.ATTACK2:
    //            return 1;

    //        case AttackType.ATTACK3:
    //            return 2;

    //        case AttackType.SKILL1:
    //            return 3;

    //        case AttackType.SKILL2:
    //            return 4;

    //        case AttackType.SKILL3:
    //            return 5;

    //        case AttackType.SKILL4:
    //            return 6;

    //        default:
    //            return -1;
    //    }
    //}

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag == "Weapon" && !PlayerFSMManager.Instance.isSkill3)
    //    {
    //        if (Stat.Hp > 0)
    //            OnHitForMonster(PlayerFSMManager.Instance.attackType);
    //    }

    //    if (other.transform.tag == "Ball")
    //    {
    //        if (PlayerFSMManager.Instance.isNormal)                
    //            Instantiate(hitEffect_Skill1, hitLocation.transform.position, Quaternion.identity);
    //        if (!PlayerFSMManager.Instance.isNormal)
    //            Instantiate(hitEffect_Skill1_Special, hitLocation.transform.position, Quaternion.identity);

    //        other.transform.gameObject.SetActive(false);

    //        if (Stat.Hp > 0)
    //        {
    //            //OnHit();
    //            OnHitForMonster(AttackType.SKILL1);
    //        }
    //    }
    //    if (other.transform.tag == "Skill2" && PlayerFSMManager.Instance.isSkill2)
    //    {
    //        StartCoroutine("Skill2Timer");

    //        SetState(RedHatState.HIT);
    //    }
    //    if (other.transform.tag == "Weapon" && PlayerFSMManager.Instance.isSkill3)
    //    {
    //        StartCoroutine("Skill3Timer");
    //    }
    //}

    //public override IEnumerator Skill3Timer()
    //{
    //    return base.Skill3Timer();
    //}
    //public override IEnumerator Skill2Timer()
    //{
    //    return base.Skill2Timer();
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.tag == "Skill2")
    //    {
    //        if (Stat.Hp > 0)
    //        {
    //            try
    //            {
    //                OnHitForMonster(AttackType.SKILL2);
    //            }
    //            catch
    //            {

    //            }
    //        }
    //    }
    //}

    //public void AttackCheck()
    //{
    //    Weapon_Collider.gameObject.SetActive(true);
    //}

    //public void AttackCancel()
    //{
    //    Weapon_Collider.gameObject.SetActive(false);
    //}
    
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
