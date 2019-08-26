using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MacFSMManager))]
public class MacFSMState : MonoBehaviour
{
    public MacFSMManager _manager;
    public bool sub;

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

    private void Update()
    {
        HPUI();

    }

    protected virtual void FixedUpdate()
    {
        if (sub) return;

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
