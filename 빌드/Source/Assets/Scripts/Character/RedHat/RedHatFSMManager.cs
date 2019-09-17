using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum RedHatState
{
    POPUP = 0,
    CHASE,
    ATTACK,
    DEAD,
    DASH,
    HIT,
}


[RequireComponent(typeof(RedHatStat))]
public class RedHatFSMManager : FSMManager
{
    private bool _isInit = false;
    public RedHatState startState = RedHatState.POPUP;
    private Dictionary<RedHatState, RedHatFSMState> _States = new Dictionary<RedHatState, RedHatFSMState>();

    private RedHatState _CurrentState;
    public RedHatState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public RedHatFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private RedHatStat _Stat;
    public RedHatStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim { get { return _Anim; } }

    public Transform _AttackTransform;
    public SkinnedMeshRenderer _MR;

    public CharacterStat _lastAttack;

    public Slider _HPSilder;
    public GameObject hitEffect;
    public GameObject hitEffect_Special;
    public GameObject hitEffect_Skill1;
    public Transform hitLocation;

    public MonsterSound _Sound;

    public GameObject dashEffect;

    public float _DetectingRange;

    public Collider _PriorityTarget;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<RedHatStat>();
        _Anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<MonsterSound>();

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        RedHatState[] stateValues = (RedHatState[])System.Enum.GetValues(typeof(RedHatState));
        foreach (RedHatState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("RedHat" + s.ToString());
            RedHatFSMState state = (RedHatFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (RedHatFSMState)gameObject.AddComponent(FSMType);
            }

            _States.Add(s, state);
            state.enabled = false;
        }

        monsterType = ObjectManager.MonsterType.RedHat;
    }

    private void Start()
    {
        SetState(startState);
        _isInit = true;
    }

    public void SetState(RedHatState newState)
    {
        if (_isInit)
        {
            _States[_CurrentState].enabled = false;
            _States[_CurrentState].EndState();
        }
        _CurrentState = newState;
        _States[_CurrentState].BeginState();
        _States[_CurrentState].enabled = true;
        _Anim.SetInteger("CurrentState", (int)_CurrentState);
    }

    public void OnHit()
    {
        

        Stat.TakeDamage(Stat, 350);

        //hp--;
        //카메라쉐이킹
        Shake.instance.ShakeCamera(0.3f, 0.3f, 0.7f);
        _Sound.PlayHitSFX();

        //hit스크립트로넘겨줌
        if (Stat.Hp > 0)
        {
            if (CurrentState == RedHatState.DASH) return;

            SetState(RedHatState.HIT);

            //플레이어 쳐다본 후
            try
            {
                transform.localEulerAngles = Vector3.zero;
                transform.LookAt(PlayerFSMManager.instance.Anim.transform);
                // 뒤로 밀림
                transform.Translate(Vector3.back * 20f * Time.smoothDeltaTime, Space.Self);
                //플레이어피버게이지증가?
                //InputHandler.instance.FeverGauge++;
            }
            catch
            {

            }
        }
        else
        {
            SetDeadState();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            if (PlayerFSMManager.instance.isNormal)
                Instantiate(hitEffect, hitLocation.transform.position, Quaternion.identity);
            if (!PlayerFSMManager.instance.isNormal)
                Instantiate(hitEffect_Special, hitLocation.transform.position, Quaternion.identity);

            if (Stat.Hp > 0)
            {
                OnHit();
            }
        }

        if (other.transform.tag == "Ball")
        {
            Instantiate(hitEffect_Skill1, hitLocation.transform.position, Quaternion.identity);


            if (Stat.Hp > 0)
            {
                OnHit();

                other.transform.gameObject.SetActive(false);
            }

        }
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Skill2")
        {
            if (PlayerFSMManager.instance.isNormal)
                Instantiate(hitEffect, hitLocation.transform.position, Quaternion.identity);
            else
                Instantiate(hitEffect_Special, hitLocation.transform.position, Quaternion.identity);
            if (Stat.Hp > 0)
            {
                OnHit();
            }
        }
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        Debug.Log("Dead");
        SetState(RedHatState.DEAD);
    }

}
