using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Timeline;

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
    public int SpecialGauge = 0;
    public Image SpecialGauge_Image;
    public Image Skill1UI, Skill2UI, Skill3UI;
    Vector3 target;
    [SerializeField]
    float Skill1Timer1, Skill1Timer2;
    [SerializeField]
    float Skill2CTime, Skill3CTime = 10f;
    [Header("스킬1번 날라가는 속도,")]
    public float skill1Speed = 20f;
    [Header("스킬1번 날라가는 시간,")]
    public float skill1ShootTime = 4f;

    
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
    PostProcessVolume volume;
    PostProcessLayer layer;
    PostProcessEffectSettings asdf;
    public Cinemachine.CinemachineVirtualCamera CMvcam2;

    
    public Shake shake;


    public GameObject[] Skill1_Effects;
    public GameObject[] Skill1_Shoots;
    public GameObject[] Skill1_Special_Effects;
    public GameObject[] Skill1_Special_Shoots;

    public int Skill1_Amount = 1;

    public int[] randomShoot;

    public GameObject Skill3_Start;
    public GameObject Skill3_End;

    public GameObject Skill2_Start;
    public GameObject FlashEffect1, FlashEffect2;
    public Vector3 FlashPosition;
    bool isFlashStart = false;
    [Header("스킬2번 현재 거리, 최소거리, 최대거리")]
    public float skill2_Distance;
    public float skill2_minDis;
    public float skill2_maxDis;

    public Transform Skill2_Parent;
    public Vignette vignette;
    public bool isShake = false;

    public bool isMouseYLock;
    Bloom bloom;

    public PostProcessProfile profile1;

    protected override void Awake()
    {
        base.Awake();
        SetGizmoColor(Color.red);
        
        _cc = GetComponentInChildren<CapsuleCollider>();
        _stat = GetComponent<PlayerStat>();
        _anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<PlayerSound>();
        
        CMvcam2 = GameObject.Find("CMvcam2").GetComponent<Cinemachine.CinemachineVirtualCamera>();

        GameObject.Find("mainCam").GetComponent<PostProcessVolume>().profile = profile1;
        vignette = GameObject.Find("mainCam").GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>();
        bloom = GameObject.Find("mainCam").GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>();
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
        isMouseYLock = false;
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
    VignetteModeParameter parameter;
    //public Texture2D aaasdf;
    public TextureParameter Concent;

    private void Start()
    {

        SetState(startState);
        _isinit = true;

        bloom.active = true;
        bloom.enabled.Override(true);
        
        bloom.enabled = new BoolParameter() { value = true, overrideState = false };

        //Concent.value = aaasdf;


        vignette.active = true;
        vignette.enabled.Override(true);
        vignette.enabled = new BoolParameter() { value = true, overrideState = true };        
        vignette.mode.overrideState = true;
        vignette.color.overrideState = true;
        vignette.mask.overrideState = true;
        vignette.opacity.overrideState = true;        
        parameter = vignette.mode;
        parameter.value = VignetteMode.Masked;
        vignette.color.value = new Color(102, 0, 153, 30);

        
        Debug.Log(Concent + "ㅋㅋ");
        Concent.overrideState = true;
        Concent.defaultState = TextureParameterDefault.White;


        //vignette.mask = Concent;
        //vignette.mask = Concent;
        //vignette.mask = Concent;
        Debug.Log(vignette.mask + "마스크3");
        //Concent.
        //  Debug.Log(Concent.value + "ㅋㅋㅋ");
        // Debug.Log(aaasdf + "z");
        vignette.mask.value = Concent.value;
        //vignette.mask.defaultState = false;
        //Concent.overrideState = true;
        //Concent.defaultState = TextureParameterDefault.White;
        //vignette.mask = new TextureParameter { value = null };
        //vignette.mask.overrideState = true;

        //vignette.mask.overrideState = true;
        vignette.opacity.value = 0f;




        //vignette.mask = Concent;
        //Concent.value = aaasdf;
        //vignette.mode = parameter.value;

        //vignette.mask = 
        //vignette.mode = mos;

        //BoolParameter tempbool = new BoolParameter
        //{
        //    value = true
        //};
        //vignette.enabled = tempbool;


        //Skill1UI = GameObject.Find("Skill1_CoolTime").GetComponent<Image>();
        try
        {
            Skill1UI.fillAmount = 1f;
            Skill1UI.gameObject.SetActive(false);
            Skill2UI.fillAmount = 1f;
            Skill2UI.gameObject.SetActive(false);
        }
        catch
        {

        }

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
            Skill1_Shoots[i].SetActive(false);
            Skill1_Special_Effects[i].SetActive(false);
            Skill1_Special_Shoots[i].SetActive(false);
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
        isShake = true;

        Attack_Capsule.enabled = true;
    }
    public void AttackCancel()
    {
        isShake = false;
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

    
    private void Update()
    {
        if(isSkill3)
            CMvcam2.m_Lens.FieldOfView = 70f;

        else
            CMvcam2.m_Lens.FieldOfView = 60f;

        
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

        if (isSpecial)
            return;


        GetInput();
        try
        {
            Skill1();
        }
        catch
        {

        }
        AttackDirection();
        
        Skill2();
        Skill3();

        try
        {
            if (SpecialGauge_Image.fillAmount <= 0)
                SpecialGauge_Image.fillAmount = 0;
            if (SpecialGauge_Image.fillAmount >= 1)
            {
                SpecialGauge_Image.fillAmount = 1;
            }
        }
        catch
        {

        }
        

        Attack();
        Dash();

        Skill3MouseLock();
        try { 
        if(isNormal)
            SpecialGauge_Image.fillAmount = SpecialGauge / 100f;
        }
        catch
        {

        }
        Skill3UIReset();
        if (!isNormal)
        {
            normalTimer -= Time.deltaTime;

            try
            {
                SpecialGauge_Image.fillAmount = (normalTimer * 3.33f) / 100f;
            }
            catch
            {

            }

            if (normalTimer <= 0f)
            {
                isNormal = true;
                ChangeNormal();
                normalTimer = 30;
                
            }
        }
        
        if (_monster.Count <= 0)
        {
           
            skillReturn(Skill1_Shoots, Skill1_Special_Shoots, isNormal);
            isShoot = false;

        }
        if (isSkill2)
            return;
    }
    float normalTimer = 30f;

    
    private void FixedUpdate()
    {
        if(isShake)
            StartCoroutine(shake.ShakeCamera(.2f, 0.02f, 0.0f));

        if (isSpecial)
            return;

        r_x = Input.GetAxis("Mouse X");


        Skill2Set();

        //mainCamera.gameObject.SetActive(true);
        _anim.transform.Rotate(Vector3.up * mouseSpeed * Time.deltaTime * r_x);

        //Skill2_Start.transform.position = new Vector3(_anim.transform.position.x, _anim.transform.position.y, _anim.transform.position.z +skill2_Distance);
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
    void ChangeNormal()
    {
        try
        {
            pc_Icon.gameObject.SetActive(true);
            sp_Icon.gameObject.SetActive(false);

            Normal.SetActive(true);
            Special.SetActive(false);            
        }
        catch
        {

        }
    }
    public void ChangeModel()
    {
        if (isNormal && SpecialGauge >=100)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                try
                {
                    pc_Icon.gameObject.SetActive(false);
                    sp_Icon.gameObject.SetActive(true);
                }
                catch
                {

                }
                isSpecial = true;
                TimeLine.SetActive(true);
                skillReturn(Skill1_Effects, Skill1_Special_Effects, isNormal);
            }
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


    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isShake = true;
            
            isFlash = true;
            isFlashStart = true;
            FlashPosition = new Vector3(_anim.transform.position.x, _anim.transform.position.y + 0.83f, _anim.transform.position.z);
            FlashEffect2.SetActive(false);
            SetState(PlayerState.RUN);
            if (isSkill3)
            {
                Skill3_End.transform.position = Skill3_Start.transform.position;
                Skill3_End.transform.rotation = Skill3_Start.transform.rotation;
                Skill3_End.SetActive(true);
            }
        }
        
        if (isFlash)
        {
            if (isNormal)
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
                try
                {
                    _Sound.PlayDashSFX();
                }
                catch
                {

                }
                isCantMove = false;


            }
            if (flashTimer >= 0.3f)
            {
                if (isNormal)
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
    }



    
    
    // 스킬 켜주고 꺼주고 하는 함수
    void Skill1Set(GameObject[] effects, GameObject[] effects_special, bool isnormal)
    {
        if (isnormal)
        {
            if (Skill1_Amount <= 1)
                for (int i = 0; i < 5; i++)
                {
                    effects[i].SetActive(false);
                }
            if (Skill1_Amount >= 2)
            {
                effects[0].SetActive(true);
                effects_special[0].SetActive(false);
            }
            if (Skill1_Amount >= 3)
            {
                effects[1].SetActive(true);
                effects_special[1].SetActive(false);
            }
            if (Skill1_Amount >= 4)
            {
                effects[2].SetActive(true);
                effects_special[2].SetActive(false);
            }
            if (Skill1_Amount >= 5)
            {
                effects[3].SetActive(true);
                effects_special[3].SetActive(false);
            }
            if (Skill1_Amount >= 6)
            {
                effects[4].SetActive(true);
                effects_special[4].SetActive(false);
            }
        }
        else
        {
            if (Skill1_Amount <= 1)
                for (int i = 0; i < 5; i++)
                {
                    effects_special[i].SetActive(false);
                }
            if (Skill1_Amount >= 2)
            {
                effects_special[0].SetActive(true);
                effects[0].SetActive(false);
            }
            if (Skill1_Amount >= 3)
            {
                effects_special[1].SetActive(true);
                effects[1].SetActive(false);
            }
            if (Skill1_Amount >= 4)
            {
                effects_special[2].SetActive(true);
                effects[2].SetActive(false);
            }
            if (Skill1_Amount >= 5)
            {
                effects_special[3].SetActive(true);
                effects[3].SetActive(false);
            }
            if (Skill1_Amount >= 6)
            {
                effects_special[4].SetActive(true);
                effects[4].SetActive(false);
            }
        }
       
    }
    void Skill1Shoot(GameObject[] effects, GameObject[] effects_special, List<GameObject> targets, int[] rands, float distance, bool isnormal)
    {
        for(int i=0; i<5; i++)
        {
            if (Skill1_Amount < 2)
                return;

            if (Skill1_Amount >= i + 2)
            {
                try
                {
                    if (isnormal)
                    {
                        if (Vector3.Distance(effects[i].transform.position, targets[rands[i]].transform.position) >= distance)
                            effects[i].transform.position = Vector3.MoveTowards(effects[i].transform.position, targets[rands[i]].transform.position, skill1Speed * Time.deltaTime);
                    }
                    else
                    {
                        if (Vector3.Distance(effects_special[i].transform.position, targets[rands[i]].transform.position) >= distance)
                            effects_special[i].transform.position = Vector3.MoveTowards(effects_special[i].transform.position, targets[rands[i]].transform.position, skill1Speed * Time.deltaTime);
                    }

                }
                catch
                {

                }
            }
            
        }
    }
    void skillReturn(GameObject[] effects, GameObject[] effects_special, bool isnormal)
    {
        if (isnormal)
        {
            for (int i = 0; i < 5; i++)
            {
                effects[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                effects_special[i].SetActive(false);
            }
        }

    }
    void Skill1PositionSet(GameObject[] normal_effect, GameObject[] normal_shoot, GameObject[] special_shoot, bool isnormal)
    {
        if (isnormal)
        {
            for (int i = 0; i < 5; i++)
                normal_shoot[i].transform.position = normal_effect[i].transform.position;
        }
        else
        {
            for (int i = 0; i < 5; i++)
                special_shoot[i].transform.position = normal_effect[i].transform.position;
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
        Skill1Set(Skill1_Effects, Skill1_Special_Effects, isNormal);

        // 구체가 1개 이상 있는 상태에서
        if (isBall)
        {
            // 1 누르면
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 주변 몬스터의 수를 파악 한 후에
                _monster = GameStatus.Instance.ActivedMonsterList;

                if (_monster.Count == 0)
                    return;

                // 떠있는 구체 -> 날라가는 구체로 Active를 수정 후.
                Skill1Set(Skill1_Shoots, Skill1_Special_Shoots, isNormal);

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
                try
                {
                    _Sound.PlaySkill1SFX();
                }
                catch
                {

                }
            }
        }
        // 스킬이 날라가기 시작하면
        if (isShoot)
        {

            skillReturn(Skill1_Effects, Skill1_Special_Effects, isNormal);

            // 날라가는 시간을 정해준 후에.
            Skill1Timer1 += Time.deltaTime;
            // 날린다
            Skill1Shoot(Skill1_Shoots, Skill1_Special_Shoots, _monster, randomShoot, 0, isNormal);

            if (_monster.Count == 0)
            {

                for (int i = 0; i < 5; i++)
                {
                    Skill1_Shoots[i].transform.position = Skill1_Effects[i].transform.position;
                }

                skillReturn(Skill1_Shoots, Skill1_Special_Shoots, isNormal);
                Skill1Timer1 = 0;
                isShoot = false;
                Skill1_Amount = 1;
            }

            if (Skill1Timer1 >= skill1ShootTime)
            {

                Skill1PositionSet(Skill1_Effects, Skill1_Shoots, Skill1_Special_Shoots, isNormal);

                Skill1Timer1 = 0;
                isShoot = false;

                Skill1_Amount = 1;

            }
        }
        if (isSkill1CTime)
        {
            SKill1UIReset();
        }
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
    public void Skill2()
    {
        if (isSkill2)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetState(PlayerState.SKILL2);
            isSkill2 = true;
            Skill2UI.gameObject.SetActive(true);
            return;
        }
    }

    void Skill3MouseLock()
    {
        if (isSkill3)
        {
            isMouseYLock = true;
            return;
        }
        else
            isMouseYLock = false;

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
    
    public void SKill1UIReset()
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
    

    public void Skill2UIReset()
    {
        Skill2CTime -= Time.deltaTime;
        Skill2UI.fillAmount = Skill2CTime / 10f;
        if(Skill2CTime <= 0)        
        {
            Skill2CTime = 10f;
              Skill2UI.fillAmount = 1f;
            Skill2UI.gameObject.SetActive(false);
            Skill2_Start.SetActive(false);
            isSkill2 = false;
        }
    }
    
    public void Skill3UIReset()
    {        
        if (Skill3_End.activeSelf)
        {
            Skill3UI.gameObject.SetActive(true);
            Skill3CTime -= Time.deltaTime;
            Skill3UI.fillAmount = Skill3CTime / 10f;

            if(Skill3CTime <= 0)
            {
                Skill3CTime = 10f;
                Skill3UI.fillAmount = 1f;
                Skill3UI.gameObject.SetActive(false);
                Skill3_End.SetActive(false);                
            }

        }
    }


    public static PlayerFSMManager instance;
}
