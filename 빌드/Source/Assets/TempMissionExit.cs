using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMissionExit : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (GameManager.Instance._IsDummyScene)
            {
                MissionManager.PopUpMission();
            }
        }
    }
}
