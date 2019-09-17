using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatDEAD : RedHatFSMState
{
    public override void BeginState()
    {
        base.BeginState();


        if (_manager.dashEffect != null)
        {
            EffectPoolManager._Instance._RedHatEffectPool.ItemReturnPool(_manager.dashEffect);
            _manager.dashEffect = null;
        }

        MonsterPoolManager._Instance._RedHat.ItemReturnPool(gameObject, "monster");

    }

    public override void EndState()
    {
        base.EndState();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
