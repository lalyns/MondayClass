using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    public float _GaugeValue;

    public void GaugePlayer()
    {
        float gauge = 10;

        //조건 판단 (현재게이지 + 추가게이지량 > 최대게이지량)일때, 어떻게 처리할것인지 조건 추가
        gauge += _GaugeValue;
    }

}
