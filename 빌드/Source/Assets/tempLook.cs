using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempLook : MonoBehaviour
{
    public Transform target;

    public float _Time = 0;
    public float _DashReadyTime = 1.5f;
    public float _DashTime = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _Time += Time.deltaTime;
        if (_Time < _DashReadyTime)
        {

            transform.LookAt(target);
            Vector3 rotation = transform.eulerAngles;
            rotation.x = 0;

            transform.eulerAngles = rotation;
        }

        if (_Time > _DashReadyTime && _Time < _DashReadyTime + _DashTime)
        {

        }

        if (_Time > _DashReadyTime + _DashTime)
        {
            EffectEnd();
        }
    }

    public void EffectEnd()
    {
        _Time = 0;
        EffectPoolManager._Instance._RedHatEffectPool.ItemReturnPool(this.gameObject);
    }
}
