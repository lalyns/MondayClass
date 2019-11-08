using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

using MC.UI;
using MC.Sound;
using MC.SceneDirector;
using MC.Mission;
public enum PlayerState
{
    IDLE = 0,
    RUN,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    ATTACKBACK1,
    ATTACKBACK2,
    SKILL2,
    SKILL3,
    TRANS,
    TRANS2,
    SKILL4,
    HIT,
    HIT2,
    DEAD,
    IDLE2,
    CLEAR,
}
public enum AttackType
{
    NONE = 0,
    ATTACK1 = 1,
    ATTACK2 = 1 << 2,
    ATTACK3 = 1 << 3,
    SKILL1 = 1 << 4,
    SKILL2 = 1 << 5,
    SKILL3 = 1 << 6,
    SKILL4 = 1 << 7,
}

[RequireComponent(typeof(PlayerStat))]
public class PlayerFSMManager : FSMManager
{
    public PlayerSound _Sound;
    private static PlayerFSMManager instance;
    public static PlayerFSMManager Instance
    {
        get
        {
            if (instance == null && MCSceneManager.currentScene != MCSceneManager.TITLE)
                instance = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerFSMManager>();
            return instance;
        }
    }


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
    public float vertical, horizontal;
    public float attackCount;
    public GameObject[] Skill1Effeects;
    public bool isBall, isShoot;
    public bool isSkill1CTime = true, isSkill2CTime = true, isSkill3CTime = true, isSkill4CTime = true;

    public AttackType attackType;

    [SerializeField]
    private List<GameObject> _monster = new List<GameObject>();
    public float SpecialGauge = 0;
    Vector3 target;
    [SerializeField]
    public float Skill1Timer1, Skill1CTime;
    [SerializeField]
    public float Skill2CTime, Skill3CTime = 10f, Skill4CTime = 10f;
    [Header("스킬1번 날라가는 속도,")]
    public float skill1Speed = 40f;
    [Header("스킬1번 날라가는 시간,")]
    public float skill1ShootTime = 4f;


    public bool isAttackOne, isAttackTwo, isAttackThree, isSkill2;

    public bool isHardAttack;
    public bool isSkill3, isSkill4;

    //[HideInInspector]
    public float _attack1Time, _attack2Time, _attack3Time, _attackBack1, _attackBack2, _specialAnim, _skill2Time, _skill3Time;

    [Header("X축 마우스 감도")]
    public float mouseSpeed = 80f;
    public float Skill3MouseSpeed = 10f;

    float r_x = 0;
    // [HideInInspector]
    public float _v, _h;

    public bool isInputLock;

    public float flashTimer = 0;
    public bool isSpecial, isFlash;
    public bool isInvincibility = false;
    public void SetInvincibility(bool value)
    {
        isInvincibility = value;
    }
    public bool isSkill3Dash = false;
    public bool isSkill2Dash = false;
    public GameObject Normal;
    public GameObject Special;
    public GameObject WeaponTransformEffect;
    public GameObject TimeLine, TimeLine2;
    public GameObject ClearTimeLine, ClearTimeLine2;
    public GameObject Change_Effect;
    public float specialTimer = 0;
    CapsuleCollider Attack_Capsule;
    [HideInInspector]
    public CapsuleCollider Skill3_Capsule;
    [Header("플레이어가 변신상태인지 아닌지 확인시켜줌.")]
    public bool isNormal = false;

    CameraManager camManager;
    public FollowCam followCam;
    [HideInInspector]
    public Camera mainCamera;
    PostProcessVolume volume;
    PostProcessLayer layer;
    PostProcessEffectSettings asdf;
    public Cinemachine.CinemachineVirtualCamera CMvcam2;


    public Shake shake;

    public Skill1Shoots Skill1Shoots;
    public GameObject[] Skill1_Effects;
    public GameObject[] Skill1_Shoots;
    public GameObject[] Skill1_Special_Effects;
    public GameObject[] Skill1_Special_Shoots;

    public int Skill1_Amount = 1;

    public int[] randomShoot;

    public GameObject Skill3_Start;
    public GameObject Skill3_End;

    public GameObject Skill2_Normal, Skill2_Special;
    public GameObject FlashEffect1, FlashEffect2;
    public Vector3 FlashPosition;
    [Header("스킬2번 현재 거리, 최소거리, 최대거리")]
    public float skill2_Distance;
    public float skill2_minDis;
    public float skill2_maxDis;

