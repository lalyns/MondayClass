using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisULTIMATE : RirisFSMState
{
    [System.Serializable]
    public class UltiPattern
    {
        public Transform[] trans;
    }

    public UltiPattern[] ultiPatterns = new UltiPattern[5];


    public override void BeginState()
    {
        base.BeginState();


    }

    public override void EndState()
    {
        base.EndState();


    }

    protected override void Update()
    {
        base.Update();


    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();


    }

    public IEnumerator PatternA(UltiPattern pattern, float delay)
    {
        for (int i = 0; i < pattern.trans.Length; i++)
        {
            yield return new WaitForSeconds(delay);
            
            // 대충 이펙트 호출하는 코드

        }
    }

    public void PatternEnd()
    {
        _manager.SetState(RirisState.PATTERNEND);
    }
}
