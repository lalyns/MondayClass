using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RedHatFSMManager))]
public class RedHatFSMState : MonoBehaviour
{
    protected RedHatFSMManager _manager;
    protected float _Skill1Time;

    private void Awake()
    {
        _manager = GetComponent<RedHatFSMManager>();
    }

    public virtual void BeginState()
    {
        
    }

    public virtual void EndState()
    {

    }

    protected virtual void Update()
    {
        HPUI();
    }

    protected void DahsCheck()
    {
        if (_manager.CurrentState != RedHatState.DASH)
        {
            _Skill1Time += Time.deltaTime;
        }

        if (_Skill1Time > _manager.Stat.statData._SkillCoolTime1)
        {
            _Skill1Time = 0;
            _manager.SetState(RedHatState.DASH);
        }
    }

    public void HPUI()
    {
        float HP = _manager.Stat.Hp;

        _manager._HPSilder.value = HP / _manager.Stat.MaxHp;

    }

    protected virtual void FixedUpdate()
    {
        Vector3 gravity = Vector3.zero;
        gravity.y = Physics.gravity.y * Time.deltaTime;

        _manager.CC.Move(gravity);
    }

}
