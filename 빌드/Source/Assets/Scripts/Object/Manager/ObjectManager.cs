using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public ObjectPool _ObjectPool;
    public Transform[] _RespawnPos;

    // 싱글턴 선언을 위한 인스턴스
    public static ObjectManager _Instance;
    
    public static ObjectManager GetObjectManager()
    {
        return _Instance;
    }

    private void Awake()
    {
        if(_Instance == null)
        {
            _Instance = GetComponent<ObjectManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _ObjectPool.ItemSetActive(_RespawnPos[UnityEngine.Random.Range(0,_RespawnPos.Length)]);
        }
    }

    /// <summary>
    /// 오브젝트를 풀로 반환하기위한 매니저 지원함수
    /// </summary>
    /// <param name="go"> 풀로 반환을 할 GameObject </param>
    public static void ReturnPool(GameObject go)
    {
        _Instance._ObjectPool.ItemReturnPool(go);
    }


}

