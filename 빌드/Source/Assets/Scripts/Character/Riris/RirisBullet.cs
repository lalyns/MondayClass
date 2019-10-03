using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool directionType = false;

    private void Start()
    {
        RirisFSMManager = GameObject.FindGameObjectWithTag("Boss").GetComponent<RirisFSMManager>();
        bulletPool = GameObject.FindGameObjectWithTag("BossBulletPool").GetComponent<ObjectPool>();
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
            SetBullet();
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

    public void SetBullet()
    {
        if (directionType)
        {
            direction = (this.transform.position - RirisFSMManager.Pevis.position).normalized;
            transform.LookAt(transform.position + direction);
        }
        else
        {
            direction = GameLib.DirectionToCharacter(collider, RirisFSMManager.PlayerCapsule);
            transform.LookAt(transform.position + direction);
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
        bulletPool.ItemReturnPool(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Stage")
        {
            Debug.Log("Tagging : " + other.gameObject.name.ToString());

            effect2.GetComponentInChildren<ParticleSystem>().Play();
            ReturnBullet();
        }
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
