using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// C# 가비지컬렉션 회피 및 오브젝트 관리를 위한 오브젝트 풀 클래스
/// </summary>
public class ObjectPool : MonoBehaviour
{
    // 오브젝트 생성시 생성할 최소 수
    public int _InitializeItemSize;
    [Tooltip("오브젝트 풀링을 할 대상 게임 오브젝트")]
    public GameObject _PoolingItem;

    LinkedList<GameObject> _InActiveItemPool = new LinkedList<GameObject>();
    LinkedList<GameObject> _ActiveItem = new LinkedList<GameObject>();

    /// <summary>
    /// 오브젝트 생성을 위한 매소드
    /// </summary>
    public void CreateItem()
    {
        for(int i=0; i<_InitializeItemSize; i++)
        {
            
            var item = Instantiate(_PoolingItem, this.transform);
            item.SetActive(false);
            _InActiveItemPool.AddLast(item);
        }

        //for(int i=0; i<transform.childCount; i++)
        //{
        //    Debug.Log(transform.GetChild(i).name);
        //}
    }

    /// <summary>
    /// 오브젝트 풀 내부의 오브젝트 작동을 위한 매소드
    /// </summary>
    /// <param name="respawnPos"> 오브젝트 스폰 위치 </param>
    public void ItemSetActive(Transform respawnPos)
    {
        if(_InActiveItemPool.Count == 0)
        {
            CreateItem();
        }

        var item = _InActiveItemPool.First.Value;
        _InActiveItemPool.RemoveFirst();

        item.transform.localPosition = respawnPos.position;
        item.SetActive(true);

        _ActiveItem.AddLast(item);
    }

    /// <summary>
    /// 작동 중인 오브젝트의 활성화를 비활성화로 변경하는 매소드
    /// 변수 넣을예정 (활성화를 해제해야하는 경우)
    /// </summary>
    public void ItemReturnPool()
    {
        if(_ActiveItem.Count == 0)
        {
            return;
        }

        var item = _ActiveItem.First.Value;
        _ActiveItem.RemoveFirst();

        item.transform.localPosition = this.transform.position;
        item.SetActive(false);

        _InActiveItemPool.AddLast(item);
    }
}
