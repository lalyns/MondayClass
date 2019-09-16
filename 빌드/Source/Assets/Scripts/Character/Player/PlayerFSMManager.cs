using System.Collections;
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
    //public AudioSource musicPlayer;
    //public AudioClip _dashSound;
    //public AudioClip _attackSound;
    //public AudioClip _runSound;
    //public AudioClip _skill1Sound;

    public PlayerSound _Sound;

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

    public Image Skill1UI;
    Vector3 target;
    [SerializeField]
    float Skill1Timer1, Skill1Timer2;
    [Header("스킬1번 날라가는 속도,")]
    public float skill1Speed = 20f;
    [Header("스킬1번 날라가는 시간,")]
    public float skill1ShootTime = 2f;

    
    public bool isAttackOne, isAttackTwo, isAttackThree, isSkill2;

    public bool isSkill3;

    [HideInInspector]
    public float _attack1Time, _attack2Time, _attack3Time, _attackBack1, _attackBack2, _specialAnim, _skill2Time, _skill3Time;

    [Header("X축 마우스 감도")]
    public float mouseSpeed = 80f;

    float r_x = 0;
    [HideInInspector]
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
    SphereCollider SKill2_Sphere;
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
        _Sound = GetComponent<PlayerSound>();

        Attack_Capsule = GameObject.FindGameObjectWithTag("Weapon").GetComponent<CapsuleCollider>();
        Skill3_Capsule = Skill3_Start.GetComponent<CapsuleCollider>();
        SKill2_Sphere = Skill2_Start.GetComponent<SphereCollider>();
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

        _Sound.PlayAttackSFX();
        _Sound.PlayFootStepSFX();

        instance = this;
        isSkill2 = false;
        isInputLock = false;
        isSpecial = false;
        try
        {
            pc_Icon.gameObject.SetActive(true);
            sp_Icon.gameObject.SetActive(false);
            Skill3_Start.SetActive(false);
            Skill3_End.SetActive(false);
}
        catch
        {
            
        }
        randomShoot = new int[5];
       // musicPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetState(startState);
        _isinit = true;

        //Skill1UI = GameObject.Find("Skill1_CoolTime").GetComponent<Image>();
        Skill1UI.fillAmount = 1f;
        Skill1UI.gameObject.SetActive(false);
        _attack1Time = AnimationLength("PC_Anim_Attack_001") / 1.3f;
        _attack2Time = AnimationLength("PC_Anim_Attack_002") / 1.3f;
        _attack3Time = AnimationLength("PC_Anim_Attack_003_2") / 1.3f;
        _attackBack1 = AnimationLength("PC_Anim_Attack_Back_001") / 1.3f;
        _attackBack2 = AnimationLength("PC_Anim_Attack_Back_002") / 1.3f;
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
        Skill1Timer2 = 10f;
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
    [Header("스킬2번 현재 거리, 최소거리, 최대거리")]
    public float skill2_Distance;
    public float skill2_minDis;
    public float skill2_maxDis;

    public Transform Skill2_Parent;
    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFlash = true;
            isFlashStart = true;
            FlashPosition = new Vector3(_anim.transform.position.x, _anim.transform.position.y + 0.83f, _anim.transform.position.z);
            FlashEffect2.SetActive(false);
            SetState(PlayerState.RUN);

        }

        if (isFlash)
        {
            if(isNormal)
                Normal.SetActive(false);
            if (!isNormal)
                Special.SetActive(false);
            try
            {
                FlashEffect1.SetActive(true);
                FlashEffect1.transform.position = FlashPosition;
                FlashEffect2.transform.position = new Vector3(_anim.transform.position.x, _anim.transform.position.y + 0.83f, _anim.transform.position.z);
            }
            catch
            {

            }
            //isCantMove = true;
            flashTimer += Time.deltaTime;
            if (_h >= 0.01f && flashTimer <= 0.2f)
            {
                _anim.transform.Translate(Vector3.right * 20f * Time.deltaTime);
            }
            if (_h <= -0.01f && flashTimer <= 0.2f)
            {
                _anim.transform.Translate(Vector3.right * -20f * Time.deltaTime);
            }
            if (_h == 0 && _v >= 0 && flashTimer <= 0.2f)
            {
                _anim.transform.Translate(Vector3.forward * 20f * Time.deltaTime);
            }
            if (_h == 0 && _v <= -0.01f && flashTimer <= 0.2f)
            {
                _anim.transform.Translate(Vector3.forward * -20f * Time.deltaTime);
            }

            if (flashTimer >= 0.2f && flashTimer <= 0.23f)
            {
                FlashEffect2.SetActive(true);
                _Sound.PlayDashSFX();
                isCantMove = false;


            }
            if (flashTimer >= 0.3f)
            {
                if(isNormal)
                    Normal.SetActive(true);
                if (!isNormal)
                    Special.SetActive(true);

            }
            if (flashTimer >= 0.5f)
            {
                try
                {
                    FlashEffect1.SetActive(false);
                }
                catch
                {

                }
                isFlash = false;
                isAttackOne = false;
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


        Skill2Set();

        mainCamera.gameObject.SetActive(true);
        _anim.transform.Rotate(Vector3.up * mouseSpeed * Time.deltaTime * r_x);
        
        //Skill2_Start.transform.position = new Vector3(_anim.transform.position.x, _anim.transform.position.y, _anim.transform.position.z +skill2_Distance);
    }


    void Skill2Set()
    {
        //if (isSkill2)
        //    return;
        try
        {
            skill2_Distance = 14f / followCam.height;
        }
        catch
        {

        }

        if (skill2_Distance >= skill2_maxDis)
            skill2_Distance = skill2_maxDis;

        Skill2_Parent.localPosition = new Vector3(0, 0.18f, skill2_Distance);
    }

    private void Update()
    {
        //isNormal = Normal.activeSelf;
        try
        {
            isNormal = pc_Icon.gameObject.activeSelf;
        }
        catch
        {

        }

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
        
        Skill2();
        Skill3();
        

        if (isSkill3)
            return;
        
        Dash();

        Attack();


        if (isSkill2)
            return;
        if (isSpecial)
            return;


        if (Skill1_Amount <= 1)
        {
            Skill1Set(Skill1_Shoots);
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
                pc_Icon.gameObject.SetActive (false);
                sp_Icon.gameObject.SetActive(true);
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
                isAttackOne = false;
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

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttackOne)
        {
            isAttackOne = true;
            SetState(PlayerState.ATTACK1);
            attackCount++;
           // AudioManager.playSound(_attackSound, musicPlayer);
            return;
        }

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
    void Skill1Shoot(GameObject[] effects, List<GameObject> targets, int[] rands, float distance)
    {
        for(int i=0; i<5; i++)
        {
            if (Skill1_Amount < 2)
                return;

            if (Skill1_Amount >= i + 2)
            {
                if(Vector3.Distance(effects[i].transform.position, targets[rands[i]].transform.position) >= distance)
                    effects[i].transform.position = Vector3.MoveTowards(effects[i].transform.position, targets[rands[i]].transform.position, skill1Speed * Time.deltaTime);
            }
            
        }
    }
    void Skill1Return(GameObject[] effects, GameObject[] shoots, List<GameObject> target, int[] rands, float distance)
    {
        for (int i = 0; i < 5; i++)
        {
            if (Skill1_Amount < 2)
                return;

            if (Skill1_Amount >= i + 2)
            {
                if (Vector3.Distance(shoots[i].transform.position, target[rands[i]].transform.position) <= distance)
                {
                    //shoots[i].transform.position = effects[i].transform.position;
                    //shoots[i].SetActive(false);
                }
            }
        }
    }

    public void Skill1()
    {
        
        if (!isSkill1CTime)
        {
            // 공격 횟수가 3회 성공 시 스킬1의 이펙트 하나씩 켜진다 가정 후 작성.
            if (attackCount >= 2)
            {
                attackCount = 0;
                Skill1_Amount++;

                // Skill1Effeects[0].gameObject.SetActive(true);
                isBall = true;
            }
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

                // 주변 몬스터의 수를 파악 한 후에
                _monster = GameStatus._Instance.ActivedMonsterList;

                //_monster.AddRange(GameObject.FindGameObjectsWithTag("Monster"));

                if (_monster.Count == 0)
                    return;

                // 떠있는 구체 -> 날라가는 구체로 Active를 수정 후.
                Skill1Set(Skill1_Shoots);

                // 몬스터 수의 값을 랜덤함수 5개를 돌려서 배치 시킨 후.
                for (int i = 0; i < 5; i++)
                {
                    randomShoot[i] = Random.Range((int)0, (int)_monster.Count);
                }

                


             

                Skill1UI.gameObject.SetActive(true);



                // 스킬이 날라간다.
                isShoot = true;
                isSkill1CTime = true;
                isBall = false;
                _Sound.PlaySkill1SFX();
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

            // 날라가는 시간을 정해준 후에.
            Skill1Timer1 += Time.deltaTime;
            // 날린다
            Skill1Shoot(Skill1_Shoots, _monster, randomShoot, 0);

            Skill1Return(Skill1_Effects, Skill1_Shoots, _monster, randomShoot, 0.01f);

            if (Skill1Timer1 >= skill1ShootTime)
            {
                Skill1_Amount = 1;

                for (int i = 0; i < 5; i++)
                {
                    Skill1_Shoots[i].transform.position = Skill1_Effects[i].transform.position;
                }

                Skill1Timer1 = 0;
                isShoot = false;
                _monster.Clear();
            }
        }
        if (isSkill1CTime)
        {
            Skill1Timer2 -= Time.deltaTime;
            Skill1UI.fillAmount = Skill1Timer2 / 10f;
            if (Skill1Timer2 <= 0)
            {
                Skill1Timer2 = 10f;
                Skill1UI.fillAmount = 1f;
                Skill1UI.gameObject.SetActive(false);

                isSkill1CTime = false;
            }
        }
    }
    public void Skill3()
    {
        if (isSkill3)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isNormal)
                Normal.SetActive(true);
            if (!isNormal)
                Special.SetActive(true);

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
