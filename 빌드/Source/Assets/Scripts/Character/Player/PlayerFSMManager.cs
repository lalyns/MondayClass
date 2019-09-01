﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerState
{
    IDLE = 0,
    RUN,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    ATTACKBACK1,
    ATTACKBACK2,
    TRANS,
    DEAD,
}

[RequireComponent(typeof(PlayerStat))]
[ExecuteInEditMode]
public class PlayerFSMManager : FSMManager
{
    private bool _onAttack = false;
    private bool _isinit = false;
    public PlayerState startState = PlayerState.IDLE;
    public Dictionary<PlayerState, FSMState> _states = new Dictionary<PlayerState, FSMState>();

    [HideInInspector]
    public CharacterStat _lastAttack;

    [SerializeField]
    private PlayerState _currentState;
    public PlayerState CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    public FSMState CurrentStateComponent
    {
        get { return _states[_currentState]; }
    }

    private CapsuleCollider _cc;
    public CapsuleCollider CC { get { return _cc; } }

    private PlayerStat _stat;
    public PlayerStat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    //public CharacterController testTarget;

    public bool isCantMove;
    float vertical, horizontal;
    public float attackCount;
    public GameObject[] Skill1Effeects;
    bool isBall, isShoot, isSkill1CTime;

    private List<GameObject> _monster = new List<GameObject>();

    Image Skill1UI;
    Vector3 target;
    int randomShoot;
    float Skill1Timer;
    [Header("스킬1번 날라가는 속도,")]
    public float skill1Speed = 20f;
    [Header("스킬1번 날라가는 시간,")]
    public float skill1ShootTime = 2f;

    public bool isAttackOne, isAttackTwo, isAttackThree;
    public float _attack1Time, _attack2Time, _attack3Time, _attackBack1, _attackBack2, _specialAnim;

    [Header("X축 마우스 감도")]
    public float mouseSpeed = 80f;

    float r_x = 0;

    public float _v, _h;

