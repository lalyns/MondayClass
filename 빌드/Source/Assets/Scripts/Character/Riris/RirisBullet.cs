using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class RirisBullet : MonoBehaviour
{
    RirisFSMManager RirisFSMManager;
    ObjectPool bulletPool;

    Collider collider;

    public GameObject model;
    public GameObject effect1;
    public GameObject effect2;

    // 스텟에서 상속받아서 사용할것
    float speed;
    float lifeTime;

    bool isPlay = false;
    public float endDelay = 1.0f;

    float time = 0;

    public Vector3 direction = Vector3.forward;
    public bool IsFire = false;

    public bool Moving = true;

    public bool dameged = false;

    float damageType = 1;
    

    private void Start()
    {
        RirisFSMManager = GameObject.FindGameObjectWithTag("Boss").GetComponentInParent<RirisFSMManager>();
        bulletPool = BossEffects.Instance.bullet;
        collider = GetComponent<Collider>();

    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time < 1f)
        {
            model.SetActive(true);
            speed = RirisFSMManager.Stat._BulletSpeed;
            lifeTime = RirisFSMManager.Stat._BulletLifeTime;
        }
        else if(time >= 1f && !IsFire)
        {
            FireBullet();
        }

        if (IsFire && Moving)
            this.transform.position += direction * speed * Time.deltaTime;

        if(time > lifeTime && !isPlay)
        {
            effect1.GetComponentInChildren<ParticleSystem>().Play();
            isPlay = true;
        }

        if(time > lifeTime)
        {
            StartCoroutine(EffectPlayAndReturn(effect1));
            time = 0;
            IsFire = false;
            isPlay = false;
            Moving = false;
        }
    }

    public void SetBullet(Vector3 position, bool type)
    {
        if (type)
        {
            direction = (this.transform.position - position).normalized;
            transform.LookAt(transform.position + direction);
            damageType = 8f;
        }
        else
        {
            Debug.Log(collider.name);
            Debug.Log(RirisFSMManager.PlayerCapsule.name);
            direction = GameLib.DirectionToCharacter(collider, RirisFSMManager.PlayerCapsule);
            transform.LookAt(transform.position + direction);
            damageType = 1f;
        }
    }

    public void FireBullet()
    {
        IsFire = true;
    }

    public void ReturnBullet()
    {
        time = 0;
        IsFire = false;
        isPlay = false;
        Moving = false;
        dameged = false;
        bulletPool.ItemReturnPool(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Stage")
        {
            Debug.Log("Tagging : " + other.gameObject.name.ToString());
            StartCoroutine(EffectPlayAndReturn(effect2));
        }

        if (other.transform.tag == "Wall")
        {
            Debug.Log("Tagging : " + other.gameObject.name.ToString());
            StartCoroutine(EffectPlayAndReturn(effect1));
        }

        if (other.transform.tag == "Player")
        {
            float damage = RirisFSMManager.Stat.damageCoefiiecient[0] * 0.01f *
                    (RirisFSMManager.Stat.Str + RirisFSMManager.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                    - PlayerFSMManager.Instance.Stat.Defense;

            var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", RirisFSMManager.Stat, damage * damageType);
            Invoke("AttackSupport", 0.5f);
            dameged = true;

            Debug.Log("Tagging : " + other.gameObject.name.ToString());
            StartCoroutine(EffectPlayAndReturn(effect1));
        }
    }

    public void AttackSupport()
    {
        Debug.Log("attackCall");
        UserInterface.Instance.UIPlayer.hpBar.HitBackFun();
    }

    IEnumerator EffectPlayAndReturn(GameObject effect)
    {
        effect.GetComponentInChildren<ParticleSystem>().Play();
        model.SetActive(false);
        Moving = false;

        yield return new WaitForSeconds(1.0f);

        ReturnBullet();
        Moving = true;
    }
}
