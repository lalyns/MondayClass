using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacDEAD : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        MonsterPoolManager._Instance._Mac.ItemReturnPool(gameObject, "monster");
    }

    public override void EndState()
    {
        base.EndState();

    }

    public void Update()
    {
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
