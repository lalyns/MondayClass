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
            if (!MissionManager.Instance.CurrentMission.MissionOperate &&
                !MissionManager.Instance.CurrentMission.missionEnd)
            {
                MissionManager.Instance.CurrentMission.startImage.gameObject.SetActive(true);
                MissionManager.Instance.CurrentMission.startImage.GetComponent<Animator>().Play("play");

                Invoke("CanvasOff", 5f);

                MissionManager.StartMission();
                GameStatus.SetCurrentGameState(CurrentGameState.Start);
            }
        }
    }

    public void CanvasOff()
    {
        MissionManager.Instance.CurrentMission.startImage.gameObject.SetActive(false);
    }
}
