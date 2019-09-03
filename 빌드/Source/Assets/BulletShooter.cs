using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public float _Time = 0;
    public float _CreateTime = 2.0f;

    public Transform _Launcher;
    public ObjectPool _BulletPool;

    private void Update()
    {
        _Time += Time.deltaTime;

        if (_Time > _CreateTime)
        {
            _BulletPool.ItemSetActive(_Launcher);
            _Time = 0;
        }
    }
}
