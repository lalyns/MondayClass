using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE = 0,
    RUN,
    DEAD
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

    private CharacterController _cc;
    public CharacterController CC { get { return _cc; } }

    private PlayerStat _stat;
    public PlayerStat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    public CharacterController testTarget;

    public int clickLayer = 0;
    public GameObject weapon;

    protected override void Awake()
    {
        base.Awake();
        SetGizmoColor(Color.red);
        ShowSight(false);
        clickLayer = (1 << 9) + (1 << 10);

        _cc = GetComponent<CharacterController>();
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

    }

    private void Start()
    {
        SetState(startState);
        _isinit = true;
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
        return Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Horizontal") <= -0.01f ||
            Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f;
    }

    private void Update()
    {
        // 공격처리는 죽음을 제외한 모든 상황에서 처리
        if (CurrentState != PlayerState.DEAD)
        {
            _onAttack = Input.GetAxis("Fire1") >= 0.01f ? true : false;
            _anim.SetBool("OnAttack", _onAttack);
        }
        weapon.transform.position += new Vector3(100f * Time.deltaTime, 0, 0);

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
}
