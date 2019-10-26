using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

[RequireComponent(typeof(MacFSMManager))]
public class MacFSMState : MonoBehaviour
{
    [HideInInspector] public MacFSMManager _manager;
    public bool sub;

    protected float _SkillCoolTime = 10f;
    protected float _CurTime = 0f;


    protected bool useGravity = true;

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
        Debug.DrawLine(_manager.agent.destination, new Vector3(_manager.agent.destination.x, 
            _manager.agent.destination.y + 1f, _manager.agent.destination.z), Color.red);

    }

    protected virtual void FixedUpdate()
    {
        if (sub) return;

        if (GameManager.Instance.uIActive.monster)
            HPUI();

        if(useGravity)
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
        try
        {
            UserInterface.Instance.HPChangeEffect(_manager.Stat, _manager._HPBar);
        }
        catch
        {

        }

    }

}
