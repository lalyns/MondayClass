using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class MacBullet : MonoBehaviour
{
    public enum MacBulletType { Normal, Skill }
    public MacBulletType _Type;

    public GameObject _CreateEffect;
    public ParticleSystem[] _CreateEffectParticles;

    public GameObject _MoveEffect;
    public GameObject _DestroyEffect;
    public ParticleSystem[] _DestroyEffectParticles;

    [HideInInspector] public Vector3 dir;
    public float speed = 3f;

    public MacFSMManager mac;

    // 상수 목록
    public float _CreativeTime;
    public float _DestroyTime;
    public float _DestroyDelay;

    // 초기화 목록
    public float _PlayTime = 0.0f;
    public float _DestroyPlayTime = 0.0f;

    public bool _Move = false;
    public bool _SetPlay = false;
    public bool _Destroy = false;
    private bool _Dameged = false;

    private bool _SoundPlay = false;

    public void OnEnable()
    {
    }

    private void Start()
    {
        switch (_Type)
        {
            case MacBulletType.Normal:
                _CreativeTime = 0.800f;
                _DestroyTime = 2.000f;
                _DestroyDelay = 0.650f;
                break;
            case MacBulletType.Skill:
                _CreativeTime = 1.000f;
                _DestroyTime = 7.000f;
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

        if (_PlayTime < _CreativeTime)
        {
            if (!_SetPlay)
            {
                PlayEffect(_CreateEffectParticles);
                _SetPlay = !_SetPlay;

                transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
            }
        }

        if (_PlayTime > _CreativeTime && _PlayTime < _CreativeTime + _DestroyTime)
        {
            _CreateEffect.SetActive(false);
            _MoveEffect.SetActive(true);


            if (!_Destroy)
            {
                this.transform.position += dir * speed * Time.deltaTime;
            }
        }
       
        if (_PlayTime > _CreativeTime + _DestroyTime && !_Destroy)
        {
            _MoveEffect.SetActive(false);
            _DestroyEffect.SetActive(true);

            PlayEffect(_DestroyEffectParticles);
            _Destroy = true;
        }

        if (_Destroy)
        {
            if (!_SoundPlay)
            {
                _SoundPlay = true;
                var sound = mac._Sound.monsterSFX;
                if (_Type == MacBulletType.Skill)
                {
                    sound.StopMonsterSFX(this.gameObject, sound.macBigBallMove);
                    sound.PlayMonsterSFX(this.gameObject, sound.macBigBallHit);
                }
                else
                {
                    sound.PlayMonsterSFX(this.gameObject, sound.macSmallBallHit);
                }
            }
            _DestroyPlayTime += Time.deltaTime;
        }

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
            case MacBulletType.Normal:
                pool = MonsterEffects.Instance.macBulletPool;
                break;
            case MacBulletType.Skill:
                pool = MonsterEffects.Instance.macSkillPool;
                break;
        }


        pool.ItemReturnPool(this.gameObject);

        _Move = false;
        _SetPlay = false;
        _Destroy = false;
        _SoundPlay = false;

        _PlayTime = 0;
        _DestroyPlayTime = 0;

        _CreateEffect.SetActive(true);
        _MoveEffect.SetActive(false);
        _DestroyEffect.SetActive(false);

        mac = null;
        _Dameged = false;
    }

    public void LookAtTarget(Vector3 target)
    {
        this.transform.LookAt(target);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_Dameged) return;

        if (other.transform.tag == "Player")
        {
            if (_Type == MacBulletType.Normal)
            {
                float damage = mac.Stat.damageCoefiiecient[0] * 0.01f *
                    (mac.Stat.Str + mac.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                    - PlayerFSMManager.Instance.Stat.Defense;

                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", mac.Stat, damage);
                Invoke("AttackSupport", 0.5f);
                _Dameged = true;

                var sound = mac._Sound.monsterSFX;
                sound.PlayMonsterSFX(this.gameObject, sound.macSmallBallHit);

                if (!_Destroy)
                {
                    _MoveEffect.SetActive(false);
                    _DestroyEffect.SetActive(true);

                    PlayEffect(_DestroyEffectParticles);
                    _Destroy = true;
                }
            }
            if (_Type == MacBulletType.Skill)
            {
                float damage = mac.Stat.damageCoefiiecient[1] * 0.01f *
                    (mac.Stat.Str + mac.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                    - PlayerFSMManager.Instance.Stat.Defense;

                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", mac.Stat, damage);
                Invoke("AttackSupport", 0.5f);
                _Dameged = true;

            }
            

        }

        if(other.transform.tag == "DreamPillar")
        {
            _Dameged = true;

            if (!_Destroy)
            {
                _MoveEffect.SetActive(false);
                _DestroyEffect.SetActive(true);

                PlayEffect(_DestroyEffectParticles);
                _Destroy = true;
            }
        }
    }
    public void AttackSupport()
    {
        Debug.Log("attackCall");
        UserInterface.Instance.UIPlayer.hpBar.HitBackFun();
    }
}
    