using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;

public class TiberDISSOLVE : TiberFSMState
{
    float _time = 0;
    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();

        _time = 0;
        
        GameLib.DissoveActive(_manager.materialList, false);
        MonsterPoolManager._Instance._Tiber.ItemReturnPool(gameObject, MonsterType.Tiber);
    }

    protected override void Update()
    {
        base.Update();

        if (_time >= 3f)
        {
            
            _manager.SetState(TiberState.POPUP);            
            return;
        }
    }
}
