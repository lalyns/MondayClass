using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

[RequireComponent(typeof(TiberFSMManager))]
public class TiberFSMState : MonoBehaviour
{
    protected TiberFSMManager _manager;
    protected float _Skill1Time;

    protected bool useGravity = true;

    private void Awake()
    {
        _manager = GetComponent<TiberFSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }

    protected virtual void Update()
    {
        if (GameManager.Instance.uIActive.monster)
            HPUI();
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

    protected virtual void FixedUpdate()
    {
        if (useGravity)
        {
            Vector3 gravity = Vector3.zero;
            gravity.y = Physics.gravity.y * Time.deltaTime;

            _manager.CC.Move(gravity);
        }
    }

}
