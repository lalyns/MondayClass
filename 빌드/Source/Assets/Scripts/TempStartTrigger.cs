using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempStartTrigger : MonoBehaviour
{
    public bool _start = true;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!_start) return;

            _start = false;
            GetComponentInParent<TempDungeon>().StartMission();
        }
    }

}
