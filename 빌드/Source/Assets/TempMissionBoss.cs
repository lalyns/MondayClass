using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMissionBoss : MonoBehaviour
{
    public MissionTrigger missionTrigger;

    public static TempMissionBoss _Instance;
    private void Start()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<TempMissionBoss>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
