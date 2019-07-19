using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public ObjectPool _ObjectPool;
    public Transform[] _RespawnPositions;

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
            _ObjectPool.ItemSetActive(_RespawnPositions[UnityEngine.Random.Range(0,_RespawnPositions.Length)]);
        }
    }

    /// <summary>
    /// 오브젝트 매니저의 오브젝트 리스폰 지역 변경을 도와주는 매소드
    /// </summary>
    /// <param name="respawnPositions"> 새로운 리스폰 지역의 배열 </param>
    public static void SetRespawnPosition(Transform[] respawnPositions)
    {
        Debug.Log("오브젝트매니저가 오브젝트를 소환하는 리스폰 지역을 변경합니다.");
        _Instance._RespawnPositions = respawnPositions;
    }

    /// <summary>
    /// 오브젝트 매니저의 현재 오브젝트 리스폰 지역 정보를 반환하는 매소드
    /// </summary>
    /// <returns> 오브젝트 매니저의 현재 오브젝트 리스폰 지역 </returns>
    public static Transform[] GetRespawnPosition()
    {
        return _Instance._RespawnPositions;
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

