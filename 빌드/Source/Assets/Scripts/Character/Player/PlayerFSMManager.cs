using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerState
{
    IDLE = 0,
    RUN,
    ATTACK1,
    //ATTACK1BACK,
    ATTACK2,
    ATTACK3,
    //TRANS,
    DEAD,
}

[RequireComponent(typeof(PlayerStat))]
[ExecuteInEditMode]
public class PlayerFSMManager : FSMManager
{
    private bool _onAttack = false;
    private bool _isinit = false;
    public PlayerState startState = PlayerState.IDLE;
    private Dictionary<PlayerState, FSMState> _states = new Dictionary<PlayerState, FSMState>();

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


    protected override void Awake()
    {
        base.Awake();
        SetGizmoColor(Color.red);

        _cc = GetComponentInChildren<CapsuleCollider>();
        _stat = GetComponent<PlayerStat>();
        _anim = GetComponentInChildren<Animator>();

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
    }

    private void Start()
    {
        SetState(startState);
        _isinit = true;

        //Skill1UI = GameObject.Find("Skill1_CoolTime").GetComponent<Image>();
        //Skill1UI.fillAmount = 1f;
        //Skill1UI.gameObject.SetActive(false);
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





    private void Update()
    {
        // 공격처리는 죽음을 제외한 모든 상황에서 처리
        if (CurrentState != PlayerState.DEAD)
        {
            _onAttack = Input.GetAxis("Fire1") >= 0.01f ? true : false;
            //_anim.SetBool("OnAttack", _onAttack);
        }
        GetInput();
        Skill1();
        Attack();
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

    bool isCantMove;
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
    public void Attack(bool One, bool Two, bool Three, float Timer)
    {
        Timer += Time.deltaTime;
        if (One || Two || Three)
        {
            isCantMove = true;
        }
        if(Input.GetMouseButton(0) && !One && !Two && !Three)
        {
            SetState(PlayerState.ATTACK1);
            One = true;
            Two = false;
            Three = false;
        }
        if (Input.GetMouseButton(0) && One)
        {
            SetState(PlayerState.ATTACK2);
            One = false;
            Two = true;
            Three = false;
        }
        if(Input.GetMouseButton(0) && Two)
        {
            SetState(PlayerState.ATTACK3);
            One = false;
            Two = false;
            Three = true;
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
            if (Input.GetKey(KeyCode.F))
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
