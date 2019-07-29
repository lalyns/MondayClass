using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    IDLE = 0,
    CHASE,
    ATTACK,
    DEAD
}

[RequireComponent(typeof(MonsterStat))]
public class MonsterFSMManager : FSMManager
{
    private bool _isinit = false;
    public MonsterState startState = MonsterState.IDLE;
    private Dictionary<MonsterState, MonsterFSMState> _states = new Dictionary<MonsterState, MonsterFSMState>();

    [SerializeField]
    private MonsterState _currentState;
    public MonsterState CurrentState {
        get {
            return _currentState;
        }
    }

    private CharacterController _cc;
    public CharacterController CC { get { return _cc; } }

    private CapsuleCollider _playercc;
    public CapsuleCollider PlayerCC { get { return _playercc; } }

    private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } }

    private MonsterStat _stat;
    public MonsterStat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    private Camera _sight;
    public Camera Sight { get { return _sight; } }

    //public int sightAspect = 3;


    protected override void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _stat = GetComponent<MonsterStat>();
        _anim = GetComponentInChildren<Animator>();
        _sight = GetComponentInChildren<Camera>();

        _playercc = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CapsuleCollider>();
        _playerTransform = _playercc.transform;

        MonsterState[] stateValues = (MonsterState[])System.Enum.GetValues(typeof(MonsterState));
        foreach (MonsterState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Monster" + s.ToString());
            MonsterFSMState state = (MonsterFSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (MonsterFSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }

    }

    public void SetState(MonsterState newState)
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

    private void Start()
    {
        SetState(startState);
        _isinit = true;
    }

    private void OnDrawGizmos()
    {
        if (_sight != null)
        {
            Gizmos.color = Color.red;
            Matrix4x4 temp = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(
                _sight.transform.position,
                _sight.transform.rotation,
                Vector3.one
                );

            Gizmos.DrawFrustum(
                _sight.transform.position,
                _sight.fieldOfView,
                _sight.farClipPlane,
                _sight.nearClipPlane,
                _sight.aspect
                );

            Gizmos.matrix = temp;
        }
    }

    public void NotifyTargetKilled()
    {
        SetState(MonsterState.IDLE);
    }

    public void SetDeadState()
    {
        SetState(MonsterState.DEAD);
    }

}
