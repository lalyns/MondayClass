using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    RirisFSMManager RirisFSMManager;
    ObjectPool bulletPool;

    Collider collider;

    // 스텟에서 상속받아서 사용할것
    float speed;
    float lifeTime;

    float time = 0;

    public Vector3 direction = Vector3.forward;
    public bool IsFire = false;

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
            speed = RirisFSMManager.Stat._BulletSpeed;
            lifeTime = RirisFSMManager.Stat._BulletLifeTime;
            SetBullet();
        }
        else if(time >= 1f && !IsFire)
        {
            FireBullet();
        }

        if (IsFire)
            this.transform.position += direction * speed * Time.deltaTime;

        if(time > lifeTime)
        {
            time = 0;
            IsFire = false;
            bulletPool.ItemReturnPool(this.gameObject);
        }
    }

    public void SetBullet()
    {
        if (directionType)
        {
            direction = (this.transform.position - RirisFSMManager.BulletCenter.position).normalized;
        }
        else
        {
            direction = GameLib.DirectionToCharacter(collider, RirisFSMManager.PlayerCapsule);
        }
    }

    public void FireBullet()
    {
        IsFire = true;
    }

}
