﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setoff()
    {
        EffectPoolManager._Instance._BossTornaedoPool.ItemReturnPool(this.gameObject);
    }
}
