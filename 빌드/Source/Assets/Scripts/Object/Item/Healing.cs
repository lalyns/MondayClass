using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [Tooltip("True: HOT, False: NORMAL")]
    public bool HOT;

    public int _HealValue;

    [Header("HOT가 True일떄")]
    public int _HealingTime;
    public float interval = 0.2f;

    public void HealPlayer()
    {
        // 플레이어 체력 관련 정보 삽입
        int hp = 100;

        // 플레이어 체력 회복

        if (!HOT)
        {
            //조건 판단 (현재채력 + 힐량 > 최대체력)일때, 어떻게 처리할것인지 조건 추가

            hp += _HealValue;
        }

        else
        {
            for(int i=0; i<_HealingTime; i++)
            {
                //조건 판단 (현재채력 + 힐량 > 최대체력)일때, 어떻게 처리할것인지 조건 추가

                hp += _HealValue;

                StartCoroutine("TimeWaiting");
            }
        }

        // 플레이어 체력 적용
    }

    // 회복 지연시간
    public IEnumerator TimeWaiting()
    {
        yield return new WaitForSeconds(interval);
    }
}
