using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacDEAD : MacFSMState
{
    bool Dead = false;
    float time = 0;

    public override void BeginState()
    {
        base.BeginState();

        Dead = false;
        time = 0;

        foreach (Material mat in _manager.Mats)
        {
            mat.SetFloat("_DissolveEdgeMultiplier", 8);
        }
    }

    public override void EndState()
    {
        base.EndState();

        foreach (Material mat in _manager.Mats)
        {
            mat.SetFloat("_DissolveEdgeMultiplier", 0);
            mat.SetFloat("_DissolveIntensity", 0);
        }

        time = 0;
        Dead = true;
        MonsterPoolManager._Instance._Mac.ItemReturnPool(gameObject, "monster");
    }

    protected override void Update()
    {
        base.Update();

        time += 0.45f * Time.deltaTime;

        foreach (Material mat in _manager.Mats)
        {
            mat.SetFloat("_DissolveIntensity", time);
        }

        if (time > 0.7 && !Dead)
        {
            Dead = true;
        }

        if (Dead)
        {
            EndState();
            Dead = false;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void DeadHelper()
    {
        Dead = true;
    }
}
