using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacAnimEvent : MonoBehaviour
{
    public int type;

    public GameObject bulletEffect;

    public Transform bulletLuancher;

    public float _Time = 0;

    public float _MakeTime = 0.5f;
    public float _DestroyTime = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (type == 0)
        {
            _Time += Time.deltaTime;

            if (_Time > _MakeTime)
            {
                
            }

        }

        if(type == 2)
        {

            if (_Time > _DestroyTime)
            {
                _Time += Time.deltaTime;
                Destroy(this.gameObject);
            }
        }
    }

    public void CastingAttack()
    {

        GameObject.Find("MacBulletPool").GetComponent<ObjectPool>().
            ItemSetActive(bulletLuancher, this.GetComponentInParent<MacFSMManager>().CC,
            this.GetComponentInParent<MacFSMManager>().PlayerCapsule);

    }
}
