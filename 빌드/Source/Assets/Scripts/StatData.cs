using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatData : ScriptableObject
{
    // 시체가 자동으로 사라지는 시간
    public float AutoDestroyTime = 3f;
    // 모든 캐릭터가 도는 속도
    public float TurnSpeed = 540f;

    // 플레이어의 최대 체력
    public float PlayerMaxHp = 100f;
    // 플레이어의 공격 범위
    public float PlayerAttackRange = 50f;
    // 플레이어의 이동 속도
    public float PlayerMoveSpeed = 50f;
    // 플레이어의 힘
    public float PlayerStr = 10f;

    // 슬라임
    public float SlimeMaxHp = 20f;
    public float SlimeAttackRange = 50f;
    public float SlimeMoveSpeed = 50f;
    public float SlimeStr = 10f;

    // 고블린
    public float GoblinMaxHp = 20f;
    public float GoblinAttackRange = 50f;
    public float GoblinMoveSpeed = 50f;
    public float GoblinStr = 10f;
    // 고블린 힐윈드 데미지 배율
    public float GoblinHillWindDamageRate = 2f;
    // 고블린 힐윈드 거리
    public float GoblinHillWindRange = 7f;
    // 고블린 힐윈드 속도
    public float GoblinHillWindTurnSpeed = 10f;
    // 고블린 러시 데미지 배율
    public float GoblinRushDamageRate = 3f;
    // 고블린 러시 시간
    public float GoblinRushTime = 1.5f;
    // 고블린 러시 속도
    public float GoblinRushSpeed = 0.2f;
    // 고블린 슬라임 소환 갯수
    public int GoblinSommonMany = 5;
    // 고블린이 스킬을 쓰는 확률
    public float GoblinSkillRate = 0.1f;
}
