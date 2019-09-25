using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MacFSMManager))]
public class MacFSMState : MonoBehaviour
{
    [HideInInspector] public MacFSMManager _manager;
    public bool sub;

    protected float _SkillCoolTime = 10f;
    protected float _CurTime = 0f;

    private void Awake()
    {
        _manager = GetComponent<MacFSMManager>();
        
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

    protected virtual void FixedUpdate()
    {
        if (sub) return;

        HPUI();
        Gravity();

    }

    public void SkillCool()
    {
        if (_manager.CurrentState != MacState.SKILL)
        {
            _CurTime += Time.deltaTime;
        }

        if (_CurTime > _SkillCoolTime)
        {
            _CurTime = 0;
            _manager.SetState(MacState.SKILL);
        }
    }

    public void Gravity()
    {

        Vector3 gravity = Vector3.zero;
        gravity.y = Physics.gravity.y * Time.deltaTime;

        _manager.CC.Move(gravity);
    }

    public void HPUI()
    {
        float HP = _manager.Stat.Hp;

        _manager._HPSilder.value = HP / _manager.Stat.MaxHp;

    }

}