    bool isInputLock;
    protected override void Awake()
    {
        base.Awake();
        SetGizmoColor(Color.red);

        _cc = GetComponentInChildren<CapsuleCollider>();
        _stat = GetComponent<PlayerStat>();
        _anim = GetComponentInChildren<Animator>();
        Attack_Capsule = GameObject.FindGameObjectWithTag("Weapon").GetComponent<CapsuleCollider>();
        PlayerState[] stateValues = (PlayerState[])System.Enum.GetValues(typeof(PlayerState));
        foreach (PlayerState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Player" + s.ToString());
            FSMState state = (FSMState)GetComponent(FSMType);
            if(null == state)
            {
                state = (FSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }


        instance = this;

        isInputLock = false;
        isSpecial = false;
    }

    private void Start()
    {
        SetState(startState);
        _isinit = true;

        //Skill1UI = GameObject.Find("Skill1_CoolTime").GetComponent<Image>();
        //Skill1UI.fillAmount = 1f;
        //Skill1UI.gameObject.SetActive(false);
        _attack1Time = AnimationLength("PC_Attack_001");
        _attack2Time = AnimationLength("PC_Attack_002");
        _attack3Time = AnimationLength("PC_Attack_003_2");
        _attackBack1 = AnimationLength("PC_Attack_Back_001");
        _attackBack2 = AnimationLength("PC_Attack_Back_002");
        _specialAnim = AnimationLength("PC_Transform_001");
        isAttackOne = false;
        isAttackTwo = false;
        isAttackThree = false;
    }

    public void SetState(PlayerState newState)
    {
        if(_isinit)
        {
            _states[_currentState].enabled = false;
            _states[_currentState].EndState();
        }
        _currentState = newState;
        _states[_currentState].BeginState();
        _states[_currentState].enabled = true;
        _anim.SetInteger("CurrentState", (int)_currentState);     
    }

    // 움직이는지 체크하는 함수
    public bool OnMove()
    {
        return horizontal >= 0.01f || horizontal <= -0.01f ||
            vertical >= 0.01f || vertical <= -0.01f;
    }

    public void AttackCheck()
    {
        Attack_Capsule.enabled = true;
    }
    public void AttackCancle()
    {
        Attack_Capsule.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isSpecial)
            return;
        r_x = Input.GetAxis("Mouse X");
        _anim.transform.Rotate(Vector3.up * mouseSpeed * Time.deltaTime * r_x);
    }

    bool isSpecial;
    public GameObject Normal;
    public GameObject Special;
    public GameObject WeaponTransformEffect;
    public GameObject TimeLine;
    public GameObject Change_Effect;
    public float specialTimer = 0;
    public CapsuleCollider Attack_Capsule;
    private void Update()
    {
        // 공격처리는 죽음을 제외한 모든 상황에서 처리
        if (CurrentState != PlayerState.DEAD)
        {
            _onAttack = Input.GetAxis("Fire1") >= 0.01f ? true : false;
            //_anim.SetBool("OnAttack", _onAttack);
        }

        if (isInputLock)
            return;


        ChangeModel();
        GetInput();
        Skill1();
        AttackDirection();

        //Attack(isAttackOne);        

        if (Input.GetMouseButtonDown(0) && !isAttackOne)
        {
            isAttackOne = true;
            SetState(PlayerState.ATTACK1);
            return;
        }
    }

    public override void NotifyTargetKilled()
    {
        _lastAttack = null;
        SetState(PlayerState.IDLE);
    }

    public override void SetDeadState()
    {
        SetState(PlayerState.DEAD);
    }

    public override bool IsDie() { return CurrentState == PlayerState.DEAD; }



    public void AttackDirection()
    {
        
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");

        _anim.SetFloat("Attack_X", _h);
        _anim.SetFloat("Attack_Y", _v);
    }

    // 애니메이션 시간을 가져오는 함수.
    public float AnimationLength(string name)
    {
        float time = 0;

        RuntimeAnimatorController ac = _anim.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
            if (ac.animationClips[i].name == name)
                time = ac.animationClips[i].length;
        return time;
    }

    public void ChangeModel()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isSpecial = true;
            TimeLine.SetActive(true);
        }
        if (isSpecial)
        {
            WeaponTransformEffect.SetActive(true);
            specialTimer += Time.deltaTime;
            if (specialTimer >= 0.75f)
            {
                SetState(PlayerState.TRANS);
            }
            if (specialTimer >= 1.90f + 0.75f)
            {
                WeaponTransformEffect.SetActive(false);
                Normal.SetActive(false);
                Special.SetActive(true);
            }
            if (specialTimer >= _specialAnim + 1.3f)
            {
                Change_Effect.SetActive(false);
                SetState(PlayerState.IDLE);
            }
            if (specialTimer >= _specialAnim + 2f)
            {
                specialTimer = 0;
                TimeLine.SetActive(false);
                isSpecial = false;
                return;
            }
        }
    }

    public void GetInput()
    {        
        if (isCantMove)
        {
            vertical = 0;
            horizontal = 0;
        }
        
        if (!isCantMove)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }

        //if (vertical >= 0.1f && horizontal == 0)
        //{
        //    //전진애니메이션
        //    _anim.SetFloat("Direction_Y", vertical);
        //    _anim.SetFloat("Direction_X", 0);
        //}
        //else if (vertical <= -0.1f && horizontal == 0)
        //{
        //    _anim.SetFloat("Direction_Y", vertical);
        //    _anim.SetFloat("Direction_X", 0);
        //}
        //else if (horizontal >= 0.1f && vertical == 0)
        //{
        //    //오른쪽
        //    _anim.SetFloat("Direction_Y", 0);
        //    _anim.SetFloat("Direction_X", horizontal);
        //}
        //else if (horizontal <= -0.1f && vertical == 0)
        //{
        //    //왼쪽
        //    _anim.SetFloat("Direction_Y", 0);
        //    _anim.SetFloat("Direction_X", horizontal);
        //}
        if (!(horizontal == 0f && vertical == 0f))
        {
            _anim.SetFloat("Direction_Y", vertical);
            _anim.SetFloat("Direction_X", horizontal);
        }
    }  

    public void Skill1()
    {
        if(attackCount >= 10)
        {
            attackCount = 0;
            Skill1Effeects[0].gameObject.SetActive(true);
            isBall = true;
        }
        if (isBall)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                _monster.AddRange(GameObject.FindGameObjectsWithTag("Monster"));
                randomShoot = Random.Range((int)0, (int)_monster.Count + 1);
                Skill1Effeects[0].gameObject.SetActive(false);
                Skill1Effeects[1].gameObject.SetActive(true);
                isShoot = true;
                isBall = false;
                isSkill1CTime = true;
                Skill1UI.gameObject.SetActive(true);
            }
        }
        if (isShoot)
        {
            target = _monster[randomShoot].transform.position;
            Skill1Effeects[1].transform.position = Vector3.MoveTowards(Skill1Effeects[1].transform.position, target, skill1Speed * Time.deltaTime);
            Skill1Timer += Time.deltaTime;

            if (Skill1Timer > skill1ShootTime)
            {
                Skill1Effeects[1].transform.position = Skill1Effeects[0].transform.position;
                Skill1Effeects[1].SetActive(false);
                Skill1Timer = 0;
                isShoot = false;
                _monster.Clear();
            }
        }
        if (isSkill1CTime)
        {
            Skill1Timer -= Time.deltaTime;
            Skill1UI.fillAmount = Skill1Timer / 10f;
            if(Skill1Timer <= 0)
            {
                Skill1Timer = 10f;
                Skill1UI.fillAmount = 1f;
                Skill1UI.gameObject.SetActive(false);

                isSkill1CTime = false;
            }
        }
    }
    public static PlayerFSMManager instance;

  
}
