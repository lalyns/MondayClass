using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionExit : MonoBehaviour
{
    public GameObject nextDungeon;

    public GameObject _PortalEffect;

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
    /// <summary>
    /// 플레이어가 던전 출구에 도착했을때 호출함
    /// </summary>
    /// <param name="other"> 충돌체 </param>
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if (GameManager._Instance._IsDummyScene)
            {
                MissionManager.Instance.CurrentMission.MissionSelect();

            }
        }
    }
}
