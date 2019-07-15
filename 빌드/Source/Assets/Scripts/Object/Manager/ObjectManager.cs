using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public ObjectPool _ObjectPool;
    public Transform _RespawnPos;

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _ObjectPool.ItemSetActive(_RespawnPos);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            _ObjectPool.ItemReturnPool();
        }
    }


}

