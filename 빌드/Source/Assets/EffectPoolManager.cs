﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolManager : MonoBehaviour
{
    public static EffectPoolManager _Instance;

    public ObjectPool _MacBulletPool;
    public ObjectPool _BossBulletPool;

    private void Awake()
    {
        if(_Instance == null)
        {
            _Instance = this.GetComponent<EffectPoolManager>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


}