    public Transform Skill2_Parent;
    public Vignette vignette;
    public ColorGrading colorGrading;
    public bool isShake = false;

    public bool isMouseYLock;
    Bloom bloom;

    public bool isIDLE;

    public Rigidbody rigid;

    public SkinnedMeshRenderer[] _MR;
    public List<Material> materialList = new List<Material>();

    public List<Transform> Seats = new List<Transform>();

    VignetteModeParameter parameter;
    //public Texture2D aaasdf;
    public TextureParameter Concent;

    float normalTimer;
    float gaugePerSecond;

    public int ShieldCount;
    [HideInInspector] public bool isSpecialIDLE = false;
    [HideInInspector] public bool isHit2 = false;
    public int CurrentIdle;
    public int CurrentClear;

    public List<GameObject> Shields = new List<GameObject>();

    public bool isSkill1Upgrade = false;

    public int Skill1BounceCount = 1;

    public List<GameObject> UltimateEffect = new List<GameObject>();

    public bool isCanUltimate = false;

    public EnemyHPBar enemyHPBar;
    public MissionTutorial mission;
    protected override void Awake()
    {
        base.Awake();

        
        SetGizmoColor(Color.red);

        _cc = GetComponentInChildren<CapsuleCollider>();
        _stat = GetComponent<PlayerStat>();
        _anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<PlayerSound>();
        rigid = GetComponent<Rigidbody>();

        CMvcam2 = GameObject.Find("CMvcam2").GetComponent<Cinemachine.CinemachineVirtualCamera>();

        vignette = GameObject.Find("mainCam").GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>();
        colorGrading = GameObject.Find("mainCam").GetComponent<PostProcessVolume>().profile.GetSetting<ColorGrading>();

        bloom = GameObject.Find("mainCam").GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>();
        Attack_Capsule = GameObject.FindGameObjectWithTag("Weapon").GetComponent<CapsuleCollider>();
        Skill3_Capsule = Skill3_Start.GetComponent<CapsuleCollider>();
        enemyHPBar = GameObject.Find("EnemyHPBar").GetComponent<EnemyHPBar>();
        Skill1Shoots.gameObject.SetActive(false);
        instance = this;
        isSkill2 = false;
        isInputLock = false;
        isSpecial = false;
        isMouseYLock = false;
        try
        {
            Skill3_Start.SetActive(false);
            Skill3_End.SetActive(false);
        }
        catch
        {

        }
        randomShoot = new int[5];

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

        for (int x = 0; x < _MR.Length; x++)
            materialList.AddRange(_MR[x].materials);
        remainingDash = 3;

        try
        {
            mission = GameObject.Find("Tutorial").GetComponent<MissionTutorial>();
        }
        catch
        {
            mission = null;
        }
    }

    
    //public float clip1, clip2;
    private void Start()
    {

        SetState(startState);
        _isinit = true;

        bloom.active = true;
        bloom.enabled.Override(true);
        bloom.enabled = new BoolParameter() { value = true, overrideState = false };


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
        Concent.overrideState = true;
        Concent.defaultState = TextureParameterDefault.White;

        vignette.mask.value = Concent.value;
        vignette.opacity.value = 0f;

        normalTimer = Stat.transDuration;
        gaugePerSecond = 100.0f / normalTimer;



        //clip1 = AnimationClipChange("PC_Anim_Attack_003_2");

        _attack1Time = AnimationLength("PC_Anim_Attack_001") / 1.5f;
        _attack2Time = AnimationLength("PC_Anim_Attack_002") / 1.8f;
        _attack3Time = AnimationLength("PC_Anim_Attack_003_2") / 1.5f;
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

        Skill1CTime = Stat.skillCTime[0];
        Skill2CTime = Stat.skillCTime[1];
        Skill3CTime = Stat.skillCTime[2];

        for (int i = 0; i < Shields.Count; i++)
        {
            Shields[i].SetActive(false);
        }

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


    public bool OnMove()
    {
        return horizontal >= 0.01f || horizontal <= -0.01f ||
            vertical >= 0.01f || vertical <= -0.01f;
    }

    float footPeriod = 0.0f;
    private void Update()
    {
        if (GameStatus.currentGameState == CurrentGameState.Dialog || !GameStatus.Instance.canInput) return;

        SetInvincibility(GameStatus.currentGameState == CurrentGameState.Product);

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    StartCoroutine(shake.ShakeUI(0.2f, 4f, 3f));
        //}

        if (isInputLock || isDead)
            return;

        if(Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKey(KeyCode.D))
        {
            SetDeadState();
        }
        //if (GameSetting.rewardAbillity.feverGauge)
        //{
        //    SpecialGauge = 100;
        //    //GameSetting.rewardAbillity.feverGauge = false;
        //}

        if (_v != 0 || _h != 0)
        {
            if (_v < 0 && _h == 0)
            {
                if (footPeriod >= 0.35f)
                {
                    footPeriod = 0f;
                    _Sound.sfx.PlayPlayerSFX(gameObject, _Sound.sfx.footstepSFX);
                }
            }
            else
            {
                if (footPeriod >= 0.25f)
                {
                    footPeriod = 0f;
                    _Sound.sfx.PlayPlayerSFX(gameObject, _Sound.sfx.footstepSFX);
                }
            }
            footPeriod += Time.deltaTime;
        }

        if (CurrentState == PlayerState.IDLE) footPeriod = 0.0f;

        if (Stat.Hp <= 0)
        {
            _h = 0;
            _v = 0;
            SetDeadState();
            isInputLock = true;
        }


        // Fade In Out 시 적용됨.
        if (!GameManager.Instance.CharacterControl)
        {
            SetState(PlayerState.IDLE);
            return;
        }

        if (mission != null)
        {
            if (mission.currentTutorial == TutorialEvent.Transform)
            {
                ChangeModel();
                Skill1();
                Skill2();
                Skill3();
                Skill3MouseLock();
                Skill3Reset();
            }
        }
        if (mission == null)
            ChangeModel();

        if (isSpecial || isSkill4)
            return;
        if (remainingDash > 0 && !isSkill3Dash && !isSkill2Dash)            
            Dash();
        if(!isSkill2Dash)
            GetInput();
        //if (isSpecialIDLE)
        //    return;
        if (CurrentState == PlayerState.CLEAR || CurrentState == PlayerState.DEAD)
            return;

        
        AttackDirection();

        if (!isSkill2End && !isSkill3 && !isSkill2Dash && !isHit2)
            Attack();

        // if 튜토리얼 스킬 1번 사용해야 할 때라면
        if (mission != null)
        {

            if (mission.currentTutorial == TutorialEvent.Skill1)
            {
                Skill1();
                return;
            }

            if (mission.currentTutorial == TutorialEvent.Skill2)
            {
                Skill1();
                Skill2();
                return;
            }

            // if 스킬3번 사용
            if (mission.currentTutorial == TutorialEvent.Skill3)
            {
                Skill1();
                Skill2();
                Skill3();
                Skill3MouseLock();
                Skill3Reset();
                return;
            }


        }
        if (mission == null)
        {
            Skill1();
            Skill2();
            Skill3();
            Skill3MouseLock();
            Skill3Reset();
        }
        // if 궁극기 사용
        Skill4();





        //_anim.SetFloat("CurrentIdle", (int)CurrentIdle);
        //_anim.SetFloat("CurrentClear", (int)CurrentClear);
        Debug.Log(Stat.skillCTime[0] + "," + Stat.skillCTime[1] + ", " + Stat.skillCTime[2] + "스킬쿨타임들");
        Debug.Log(Skill1CTime +","+ Skill2CTime + ","+ Skill3CTime + "스킬쿨타임skill1ctime");
        if (isNormal)
        {
            if (isSkillTimeSet)
            {
                Skill1CTime = 5;
                Skill2CTime = 10;
                Skill3CTime = 15;
                isSkillTimeSet = false;
            }

            _anim.SetFloat("Normal", 0);
            Stat.skillCTime[0] = 5f;
            Stat.skillCTime[1] = 10f;
            Stat.skillCTime[2] = 15f;
            Stat.StrSet(30);
        }
        else if (!isNormal)
        {
            if (!isSkillTimeSet)
            {
                Skill1CTime = 2;
                Skill2CTime = 5;
                Skill3CTime = 7;
                isSkillTimeSet = true;
            }
            _anim.SetFloat("Normal", 1f);
            Stat.skillCTime[0] = 2f;
            Stat.skillCTime[1] = 5f;
            Stat.skillCTime[2] = 7f;
            Stat.StrSet(40);
        }
        if (!isNormal && !isSkill4)
        {
            normalTimer -= Time.deltaTime;
            SpecialGauge -= gaugePerSecond * Time.deltaTime;

            if (SpecialGauge <= 0f)
            {
                isNormal = true;
                ChangeNormal();
                SpecialGauge = 0;
                normalTimer = Stat.transDuration;

            }
        }

        if (_monster.Count <= 0)
        {

            Skill1Return(Skill1_Shoots, Skill1_Special_Shoots, isNormal);
            isShoot = false;

        }

        if (ShieldCount <= 0)
        {
            for (int i = 0; i < 3; i++)
                Shields[i].SetActive(false);
        }
        if (ShieldCount == 1)
        {
            Shields[0].SetActive(true);
            Shields[1].SetActive(false);
            Shields[2].SetActive(false);
        }
        if (ShieldCount == 2)
        {
            Shields[0].SetActive(false);
            Shields[1].SetActive(true);
            Shields[2].SetActive(false);
        }
        if (ShieldCount == 3)
        {
            Shields[0].SetActive(false);
            Shields[1].SetActive(false);
            Shields[2].SetActive(true);           
        }

        if (!isSpecial && SpecialGauge >=100 && isNormal && !isSpecialIDLE)
        {
            UltimateEffect[0].SetActive(true);
        }
        if (!isNormal && !isSkill4 && !isSpecialIDLE)
        {
            UltimateEffect[0].SetActive(false);
            UltimateEffect[1].SetActive(true);
        }
        if (isSkill4)
        {
            UltimateEffect[1].SetActive(false);
        }
        if (isSpecialIDLE || ClearTimeLine.activeSelf || ClearTimeLine2.activeSelf || isDead)
        {
            UltimateEffect[0].SetActive(false);
            UltimateEffect[1].SetActive(false);
        }
        
    }

    public void SetSpecialGauge()
    {
        SpecialGauge = 100;
    }

    private void FixedUpdate()
    {
        Skill2Set();

        r_x = Input.GetAxis("Mouse X");

        if (GameManager.Instance.CharacterControl && !isSpecial && !isSkill4 && !isDead && GameStatus.currentGameState != CurrentGameState.MissionClear)
            _anim.transform.Rotate(Vector3.up * mouseSpeed * Time.deltaTime * r_x);

    }

    public override void NotifyTargetKilled()
    {
        _lastAttack = null;
        SetState(PlayerState.IDLE);
    }
    public bool isDead = false;
    public override void SetDeadState()
    {
        if (!isDead)
        {
            SetState(PlayerState.DEAD);
            isDead = true;
            return;
        }
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
            Normal.SetActive(true);
            Special.SetActive(false);
        }
        catch
        {

        }
    }
    bool isTrans1;

