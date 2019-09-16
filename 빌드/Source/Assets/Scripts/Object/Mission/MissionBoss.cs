﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBoss : Mission
{
    public static MissionBoss _Instance;

    protected override void Start()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<MissionBoss>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
