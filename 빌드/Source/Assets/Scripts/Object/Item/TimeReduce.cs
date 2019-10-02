﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReduce : MonoBehaviour
{
    
    [Header("true = 초기화, false = 절반")]
    public bool isMax;

    PlayerFSMManager player;
    SphereCollider sphere;
    BoxCollider box;
    ParticleSystem particle;

    float _time;
    public float CoolTime = 10f;
    private void Awake()

    {
        player = PlayerFSMManager.Instance;

        sphere = GetComponent<SphereCollider>();
        box = GetComponentInChildren<BoxCollider>();

        particle = GetComponentInChildren<ParticleSystem>();

        particle.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!box.gameObject.activeSelf)
        {
            _time += Time.deltaTime;

            if (_time >= CoolTime)
            {
                _time = 0;
                box.gameObject.SetActive(true);
                sphere.enabled = true;

                particle.gameObject.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if (isMax)
                player.SkillCoolReset();
            else
                player.SkillCoolHalfReset();

            sphere.enabled = false;
            box.gameObject.SetActive(false);
            particle.gameObject.SetActive(true);
        }
    }

}
