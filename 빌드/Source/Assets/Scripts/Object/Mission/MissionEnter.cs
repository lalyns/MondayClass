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

    public void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            try
            {
                if (!MissionManager.Instance.CurrentMission.MissionOperate)
                {
                    GameStatus._Instance._LimitTime = 180f;
                    MissionManager.Instance.CurrentMission.OperateMission();
                }
            }
            catch
            {

            }

        }
    }
}
