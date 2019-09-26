using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatDEAD : RedHatFSMState
{
    bool Dead = false;
    float time = 0;

    public override void BeginState()
    {
        base.BeginState();

        time = 0;
        Dead = false;

        if (_manager.dashEffect != null)
        {
            EffectPoolManager._Instance._RedHatEffectPool.ItemReturnPool(_manager.dashEffect);
            _manager.dashEffect = null;
        }

        _manager.WPMats.SetFloat("_DissolveEdgeMultiplier", 8);
        foreach (Material mat in _manager.Mats)
        {
            mat.SetFloat("_DissolveEdgeMultiplier", 8);
        }

        useGravity = false;
        _manager.CC.detectCollisions = false;
    }

    public override void EndState()
    {
        base.EndState();

        _manager.WPMats.SetFloat("_DissolveEdgeMultiplier", 0);
        _manager.WPMats.SetFloat("_DissolveIntensity", 0);

        foreach (Material mat in _manager.Mats)
        {
            mat.SetFloat("_DissolveEdgeMultiplier", 0);
            mat.SetFloat("_DissolveIntensity", 0);
        }

        useGravity = true;
        _manager.CC.detectCollisions = true;
        
        MonsterPoolManager._Instance._RedHat.ItemReturnPool(gameObject, "monster");
        time = 0;
        Dead = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        time += 0.45f * Time.deltaTime;

        _manager.WPMats.SetFloat("_DissolveIntensity", time);
        foreach (Material mat in _manager.Mats)
        {
            mat.SetFloat("_DissolveIntensity", time);
        }

        if(time > 0.7 && !Dead)
        {
            Dead = true;
        }

        if (Dead) {
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
