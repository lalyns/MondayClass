using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEnter : MonoBehaviour
{
    private Collider _Colliders;
    public Collider Colliders {
        get {
            if(_Colliders == null)
            {
                _Colliders = GetComponent<Collider>();
            }
            return _Colliders;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if (!MissionManager.Instance.CurrentMission.MissionOperate)
            {
                MissionManager.StartMission();
            }
        }
    }
}
