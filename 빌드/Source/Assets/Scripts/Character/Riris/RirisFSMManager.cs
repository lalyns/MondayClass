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
    PATTERND,
    PATTERNEND,
    ULTIMATE,
    DEAD,
    HIT,
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
    public CapsuleCollider PlayerCapsule {
        get {
            if(_PlayerCapsule == null)
                _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
            return _PlayerCapsule;
        }
    }

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

    public bool isDead = false;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<RirisStat>();
        _Anim = GetComponentInChildren<Animator>();

        for(int i=0; i<MR.Length; i++)
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

            _States.Add(s, state);
            state.enabled = false;
        }
    }

    private void Start()
    {
        SetState(startState);
        _isInit = true;
    }
    public bool isChange;
    private void Update()
    {
        if ((PlayerFSMManager.Instance.isSpecial || PlayerFSMManager.Instance.isSkill4) && !isChange)
        {
            SetState(RirisState.HIT);
            isChange = true;
            return;
        }
        if (!PlayerFSMManager.Instance.isSpecial && !PlayerFSMManager.Instance.isSkill4)
        {
            isChange = false;
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

    public void SetState(RirisState newState)
    {
        //Debug.Log("New State : " + newState.ToString());

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
        _Anim.Play("Warp");
        Instantiate(missingEffect, this.Pevis.transform.position, Quaternion.identity);
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        SetState(RirisState.DEAD);
    }

}
