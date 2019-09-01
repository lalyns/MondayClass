using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTRANS : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
    }

    void Update()
    {
        //_time += Time.deltaTime;

        //if(_time >= 1.90f)
        //{
        //    _manager.WeaponTransformEffect.SetActive(false);
        //    _manager.Normal.SetActive(false);
        //    _manager.Special.SetActive(true);
        //}
        //if(_time >= _manager._specialAnim + 0.55f)
        //{
        //    _manager.Change_Effect.SetActive(false);
        //    _manager.SetState(PlayerState.IDLE);
        //}
    }
}
