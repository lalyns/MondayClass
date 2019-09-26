using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    public float _GaugeValue;
    PlayerFSMManager player;

    private void Awake()
    {
        player = PlayerFSMManager.instance;
    }
    public void GaugePlayer()
    {
        //조건 판단 (현재게이지 + 추가게이지량 > 최대게이지량)일때, 어떻게 처리할것인지 조건 추가
        if (player.SpecialGauge + _GaugeValue > 100)
            player.SpecialGauge = 100;
        else
            player.SpecialGauge += _GaugeValue;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GaugePlayer();
            gameObject.SetActive(false);
        }
    }
}
