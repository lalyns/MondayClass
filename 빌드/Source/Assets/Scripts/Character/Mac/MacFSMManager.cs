using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MacState
{
    POPUP = 0,
    CHASE,
    ATTACK,
    SKILL,
    RUNAWAY,
    HIT,
    DEAD,
}

[RequireComponent(typeof(MacStat))]
public class MacFSMManager : FSMManager
{
    private bool _isInit = false;
    public MacState startState = MacState.POPUP;
    private Dictionary<MacState, MacFSMState> _States = new Dictionary<MacState, MacFSMState>();

    private MacState _CurrentState;
    public MacState CurrentState {
        get {
            return _CurrentState;
        }
    }

    public MacFSMState CurrentStateComponent {
        get {
            return _States[_CurrentState];
        }
    }

    private CharacterController _CC;
    public CharacterController CC { get { return _CC; } }

    private CapsuleCollider _PlayerCapsule;
    public CapsuleCollider PlayerCapsule { get { return _PlayerCapsule; } }

    private MacStat _Stat;
    public MacStat Stat { get { return _Stat; } }

    private Animator _Anim;
    public Animator Anim { get { return _Anim; } }

    public Transform _AttackTransform;
    public SkinnedMeshRenderer _MR;

    public Slider _HPSilder;
    public GameObject hitEffect;
    public GameObject hitEffect_Special;
    public GameObject hitEffect_Skill1;
    public Transform hitLocation;

    public GameObject _PopupEffect;

    public MonsterSound _Sound;

    public Collider _PriorityTarget;
    public float _DetectingRange;

    protected override void Awake()
    {
        base.Awake();

        _CC = GetComponent<CharacterController>();
        _Stat = GetComponent<MacStat>();
        _Anim = GetComponentInChildren<Animator>();
        _Sound = GetComponent<MonsterSound>();

        _PlayerCapsule = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();

        MacState[] stateValues = (MacState[])System.Enum.GetValues(typeof(MacState));
        foreach (MacState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Mac" + s.ToString());
            MacFSMState state = (MacFSMState)GetComponent(FSMType);

            if(null == state)
            {
                state = (MacFSMState)gameObject.AddComponent(FSMType);
            }

            _States.Add(s, state);
            state.enabled = false;
        }

        monsterType = ObjectManager.MonsterType.Mac;
    }

    private void Start()
    {
        SetState(startState);
        _isInit = true;
    }

    public void SetState(MacState newState)
    {
        if (_isInit)
        {
            _States[_CurrentState].EndState();
            _States[_CurrentState].enabled = false;
        }
        _CurrentState = newState;
        _States[_CurrentState].BeginState();
        _States[_CurrentState].enabled = true;
        _Anim.SetInteger("CurrentState", (int)_CurrentState);
    }

    public void OnHit()
    {
      
        //hp--;
        //카메라쉐이킹
        Shake.instance.ShakeCamera();

        Stat.TakeDamage(Stat, 350);
        _Sound.PlayHitSFX();
        //Debug.Log(Stat.Hp);

        //hit스크립트로넘겨줌
        if (Stat.Hp > 0)
        {
            SetState(MacState.HIT);
            //플레이어 쳐다본 후
            transform.localEulerAngles = Vector3.zero;
            transform.LookAt(PlayerFSMManager.instance.Anim.transform);
            // 뒤로 밀림
            transform.Translate(Vector3.back * 20f * Time.smoothDeltaTime, Space.Self);
            //플레이어피버게이지증가?
            //PlayerFSMManager.instance.FeverGauge++;
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
            else
                Instantiate(hitEffect_Special, hitLocation.transform.position, Quaternion.identity);

            if (Stat.Hp > 0)
            {
                //Debug.Log("Attacked");
                OnHit();
            }

            if (_CurrentState == MacState.ATTACK)
            {
                try
                {
                }
                catch
                {

                }
            }
        }
        if (other.transform.tag == "Ball")
        {
            Instantiate(hitEffect_Skill1, hitLocation.transform.position, Quaternion.identity);


            if (Stat.Hp > 0)
            {
                OnHit();
                try
                {
                    other.transform.gameObject.SetActive(false);
                }
                catch
                {

                }

            }

            if (_CurrentState == MacState.ATTACK)
            {
                try
                {
                }
                catch
                {

                }
            }
        }
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
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Ball")
        {
            //if (Stat.Hp > 0)
            //{
            //    OnHit();
            //}

            if (_CurrentState == MacState.ATTACK)
            {
                try
                {

                }
                catch
                {

                }
            }
        }

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

        SetState(MacState.DEAD);
    }
}
