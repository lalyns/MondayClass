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
    SKILL2,
    SKILL3,
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
    public bool isBall, isShoot, isSkill1CTime;

    [SerializeField]
    private List<GameObject> _monster = new List<GameObject>();

    Image Skill1UI;
    Vector3 target;
    [SerializeField]
    float Skill1Timer;
    [Header("스킬1번 날라가는 속도,")]
    public float skill1Speed = 20f;
    [Header("스킬1번 날라가는 시간,")]
    public float skill1ShootTime = 2f;

    public bool isAttackOne, isAttackTwo, isAttackThree, isSkill2, isSkill3;
    public float _attack1Time, _attack2Time, _attack3Time, _attackBack1, _attackBack2, _specialAnim, _skill2Time, _skill3Time;

    [Header("X축 마우스 감도")]
    public float mouseSpeed = 80f;

    float r_x = 0;

    public float _v, _h;

    bool isInputLock;

    public float flashTimer = 0;
    public bool isSpecial, isFlash;
    public GameObject Normal;
    public GameObject Special;
    public GameObject WeaponTransformEffect;
    public GameObject TimeLine;
    public GameObject Change_Effect;
    public float specialTimer = 0;
    CapsuleCollider Attack_Capsule;
    CapsuleCollider Skill3_Capsule;
    BoxCollider SKill2_Box;
    public Image pc_Icon, sp_Icon;
    [Header("플레이어가 변신상태인지 아닌지 확인시켜줌.")]
    public bool isNormal = false;

    CameraManager camManager;
    FollowCam followCam;
    Camera mainCamera;
    Shake shake;


    public GameObject[] Skill1_Effects;
    public GameObject[] Skill1_Shoots;
    public int Skill1_Amount = 1;

    public int[] randomShoot;

    public GameObject Skill3_Start;
    public GameObject Skill3_End;

    public GameObject Skill2_Start;
    protected override void Awake()
    {
        base.Awake();
        SetGizmoColor(Color.red);

        _cc = GetComponentInChildren<CapsuleCollider>();
        _stat = GetComponent<PlayerStat>();
        _anim = GetComponentInChildren<Animator>();
        Attack_Capsule = GameObject.FindGameObjectWithTag("Weapon").GetComponent<CapsuleCollider>();
        Skill3_Capsule = Skill3_Start.GetComponent<CapsuleCollider>();
        SKill2_Box = Skill2_Start.GetComponent<BoxCollider>();
        PlayerState[] stateValues = (PlayerState[])System.Enum.GetValues(typeof(PlayerState));
        foreach (PlayerState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Player" + s.ToString());
            FSMState state = (FSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (FSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }


        instance = this;
        isSkill2 = false;
        isInputLock = false;
        isSpecial = false;
        try
        {
            pc_Icon.enabled = true;
            sp_Icon.enabled = false;
            Skill3_Start.SetActive(false);
            Skill3_End.SetActive(false);
}
        catch
        {

        }
        randomShoot = new int[5];
    }

    private void Start()
    {
        SetState(startState);
        _isinit = true;

        //Skill1UI = GameObject.Find("Skill1_CoolTime").GetComponent<Image>();
        //Skill1UI.fillAmount = 1f;
        //Skill1UI.gameObject.SetActive(false);
        _attack1Time = AnimationLength("PC_Anim_Attack_001");
        _attack2Time = AnimationLength("PC_Anim_Attack_002");
        _attack3Time = AnimationLength("PC_Anim_Attack_003_2");
        _attackBack1 = AnimationLength("PC_Anim_Attack_Back_001");
        _attackBack2 = AnimationLength("PC_Anim_Attack_Back_002");
        _specialAnim = AnimationLength("PC_Anim_Transform_001");
        _skill2Time = AnimationLength("PC_Anim_Skill_002");
        _skill3Time = AnimationLength("PC_Anim_Skill_003");
        isAttackOne = false;
        isAttackTwo = false;
        isAttackThree = false;

        for (int i = 0; i < 5; i++)
        {
            Skill1_Effects[i].SetActive(false);
        }
        //camManager = CameraManager.singleton;
        //camManager.Init(this.transform);
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
        mainCamera = GameObject.Find("mainCam").GetComponent<Camera>();
        followCam = shake.GetComponent<FollowCam>();
    }
   
    public void SetState(PlayerState newState)
    {
        if (_isinit)
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
    public void AttackCancel()
    {
        Attack_Capsule.enabled = false;
    }
    public void Skill3Attack()
    {
        Skill3_Capsule.enabled = true;
    }
    public void Skill3Cancel()
    {
        Skill3_Capsule.enabled = false;
    }

    public GameObject FlashEffect1, FlashEffect2;
    public Vector3 FlashPosition;
    bool isFlashStart = false;
    float skill2_Distance;

    public Transform Skill2_Parent;
    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFlash = true;
            isFlashStart = true;
            FlashPosition = new Vector3(_anim.transform.position.x, _anim.transform.position.y + 0.83f, _anim.transform.position.z);
            FlashEffect2.SetActive(false);
        }

        if (isFlash)
        {
            Normal.SetActive(false);
            try
            {
                FlashEffect1.SetActive(true);
                FlashEffect1.transform.position = FlashPosition;
                FlashEffect2.transform.position = new Vector3(_anim.transform.position.x, _anim.transform.position.y + 0.83f, _anim.transform.position.z);
            }
            catch
            {

            }
            isCantMove = true;
            flashTimer += Time.deltaTime;
            if (_h >= 0.01f && flashTimer <= 0.2f)
            {
                _anim.transform.Translate(Vector3.right * 10f * Time.deltaTime);
            }
            if (_h <= -0.01f && flashTimer <= 0.2f)
            {
                _anim.transform.Translate(Vector3.right * -10f * Time.deltaTime);
            }
            if (_h == 0 && _v >= 0 && flashTimer <= 0.2f)
            {
                _anim.transform.Translate(Vector3.forward * 10f * Time.deltaTime);
            }
            if (_h == 0 && _v <= -0.01f && flashTimer <= 0.2f)
            {
                _anim.transform.Translate(Vector3.forward * -10f * Time.deltaTime);
            }

            if (flashTimer >= 0.2f && flashTimer <= 0.23f)
            {
                FlashEffect2.SetActive(true);

            }
            if (flashTimer >= 0.3f)
            {
                Normal.SetActive(true);

            }
            if (flashTimer >= 0.5f)
            {
                try
                {
                    FlashEffect1.SetActive(false);
                    isCantMove = false;

                }
                catch
                {

                }
                isFlash = false;
                flashTimer = 0;
                return;
            }
        }

        //    Normal.SetActive(false);
        //    isFlash = true;
        //    if (isFlash)
        //    {
        //        flashTimer += Time.deltaTime;
        //        isCantMove = true;
        //        if (_h >= 0.01)
        //        {
        //            //_anim.transform.Translate(Vector3.right * 10f * Time.deltaTime);
        //            //오른쪽으로 오지게 이동.
        //        }
        //        if (_h <= -0.01f)
        //        {
        //            //왼쪽으로 오지게 이동.
        //        }
        //        if (_h == 0 && _v >= 0)
        //        {
        //            //앞으로 오지게 이동
        //        }
        //        if (_h == 0 && _v <= -0.01)
        //        {
        //            //뒤로 오지게 이동
        //        }
        //        if (flashTimer >= 0.3f)
        //        {
        //            flashTimer = 0;
        //            Normal.SetActive(true);
        //            isFlash = false;
        //            isCantMove = false;
        //        }
        //    }
        //}
    }
    private void FixedUpdate()
    {
        if (isSpecial)
            return;
        r_x = Input.GetAxis("Mouse X");
        skill2_Distance = 14f / followCam.height;
        //if(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.V))
        //{
        //    camManager.Tick(Time.deltaTime);

        //    camManager.gameObject.SetActive(true);
        //    mainCamera.gameObject.SetActive(false);
        //}
        //else
        //{
        //            camManager.camInit(_anim.transform);
        mainCamera.gameObject.SetActive(true);
        //  camManager.gameObject.SetActive(false);
        _anim.transform.Rotate(Vector3.up * mouseSpeed * Time.deltaTime * r_x);
        //}
        
        //Skill2_Start.transform.position = new Vector3(_anim.transform.position.x, _anim.transform.position.y, _anim.transform.position.z +skill2_Distance);
        Skill2_Parent.localPosition = new Vector3(0, 0, skill2_Distance);
    }




    private void Update()
    {
        isNormal = Normal.activeSelf;

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
        Dash();
        Skill2();
        Skill3();
        //Attack(isAttackOne);        
        if (isSkill3)
            return;
        if (Input.GetMouseButtonDown(0) && !isAttackOne)
        {
            isAttackOne = true;
            SetState(PlayerState.ATTACK1);
            attackCount++;
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
            try
            {
                pc_Icon.enabled = false;
                sp_Icon.enabled = true;
            }
            catch
            {

            }
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

        _anim.SetFloat("Direction_Y", vertical);
        _anim.SetFloat("Direction_X", horizontal);
    }


    // 스킬 켜주고 꺼주고 하는 함수
    void Skill1Set(GameObject[] effects)
    {
        if (Skill1_Amount <= 1)
            for (int i = 0; i < 5; i++)
                effects[i].SetActive(false);
        if (Skill1_Amount >= 2)
            effects[0].SetActive(true);
        if (Skill1_Amount >= 3)
            effects[1].SetActive(true);
        if (Skill1_Amount >= 4)
            effects[2].SetActive(true);
        if (Skill1_Amount >= 5)
            effects[3].SetActive(true);
        if (Skill1_Amount >= 6)
            effects[4].SetActive(true);
    }
    void Skill1Shoot(GameObject[] effects, List<GameObject> targets, int[] rands)
    {
        for(int i=0; i<5; i++)
        {
            if(Skill1_Amount >= i + 2)
            {
                effects[i].transform.position = Vector3.MoveTowards(effects[i].transform.position, targets[rands[i]].transform.position, skill1Speed * Time.deltaTime);
            }
        }
    }
    void Skill1Return(GameObject[] effects, GameObject[] shoots, List<GameObject> target, int[] rands, float distance)
    {
        for (int i = 0; i < 5; i++)
        {
            if (Skill1_Amount >= i + 2)
            {
                if (Vector3.Distance(shoots[i].transform.position, target[rands[i]].transform.position) <= distance)
                {
                    //shoots[i].transform.position = effects[i].transform.position;
                    shoots[i].SetActive(false);
                }
            }
        }
    }

    public void Skill1()
    {
        // 공격 횟수가 3회 성공 시 스킬1의 이펙트 하나씩 켜진다 가정 후 작성.
        if (attackCount >= 3)
        {
            attackCount = 0;
            Skill1_Amount++;

            // Skill1Effeects[0].gameObject.SetActive(true);
            isBall = true;
        }
        // 변신 전은 구체 3개
        if (isNormal)
        {
            if (Skill1_Amount >= 4)
                Skill1_Amount = 4;
        }
        // 변신 후는 구체 5개
        if (!isNormal)
        {
            if (Skill1_Amount >= 6)
                Skill1_Amount = 6;
        }
        // 만약 구체 갯수가 0개면 다 꺼줌.
        Skill1Set(Skill1_Effects);

        // 구체가 1개 이상 있는 상태에서
        if (isBall)
        {
            // 1 누르면
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 떠있는 구체 -> 날라가는 구체로 Active를 수정 후.
                Skill1Set(Skill1_Shoots);

                // 주변 몬스터의 수를 파악 한 후에
                _monster.AddRange(GameObject.FindGameObjectsWithTag("Monster"));
                
                // 몬스터 수의 값을 랜덤함수 5개를 돌려서 배치 시킨 후.
                for (int i=0; i<5; i++)
                {
                    randomShoot[i] = Random.Range((int)0, (int)_monster.Count);
                }               

                // 스킬이 날라간다.
                isShoot = true;
                isSkill1CTime = true;
                isBall = false;

                //Skill1UI.gameObject.SetActive(true);
            }
        }
        // 스킬이 날라가기 시작하면
        if (isShoot)
        {
            // 기존 떠있던 이펙트의 Active를 꺼주고.
            for (int i = 0; i < 5; i++)
            {
                Skill1_Effects[i].SetActive(false);
            }

            //target = _monster[0].transform.position;
            // 날라가는 시간을 정해준 후에.
            Skill1Timer += Time.deltaTime;
            // 날린다
            Skill1Shoot(Skill1_Shoots, _monster, randomShoot);

            Skill1Return(Skill1_Effects, Skill1_Shoots, _monster, randomShoot, 0.02f);

            if (Skill1Timer > skill1ShootTime)
            {
                for (int i = 0; i < 5; i++)
                {
                    Skill1_Shoots[i].transform.position = Skill1_Effects[i].transform.position;
                }

                Skill1_Amount = 1;
                Skill1Timer = 0;
                isShoot = false;
                _monster.Clear();
            }
        }
        //if (isSkill1CTime)
        //{
        //    Skill1Timer -= Time.deltaTime;
        //    //  Skill1UI.fillAmount = Skill1Timer / 10f;
        //    if (Skill1Timer <= 0)
        //    {
        //        Skill1Timer = 10f;
        //        //Skill1UI.fillAmount = 1f;
        //        //Skill1UI.gameObject.SetActive(false);

        //        isSkill1CTime = false;
        //    }
        //}
    }
    public void Skill3()
    {
        if (isSkill3)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetState(PlayerState.SKILL3);
            isSkill3 = true;
            return;
        }
    }
    public void Skill2()
    {
        if (isSkill2)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetState(PlayerState.SKILL2);
            isSkill2 = true;
            return;
        }
    }
    public static PlayerFSMManager instance;


}
