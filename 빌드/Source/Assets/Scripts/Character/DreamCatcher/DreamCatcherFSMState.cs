using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DreamCatcherFSMManager))]
public class DreamCatcherFSMState : MonoBehaviour
{
    protected DreamCatcherFSMManager _manager;
    protected float _Skill1Time;

    private void Awake()
    {
        _manager = GetComponent<DreamCatcherFSMManager>();
    }

    public virtual void BeginState()
    {
        
    }

    public virtual void EndState()
    {

    }

    protected virtual void Update()
    {
        
    }

    protected void DahsCheck()
    {
        if (_manager.CurrentState != DreamCatcherState.DASH)
        {
            _Skill1Time += Time.deltaTime;
        }

        if (_Skill1Time > _manager.Stat.statData._SkillCoolTime1)
        {
            _Skill1Time = 0;
            _manager.SetState(DreamCatcherState.DASH);
        }
    }

    protected virtual void FixedUpdate()
    {
        Vector3 gravity = Vector3.zero;
        gravity.y = Physics.gravity.y * Time.deltaTime;

        _manager.CC.Move(gravity);
    }

}
