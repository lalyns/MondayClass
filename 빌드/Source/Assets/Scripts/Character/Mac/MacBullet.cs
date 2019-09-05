using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacBullet : MonoBehaviour
{
    public GameObject _SetEffect;
    public ParticleSystem[] _SetEffectParticles;

    public GameObject _MoveEffect;
    public GameObject _DestroyEffect;

    [System.NonSerialized] public Vector3 dir;
    public float speed = 3f;

    // 상수 목록
    public const float _CreativeTime = 0.8f;
    public const float _DestroyTime = 4.0f;
    public const float _DestroyConstTime = 0.65f;

    // 초기화 목록
    public float _PlayTime = 0.0f;
    public float _DestroyPlayTime = 0.0f;

    public bool _Move = false;
    public bool _Play = false;
    public bool _Destroy = false;

    private void Start()
    {
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
            _DestroyEffect.SetActive(true);
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
        EffectPoolManager._Instance._MacBulletPool.ItemReturnPool(this.gameObject);

        _Move = false;
        _Play = false;
        _Destroy = false;

        _PlayTime = 0;
        _DestroyPlayTime = 0;

        _SetEffect.SetActive(true);
        _MoveEffect.SetActive(false);
        _DestroyEffect.SetActive(false);
    }

    public void LookAtTarget(Transform target)
    {
        this.transform.LookAt(target);
    }
}
