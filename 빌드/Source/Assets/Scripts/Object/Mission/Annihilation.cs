using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annihilation : MonoBehaviour
{
    private MissionManager _MissionManager;

    private void Awake()
    {
        _MissionManager = MissionManager.GetMissionManager;
    }

    private void Update()
    {
        
    }

    private bool CheckForClear()
    {
        bool isClear = false;



        return isClear;
    }

}
