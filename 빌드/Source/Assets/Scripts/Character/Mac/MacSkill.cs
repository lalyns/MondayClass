using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacSkill : MonoBehaviour
{
    public GameObject _SetEffect;
    public ParticleSystem[] _SetEffectParticles;

    public GameObject _MoveEffect;
    public GameObject _DestroyEffect;

    public Vector3 dir;
    public float speed;

    public const float _CreativeTime = 0.8f;
    public const float _DestroyTime = 4.0f;
    public const float _DestroyConstTime = 0.65f;

    // 초기화 목록
    public float _PlayTime = 0.0f;
    public float _DestroyPlayTime = 0.0f;

    public bool _Move = false;
    public bool _Play = false;
    public bool _Destroy = false;

    // Start is called before the first frame update
    void Start()
    {
        _MoveEffect.SetActive(false);
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

        if (_PlayTime < _CreativeTime)
        {
            if (_Play) return;

            foreach (ParticleSystem particle in _SetEffectParticles)
            {
                particle.Play();
            }
            _Play = !_Play;
        }

        if (_PlayTime > _CreativeTime && _PlayTime < _DestroyTime)
        {
            _SetEffect.SetActive(false);
            _MoveEffect.SetActive(true);

            this.transform.position += dir * speed * Time.deltaTime;
        }

        if (_PlayTime > _DestroyTime)
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
        EffectPoolManager._Instance._MacSkillPool.ItemReturnPool(this.gameObject);

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
