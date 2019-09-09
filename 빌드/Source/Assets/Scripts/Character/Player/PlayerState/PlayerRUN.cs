using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRUN : FSMState {

    public override void BeginState()
    {
        base.BeginState();
        AudioManager.playLoopSound(_manager._runSound, _manager.musicPlayer);
    }

    public override void EndState()
    {
        base.EndState();
    }
    void Update()
    {
        // 키 입력이 없다면 IDLE 스테이트로
        if (!_manager.OnMove())
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }

        
        // 움직일 위치로 시선을 옮김
        //transform.rotation = Quaternion.RotateTowards(
        //    transform.rotation,
        //    Quaternion.LookRotation(destVector),
        //    _manager.MyStatData.TurnSpeed * Time.deltaTime);
    }
}
