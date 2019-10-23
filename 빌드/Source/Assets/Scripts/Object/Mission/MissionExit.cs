using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;

public class MissionExit : MonoBehaviour
{
    public GameObject _PortalEffect;
    public GameObject _BossPortalEffect;

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

    bool isEnd = false;
    /// <summary>
    /// 플레이어가 던전 출구에 도착했을때 호출함
    /// </summary>
    /// <param name="other"> 충돌체 </param>
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if (!isEnd) {
                var sound = MCSoundManager.Instance.objectSound.objectSFX;

                sound.PlaySound(this.gameObject, sound.portalEnter);
                MissionManager.ExitMission();
                MissionManager.PopUpMission();
                PlayerFSMManager.Instance.rigid.useGravity = false;
                isEnd = false;
            }
        }
    }
}
