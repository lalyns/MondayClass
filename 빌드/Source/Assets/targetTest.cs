﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetTest : MonoBehaviour
{
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player);
    }
}
