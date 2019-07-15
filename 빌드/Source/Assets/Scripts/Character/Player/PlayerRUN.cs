using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRUN : FSMState {

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
        // 키 입력이 없다면 IDLE 스테이트로
        if (!_manager.OnMove())
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }

        // 키에 맞게 움직임
        Vector3 destVector = Vector3.right * Input.GetAxis("Horizontal") +
            Vector3.forward * Input.GetAxis("Vertical");
        destVector = destVector * _manager.Stat.MoveSpeed * Time.deltaTime;
        _manager.CC.Move(destVector);

        // 움직일 위치로 시선을 옮김
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(destVector),
            _manager.MyStatData.TurnSpeed * Time.deltaTime);
    }
}