    bool isSkillTimeSet = false;
    public void ChangeModel()
    {
        if (isNormal && SpecialGauge >= 100)
        {
            
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameStatus. SetCurrentGameState(CurrentGameState.Product);
                isNormal = false;
                isSpecial = true;
                SetInvincibility(true);
                TimeLine.SetActive(true);                            

                SetState(PlayerState.TRANS);
                //SetState(PlayerState.IDLE);

                // 스킬 1번 사라졌다 나오게 하기.
                Skill1Return(Skill1_Effects, Skill1_Special_Effects, isNormal);
                Skill1Return(Skill1_Shoots, Skill1_Special_Shoots, isNormal);
                Skill1PositionSet(Skill1_Effects, Skill1_Shoots, Skill1_Special_Shoots, isNormal);
                if (!Normal.activeSelf)
                    Normal.SetActive(true);
                // 스킬2번 바닥 사라지게하기.
                if ((isNormal && Skill2_Test.activeSelf) || (!isNormal && Skill2_Test2.activeSelf))
                {
                    Skill2_Test.SetActive(false);
                    Skill2_Test2.SetActive(false);
                    isSkill2End = false;
                }
            }
        }

        if (isSpecial)
        {//11.6초후변신끝
            WeaponTransformEffect.SetActive(true);
            specialTimer += Time.deltaTime;
            //if (specialTimer >= 0.6833f && !isTrans1)
            //{
                //isTrans1 = true;
                //SetState(PlayerState.TRANS);
            //}
            if (specialTimer >= 1.5f)
            {
                WeaponTransformEffect.SetActive(false);
                Special.SetActive(true);
            }
            if (specialTimer >= 2f)
            {
                Normal.SetActive(false);
            }
            if (specialTimer >= 5.82f)
            {
                Change_Effect.SetActive(false);
                //SetState(PlayerState.IDLE);
            }
            if (specialTimer >= 6.7f)
            {
                specialTimer = 0;
                TimeLine.SetActive(false);
                isSpecial = false;
                isAttackOne = false;

                StartCoroutine(SetOff());
                return;
            }
        }
    }

    IEnumerator SetOff()
    {
        yield return new WaitForSeconds(2f);

        SetInvincibility(false);
        GameStatus.SetCurrentGameState(GameStatus.prevState);
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
    bool isDashSound;

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttackOne = false;
            isAttackTwo = false;
            isAttackThree = false;

            isFlash = true;

            FlashPosition = new Vector3(_anim.transform.position.x, _anim.transform.position.y + 0.83f, _anim.transform.position.z);
            FlashEffect2.SetActive(false);
            SetState(PlayerState.RUN);
            if (isSkill3)
            {
                Skill3_End.transform.position = Skill3_Start.transform.position;
                Skill3_End.transform.rotation = Skill3_Start.transform.rotation;
                Skill3_End.SetActive(true);
            }
            if ((isNormal && Skill2_Test.activeSelf) || (!isNormal && Skill2_Test2.activeSelf))
            {
                Skill2_Test.SetActive(false);
                Skill2_Test2.SetActive(false);
                isSkill2End = false;
            }
            _Sound.sfx.PlayPlayerSFX(this.gameObject, _Sound.sfx.teleportSFX);
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
            }
            if (flashTimer >= 0.3f)
            {
                if (isNormal)
                    Normal.SetActive(true);
                if (!isNormal)
                    Special.SetActive(true);
            }
            if (flashTimer >= 0.33f && !isDashSound)
            {
                isDashSound = true;
                var voice = _Sound.voice;
                voice.PlayPlayerVoice(this.gameObject, voice.teleportVoice);
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
                isDashSound = false;
                UserInterface.Instance.UIPlayer.DashStart();


                flashTimer = 0;
                return;
            }
        }
    }
    public int maxDash = 3;
    public float dashCoolTime = 3f;
    public float currentDashCoolTime = 0f;
    public int remainingDash = 0;


    // 스킬 켜주고 꺼주고 하는 함수
    void Skill1Set(GameObject[] effects, GameObject[] effects_special, bool isnormal)
    {
        if (isnormal)
        {
            if (Skill1_Amount <= 1)
                for (int i = 0; i < 5; i++)
                {
                    effects[i].SetActive(false);
                    effects_special[i].SetActive(false);
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
                effects_special[3].SetActive(false);
                effects_special[4].SetActive(false);
            }
        }
        else
        {
            if (Skill1_Amount <= 1)
                for (int i = 0; i < 5; i++)
                {
                    effects[i].SetActive(false);
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
        for (int i = 0; i < 5; i++)
        {
            if (Skill1_Amount < 2)
                return;

            if (Skill1_Amount >= i + 2)
            {
                try
                {
                    if (isnormal)
                    {
                        //if (Vector3.Distance(effects[i].transform.position, targets[rands[i]].transform.position) >= distance)
                        effects[i].transform.position = Vector3.MoveTowards(effects[i].transform.position, new Vector3(targets[rands[i]].transform.position.x, targets[rands[i]].transform.position.y + 1f, targets[rands[i]].transform.position.z), skill1Speed * Time.deltaTime);
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
    public void Skill1Return(GameObject[] effects, GameObject[] effects_special, bool isnormal)
    {

        for (int i = 0; i < 5; i++)
        {
            effects[i].SetActive(false);
        }

        for (int i = 0; i < 5; i++)
        {
            effects_special[i].SetActive(false);
        }

    }
    public void Skill1PositionSet(GameObject[] normal_effect, GameObject[] normal_shoot, GameObject[] special_shoot, bool isnormal)
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
    PlayerState state;
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
        if (CurrentState != PlayerState.IDLE2 || CurrentState != PlayerState.CLEAR || CurrentState != PlayerState.DEAD)
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
                Skill1Shoots.gameObject.SetActive(true);
                Skill1Set(Skill1_Shoots, Skill1_Special_Shoots, isNormal);

                var voice = _Sound.voice;
                var sfx = _Sound.sfx;
                voice.PlayPlayerVoice(this.gameObject, voice.skill1Voice);
                sfx.PlayPlayerSFX(this.gameObject, sfx.skill1SFX);

                // 몬스터 수의 값을 랜덤함수 5개를 돌려서 배치 시킨 후.
                for (int i = 0; i < 5; i++)
                {
                    randomShoot[i] = Random.Range((int)0, (int)_monster.Count);
                }

                // 스킬이 날라간다.
                isShoot = true;
                isSkill1CTime = true;
                isBall = false;
                try
                {
                    //_Sound.PlaySkill1SFX();
                }
                catch
                {

                }
            }
        }
        // 스킬이 날라가기 시작하면
        if (isShoot)
        {

            Skill1Return(Skill1_Effects, Skill1_Special_Effects, isNormal);

            // 날라가는 시간을 정해준 후에.
            Skill1Timer1 += Time.deltaTime;
            // 날린다
            //Skill1Shoot(Skill1_Shoots, Skill1_Special_Shoots, _monster, randomShoot, 0, isNormal);

            if (_monster.Count == 0)
            {

                for (int i = 0; i < 5; i++)
                {
                    Skill1_Shoots[i].transform.position = Skill1_Effects[i].transform.position;
                }

                Skill1Return(Skill1_Shoots, Skill1_Special_Shoots, isNormal);
                Skill1Timer1 = 0;
                isShoot = false;
                Skill1_Amount = 1;
            }
            // 날아가는 시간이 지났는데 안없어졌으면

            //if (Skill1Timer1 >= 2f && _monster.Count != 0)
            //{
            //    _monster = GameStatus.Instance.ActivedMonsterList;
            //    Skill1Shoot(Skill1_Shoots, Skill1_Special_Shoots, _monster, randomShoot, 0, isNormal);

            //}
            if (Skill1Timer1 >= skill1ShootTime)
            {

                Skill1PositionSet(Skill1_Effects, Skill1_Shoots, Skill1_Special_Shoots, isNormal);

                Skill1Timer1 = 0;
                isShoot = false;

                Skill1_Amount = 1;

            }
        }

        //스킬 쿨타임 도는 함수
        SKill1Reset();
    }

    void Skill2Set()
    {
        //if (isSkill2)
        //    return;
        try
        {
            skill2_Distance = (30 / followCam.height) - 8f;       //followCam.height;//(50f / followCam.height) - 13f;
        }
        catch
        {

        }

        if (skill2_Distance >= skill2_maxDis)
            skill2_Distance = skill2_maxDis;

        if (skill2_Distance <= skill2_minDis)
            skill2_Distance = skill2_minDis;
        Skill2_Parent.localPosition = new Vector3(0, 0.35f, skill2_Distance);
    }

    public GameObject Skill2_Test, Skill2_Test2;
    public bool isSkill2End;
    public void Skill2()
    {
        if (isSkill2) return;

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isNormal)
                Skill2_Test.SetActive(true);
            else
                Skill2_Test2.SetActive(true);

            isSkill2End = true;
        }
        if (isSkill2End)
        {
            if ((isNormal && Skill2_Test2.activeSelf) || (!isNormal && Skill2_Test.activeSelf))
            {
                Skill2_Test.SetActive(false);
                Skill2_Test2.SetActive(false);
                isSkill2End = false;
            }
        }

        if ((isNormal && Skill2_Test.activeSelf) || (!isNormal && Skill2_Test2.activeSelf))
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetState(PlayerState.SKILL2);
                isSkill2 = true;
                isSkill2Dash = true;
                return;
            }
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
        if (isSkill3 || Skill3_End.activeSelf || isSkill2Dash)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isNormal)
                Normal.SetActive(true);
            if (!isNormal)
                Special.SetActive(true);

            SetState(PlayerState.SKILL3);
            isSkill3 = true;
            isSkill3Dash = true;
            return;
        }
    }
    void DamgeUp10(float a)
    {

    }


    public void Skill4()
    {
        if (isSkill4 || isNormal || !isCanUltimate)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TimeLine2.SetActive(true);
            Skill1Return(Skill1_Effects, Skill1_Special_Effects, isNormal);
            Skill1Return(Skill1_Shoots, Skill1_Special_Shoots, isNormal);
            Skill1PositionSet(Skill1_Effects, Skill1_Shoots, Skill1_Special_Shoots, isNormal);
            SetState(PlayerState.SKILL4);
            isSkill4 = true;

            _monster = GameStatus.Instance.ActivedMonsterList;

            bool isTiber = false;
            int count = 0;
            foreach (GameObject mob in _monster)
            {
                if (mob.GetComponent<FSMManager>().monsterType == MonsterType.Tiber)
                {
                    // 여기서 내가 티버를 찾았고.  
                    // 티버를 시트7번에 앉혀.
                    isTiber = true;
                    mob.transform.position = Seats[6].position;
                    mob.transform.LookAt(new Vector3(Anim.transform.position.x, mob.transform.position.y, Anim.transform.position.z));
                    break;
                        
                }

                count++;
            }
            for (int i = 0; i < 7; i++)
            {
                if (isTiber == true && i == 6 || i == count) continue;

                try
                {
                    _monster[i].transform.position = Seats[i].transform.position;
                    _monster[i].transform.LookAt(new Vector3(Anim.transform.position.x, _monster[i].transform.position.y, Anim.transform.position.z));
                }
                catch
                {

                }
            }
            return;
        }
    }
    public void SKill1Reset()
    {
        if (isSkill1CTime)
        {
            Skill1CTime -= Time.deltaTime;
            if (Skill1CTime <= 0)
            {
                UIPlayer.SkillSetUp(0);
                Skill1CTime = Stat.skillCTime[0];
                isSkill1CTime = false;
            }
        }
    }



    public void Skill2Reset()
    {
        if (isSkill2CTime)
        {
            Skill2CTime -= Time.deltaTime;
            if (Skill2CTime <= 0)
            {
                UIPlayer.SkillSetUp(1);
                Skill2CTime = Stat.skillCTime[1];

                isSkill2 = false;
                isSkill2CTime = false;

                Skill2_Normal.SetActive(false);
                Skill2_Special.SetActive(false);
            }
        }
    }

    public void Skill3Reset()
    {
        if (isSkill3CTime)
        {
            Skill3CTime -= Time.deltaTime;

            if (Skill3CTime <= 0)
            {
                UIPlayer.SkillSetUp(2);
                Skill3CTime = Stat.skillCTime[2];
                Skill3_End.SetActive(false);
                isSkill3CTime = false;
            }

        }
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
    public void SkillCoolHalfReset()
    {
        // 스킬 1번 쿨타임 절반 만들기
        if (isSkill1CTime)
            Skill1CTime /= 2f;
        // 스킬 2번 쿨타임 절반 만들기
        if (isSkill2CTime)
            Skill2CTime /= 2f;
        // 스킬 3번 쿨타임 절반 만들기
        if (isSkill3CTime)
            Skill3CTime /= 2f;
    }
    public void SkillCoolReset()
    {
        // 스킬 1번 쿨타임 관련.
        Skill1CTime = 10f;
        isSkill1CTime = false;
        isSkill2CTime = false;
        isSkill3CTime = false;
        isBall = true;
        if (isNormal)
            Skill1_Amount = 4;
        else
            Skill1_Amount = 6;
        // 스킬 2번 쿨타임 관련.
        Skill2CTime = 10f;
        if (isNormal)
            Skill2_Normal.SetActive(false);
        else
            Skill2_Special.SetActive(false);
        isSkill2 = false;
        // 스킬 3번 쿨타임 관련.
        Skill3CTime = 10f;
        Skill3_End.SetActive(false);
    }

    public static Vector3 GetLookTargetPos(Transform transform)
    {
        return new Vector3(Instance.Anim.transform.position.x,
            transform.position.y, Instance.Anim.transform.position.z);
    }


    public CharacterStat LastHit()
    {
        return Stat.lastHitBy;
    }
}
