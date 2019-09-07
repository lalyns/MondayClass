using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacBullet : MonoBehaviour
{
    public enum BulletType { Normal, Skill }
    public BulletType _Type;

    public GameObject _SetEffect;
    public ParticleSystem[] _SetEffectParticles;

    public GameObject _MoveEffect;
    public GameObject _DestroyEffect;

    [HideInInspector] public Vector3 dir;
    public float speed = 3f;

    // 상수 목록
    [HideInInspector] public float _CreativeTime = 0.8f;
    [HideInInspector] public float _DestroyTime = 4.0f;
    [HideInInspector] public float _DestroyConstTime = 0.65f;

    // 초기화 목록
    [HideInInspector] public float _PlayTime = 0.0f;
    [HideInInspector] public float _DestroyPlayTime = 0.0f;

    [HideInInspector] public bool _Move = false;
    [HideInInspector] public bool _Play = false;
    [HideInInspector] public bool _Destroy = false;

    private void Start()
    {
        _MoveEffect.SetActive(false);

        switch (_Type)
        {
            case BulletType.Normal:
                _CreativeTime = 0.8f;
                _DestroyTime = 4.0f;
                _DestroyConstTime = 0.65f;
                break;
            case BulletType.Skill:
                _CreativeTime = 1f;
                _DestroyTime = 8.0f;
                _DestroyConstTime = 0.65f;
                break;
        }

        try
        {
            _DestroyEffect.SetActive(false);
        }
        catch
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_Move) return;

        _PlayTime += Time.deltaTime;

        if(_PlayTime < _CreativeTime)
        {
            if (_Play) return;

            foreach(ParticleSystem particle in _SetEffectParticles)
            {
                particle.Play();
            }
            _Play = !_Play;
        }

        if(_PlayTime > _CreativeTime && _PlayTime < _DestroyTime)
        {
            _SetEffect.SetActive(false);
            _MoveEffect.SetActive(true);

            this.transform.position += dir * speed * Time.deltaTime;
        }

        if(_PlayTime > _DestroyTime)
        {
            _MoveEffect.SetActive(false);

            try
            {
                _DestroyEffect.SetActive(true);
            }
            catch
            {

            }

            _Destroy = true;
        }

        if (_Destroy) _DestroyPlayTime += Time.deltaTime;

        if (_DestroyPlayTime > _DestroyConstTime)
        {
            _DestroyPlayTime = 0;
            EffectReturnPool();
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
        _Play = false;
        _Destroy = false;

        _PlayTime = 0;
        _DestroyPlayTime = 0;

        _SetEffect.SetActive(true);
        _MoveEffect.SetActive(false);

        try
        {
            _DestroyEffect.SetActive(false);
        }
        catch
        {

        }

    }

    public void LookAtTarget(Transform target)
    {
        this.transform.LookAt(target);
    }
}
