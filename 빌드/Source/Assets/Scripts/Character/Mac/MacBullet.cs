﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacBullet : MonoBehaviour
{
    public enum BulletType { Normal, Skill }
    public BulletType _Type;

    public GameObject _CreateEffect;
    public ParticleSystem[] _CreateEffectParticles;

    public GameObject _MoveEffect;
    public GameObject _DestroyEffect;
    public ParticleSystem[] _DestroyEffectParticles;

    [HideInInspector] public Vector3 dir;
    public float speed = 3f;

    // 상수 목록
    [HideInInspector] public float _CreativeTime;
    [HideInInspector] public float _DestroyTime;
    [HideInInspector] public float _DestroyDelay;

    // 초기화 목록
    [HideInInspector] public float _PlayTime = 0.0f;
    [HideInInspector] public float _DestroyPlayTime = 0.0f;

    [HideInInspector] public bool _Move = false;
    [HideInInspector] public bool _SetPlay = false;
    [HideInInspector] public bool _Destroy = false;

    private void Start()
    {
        switch (_Type)
        {
            case BulletType.Normal:
                _CreativeTime = 0.800f;
                _DestroyTime = 4.000f;
                _DestroyDelay = 0.650f;
                break;
            case BulletType.Skill:
                _CreativeTime = 1.000f;
                _DestroyTime = 5.000f;
                _DestroyDelay = 1.100f;
                break;
        }

        _CreateEffect.SetActive(true);
        _MoveEffect.SetActive(false);
        _DestroyEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_Move) return;

        _PlayTime += Time.deltaTime;

        if(_PlayTime < _CreativeTime)
        {
            if (!_SetPlay)
            {
                PlayEffect(_CreateEffectParticles);
                _SetPlay = !_SetPlay;
            }
        }

        if(_PlayTime > _CreativeTime && _PlayTime < _CreativeTime + _DestroyTime)
        {
            _CreateEffect.SetActive(false);
            _MoveEffect.SetActive(true);

            if(!_Destroy)
                this.transform.position += dir * speed * Time.deltaTime;
        }

        if(_PlayTime > _CreativeTime + _DestroyTime)
        {
            _MoveEffect.SetActive(false);
            _DestroyEffect.SetActive(true);

            if (_Destroy) return;

            PlayEffect(_DestroyEffectParticles);
            _Destroy = true;
        }

        if (_Destroy) _DestroyPlayTime += Time.deltaTime;

        if (_DestroyPlayTime > _DestroyDelay)
        {
            _DestroyPlayTime = 0;
            EffectReturnPool();
        }
    }

    public void PlayEffect(ParticleSystem[] playlist)
    {
        foreach (ParticleSystem particle in playlist)
        {
            particle.Play();
        }
    }

    public void EffectReturnPool()
    {
        ObjectPool pool = null;

        switch (_Type)
        {
            case BulletType.Normal:
                pool = EffectPoolManager._Instance._MacBulletPool;
                break;
            case BulletType.Skill:
                pool = EffectPoolManager._Instance._MacSkillPool;
                break;
        }


        pool.ItemReturnPool(this.gameObject);

        _Move = false;
        _SetPlay = false;
        _Destroy = false;

        _PlayTime = 0;
        _DestroyPlayTime = 0;

        _CreateEffect.SetActive(true);
        _MoveEffect.SetActive(false);
        _DestroyEffect.SetActive(false);

    }

    public void LookAtTarget(Transform target)
    {
        this.transform.LookAt(target);
    }
}
