using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public bool KnockBackFlag;
    public float KnockBackDuration;
    public float KnockBackPower;
    public float KnockBackDelay;

    public AttackType CurrentAttackType = AttackType.NONE;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<MacStat>();
        _Anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<MonsterSound>();

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

        if (CurrentState == MacState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            EffectPoolManager._Instance._PlayerEffectPool[0].ItemSetActive(hitLocation, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            EffectPoolManager._Instance._PlayerEffectPool[1].ItemSetActive(hitLocation, "Effect");

        CurrentAttackType = attackType;
        int value = GameLib.TransformTypeToInt(attackType);

        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;
        Stat.TakeDamage(playerStat, playerStat.DMG[value]);
        SetKnockBack(playerStat, value);
        Invoke("AttackSupport", 0.5f);

        if (attackType == AttackType.ATTACK1 || attackType == AttackType.ATTACK2)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.05f, 0.1f));
        if (attackType == AttackType.ATTACK3)
            StartCoroutine(Shake.instance.ShakeCamera(0.2f, 0.2f, 0.1f));
        if(attackType == AttackType.SKILL1)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.1f, 0.1f));
        if(attackType == AttackType.SkILL2)
            StartCoroutine(Shake.instance.ShakeCamera(0.15f, 0.1f, 0.1f));
        if (attackType == AttackType.SKILL3)
            StartCoroutine(Shake.instance.ShakeCamera(0.01f, 0.01f, 0.01f));

        if (Stat.Hp > 0)
        {
            if (CurrentState == MacState.HIT) return;

            SetState(MacState.HIT);
            //플레이어 쳐다본 후
            transform.localEulerAngles = Vector3.zero;
            transform.LookAt(PlayerFSMManager.Instance.Anim.transform);
            // 뒤로 밀림
            transform.Translate(Vector3.back * 20f * Time.smoothDeltaTime, Space.Self);
            //플레이어피버게이지증가?
            //PlayerFSMManager.instance.FeverGauge++;
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

    public void SetKnockBack(PlayerStat stat,int attackType)
    {
        KnockBackFlag = stat.KnockBackFlag[attackType];
        KnockBackDuration = stat.KnockBackDuration[attackType];
        KnockBackPower = stat.KnockBackPower[attackType];
        KnockBackDelay = stat.KnockBackDelay[attackType];
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon" && !PlayerFSMManager.Instance.isSkill3)
        {
            if (Stat.Hp > 0)
                OnHitForMonster(PlayerFSMManager.Instance.attackType);

            if (_CurrentState == MacState.ATTACK)
            {              
            }
        }
        if (other.transform.tag == "Ball")
        {
            if (PlayerFSMManager.Instance.isNormal)
                EffectPoolManager._Instance._PlayerEffectPool[2].ItemSetActive(hitLocation, "Effect");
            else
                EffectPoolManager._Instance._PlayerEffectPool[3].ItemSetActive(hitLocation, "Effect");

            if (Stat.Hp > 0)
            {
                OnHitForMonster(AttackType.SKILL1);
                other.transform.gameObject.SetActive(false);
            }
        }
       
        if(other.transform.tag == "Skill2")
        {
            SetState(MacState.HIT);
        }
        if (other.transform.tag == "Weapon" && PlayerFSMManager.Instance.isSkill3)
        {
            StartCoroutine("Skill3Timer");
        }
    }
    private void OnTriggerStay(Collider other)
    {
     
    }

    IEnumerator Skill3Timer()
    {
        while (PlayerFSMManager.Instance.isSkill3)
        {
            OnHitForMonster(AttackType.SKILL3);
            yield return new WaitForSeconds(0.1f);
        }
    }




    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Skill2")
        {
            if (Stat.Hp > 0)
            {
                try
                {
                    OnHitForMonster(AttackType.SkILL2);
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

        SetState(MacState.DEAD);
    }
}
