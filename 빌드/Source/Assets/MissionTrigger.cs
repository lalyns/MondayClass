﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public bool isStart = false;

    public void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            Debug.Log("시작 가능 정보 : " + isStart);
            if (!isStart)
            {
                MissionManager._Instance.StartMission();
                isStart = true;
            }

        }
    }
}
