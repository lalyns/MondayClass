using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

using MC.UI;
using MC.Sound;

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
    public static PlayerFSMManager Instance {
        get {
            if (instance == null)
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
    float vertical, horizontal;
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
    public float skill1Speed = 20f;
    [Header("스킬1번 날라가는 시간,")]
    public float skill1ShootTime = 4f;


    public bool isAttackOne, isAttackTwo, isAttackThree, isSkill2;

    public bool isSkill3, isSkill4;

    //[HideInInspector]
    public float _attack1Time, _attack2Time, _attack3Time, _attackBack1, _attackBack2, _specialAnim, _skill2Time, _skill3Time;

    [Header("X축 마우스 감도")]
    public float mouseSpeed = 80f;


    float r_x = 0;
    // [HideInInspector]
    public float _v, _h;

    bool isInputLock;

    public float flashTimer = 0;
    public bool isSpecial, isFlash;
    public bool isInvincibility = false;
    public void SetInvincibility(bool value)
    {
        isInvincibility = value;
    }
    public bool isSkill3Dash = false;
    public GameObject Normal;
    public GameObject Special;
    public GameObject WeaponTransformEffect;
    public GameObject TimeLine;
    public GameObject Change_Effect;
    public float specialTimer = 0;
    CapsuleCollider Attack_Capsule;
    [HideInInspector]
    public CapsuleCollider Skill3_Capsule;
    SphereCollider SKill2_Sphere;
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

    public Skill1Shoots Skill1Shoots;
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
    [Header("스킬2번 현재 거리, 최소거리, 최대거리")]
    public float skill2_Distance;
    public float skill2_minDis;
    public float skill2_maxDis;

    public Transform Skill2_Parent;
    public Vignette vignette;
    public bool isShake = false;

    public bool isMouseYLock;
    Bloom bloom;

    public bool isIDLE;

    public Rigidbody rigid;

    public SkinnedMeshRenderer[] _MR;
    public List<Material> materialList = new List<Material>();

    public List<Transform> Seats = new List<Transform>();

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
        bloom = GameObject.Find("mainCam").GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>();
        Attack_Capsule = GameObject.FindGameObjectWithTag("Weapon").GetComponent<CapsuleCollider>();
        Skill3_Capsule = Skill3_Start.GetComponent<CapsuleCollider>();
        SKill2_Sphere = Skill2_Start.GetComponent<SphereCollider>();

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

        for(int x = 0; x<_MR.Length; x++)
            materialList.AddRange(_MR[x].materials);
        remainingDash = 3;

    }
    VignetteModeParameter parameter;
    //public Texture2D aaasdf;
    public TextureParameter Concent;

    float normalTimer;
    float gaugePerSecond;

    public int ShieldCount;

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
        Skill1CTime = 10f;




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
    
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(shake.ShakeUI(0.2f, 4f, 3f));
        }

        if (isInputLock)
            return;


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

        ChangeModel();

        if (isSpecial)
            return;
        if (remainingDash > 0 && !isSkill3Dash)
            Dash();
       
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
        Skill4();
        if (!isSkill2End && !isSkill3)
            Attack();
     

        Skill3MouseLock();
        Skill3Reset();

        if (isNormal)
        {
            _anim.SetFloat("Normal", 0);
        }
        else
            _anim.SetFloat("Normal", 1f);

        if (!isNormal)
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
        if (isSkill2)
            return;
    }


    private void FixedUpdate()
    {
        Skill2Set();

        r_x = Input.GetAxis("Mouse X");

        if(GameManager.Instance.CharacterControl && !isSpecial)
            _anim.transform.Rotate(Vector3.up * mouseSpeed * Time.deltaTime * r_x);

    }

    private void LateUpdate()
    {
       
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
            Normal.SetActive(true);
            Special.SetActive(false);
        }
        catch
        {

        }
    }
    bool isTrans1;
    public void ChangeModel()
    {
        if (isNormal && SpecialGauge >= 100)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
              
                isNormal = false;
                isSpecial = true;
                SetInvincibility(true);
                TimeLine.SetActive(true);
                Skill1Return(Skill1_Effects, Skill1_Special_Effects, isNormal);
                Skill1Return(Skill1_Shoots, Skill1_Special_Shoots, isNormal);
                Skill1PositionSet(Skill1_Effects, Skill1_Shoots, Skill1_Special_Shoots, isNormal);

                if (Skill2_Test.activeSelf)
                {
                    Skill2_Test.SetActive(false);
                    isSkill2End = false;
                }
            }
        }
 
        if (isSpecial)
        {//11.6초후변신끝
            WeaponTransformEffect.SetActive(true);
            specialTimer += Time.deltaTime;
            if (specialTimer >= 0.6833f && !isTrans1)
            {
                isTrans1 = true;
                SetState(PlayerState.TRANS);
            }
            if (specialTimer >= 2.26f)
            {
                WeaponTransformEffect.SetActive(false);
                Special.SetActive(true);
            }
            if (specialTimer >= 2.7f)
            {
                Normal.SetActive(false);
            }
            if (specialTimer >= 5.82f - 0.8f)
            {
                Change_Effect.SetActive(false);
                SetState(PlayerState.IDLE);
            }
            if (specialTimer >= 5.82f)
            {
                specialTimer = 0;
                TimeLine.SetActive(false);
                isSpecial = false;
                isAttackOne = false;
                isTrans1 = false;
                StartCoroutine(SetOff());
                return;
            }
        }
    }

    IEnumerator SetOff()
    {
        yield return new WaitForSeconds(2f);

        SetInvincibility(false);
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
            if (Skill2_Test.activeSelf)
            {
                Skill2_Test.SetActive(false);
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
            if(flashTimer>=0.33f && !isDashSound)
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
    void Skill1Return(GameObject[] effects, GameObject[] effects_special, bool isnormal)
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
                Skill1Shoots.gameObject.SetActive(true);
                Skill1Set(Skill1_Shoots, Skill1_Special_Shoots, isNormal);

                var voice = _Sound.voice;
                voice.PlayPlayerVoice(this.gameObject, voice.skill1Voice);

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
            Skill1Shoot(Skill1_Shoots, Skill1_Special_Shoots, _monster, randomShoot, 0, isNormal);

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

            if (Skill1Timer1 >= 2f && _monster.Count != 0)
            {
                _monster = GameStatus.Instance.ActivedMonsterList;
                Skill1Shoot(Skill1_Shoots, Skill1_Special_Shoots, _monster, randomShoot, 0, isNormal);

            }
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
            skill2_Distance = (50f / followCam.height) - 13f;
        }
        catch
        {

        }

        if (skill2_Distance >= skill2_maxDis)
            skill2_Distance = skill2_maxDis;

        Skill2_Parent.localPosition = new Vector3(0, 0.18f, skill2_Distance);
    }

    public GameObject Skill2_Test;
    public bool isSkill2End;
    public void Skill2()
    {
        if (isSkill2) return;

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Skill2_Test.SetActive(true);            
            isSkill2End = true;            
        }

        if (Skill2_Test.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetState(PlayerState.SKILL2);
                isSkill2 = true;
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
        if (isSkill3 || Skill3_End.activeSelf)
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
        if (isSkill4 || isNormal)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetState(PlayerState.SKILL4);
            isSkill4 = true;

            _monster = GameStatus.Instance.ActivedMonsterList;
            for(int i=0; i<_monster.Count; i++)
            {
                _monster[i].transform.position = Seats[i].transform.position;
                _monster[i].transform.LookAt(new Vector3(Anim.transform.position.x, _monster[i].transform.position.y, Anim.transform.position.z));
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
                Skill1CTime = 10f;
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
                Skill2CTime = 10f;
                Skill2_Start.SetActive(false);
                isSkill2 = false;
                isSkill2CTime = false;
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
                Skill3CTime = 10f;
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
        Skill2_Start.SetActive(false);
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
}
