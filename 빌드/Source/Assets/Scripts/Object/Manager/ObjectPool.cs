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

    public LinkedList<GameObject> _InActiveItemPool = new LinkedList<GameObject>();
    public LinkedList<GameObject> _ActiveItem = new LinkedList<GameObject>();

    private void Start()
    {
        CreateItem();
    }

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

    }

    /// <summary>
    /// 오브젝트 풀 내부의 오브젝트 작동을 위한 매소드
    /// </summary>
    /// <param name="respawnTrans"> 오브젝트 스폰 위치 </param>
    public GameObject ItemSetActive(Transform respawnTrans)
    {
        if(_InActiveItemPool.Count == 0)
        {
            CreateItem();
        }

        var item = _InActiveItemPool.First.Value;
        _InActiveItemPool.RemoveFirst();

        item.transform.position = respawnTrans.position;
        item.SetActive(true);

        _ActiveItem.AddLast(item);

        return item;
    }


    public Transform ItemSetActive(Transform respawnTrans, string type)
    {
        if (_InActiveItemPool.Count == 0)
        {
            CreateItem();
        }

        var item = _InActiveItemPool.First.Value;
        _InActiveItemPool.RemoveFirst();

        item.transform.position = respawnTrans.position;
        item.SetActive(true);

        _ActiveItem.AddLast(item);

        if(type == "monster")
        {
            GameStatus.Instance.AddActivedMonsterList(item);
            try
            {
                item.GetComponent<MacFSMManager>().SetState(MacState.POPUP);
            }
            catch
            {
                item.GetComponent<RedHatFSMManager>().SetState(RedHatState.POPUP);
            }
        }

        if(type == "Effect")
        {
            item.GetComponent<Effects>().EffectPlay();
            item.GetComponent<Effects>().targetPool = this;
        }

        return item.transform;
    }

    public GameObject ItemSetActive(Vector3 spawnPos, MonsterType type)
    {
        if (_InActiveItemPool.Count == 0)
        {
            CreateItem();
        }

        var item = _InActiveItemPool.First.Value;
        _InActiveItemPool.RemoveFirst();

        item.transform.position = spawnPos;
        item.SetActive(true);

        _ActiveItem.AddLast(item);

        GameStatus.Instance.AddActivedMonsterList(item);

        switch (type)
        {
            case MonsterType.Mac:
                item.GetComponent<MacFSMManager>().SetState(MacState.POPUP);
                break;
            case MonsterType.RedHat:
                item.GetComponent<RedHatFSMManager>().SetState(RedHatState.POPUP);
                break;
            case MonsterType.Tiber:
                item.GetComponent<TiberFSMManager>().SetState(TiberState.POPUP);
                break;

        }

        return item;
    }


    public GameObject ItemSetActive(Vector3 spawnPos, string type)
    {
        if (_InActiveItemPool.Count == 0)
        {
            CreateItem();
        }

        var item = _InActiveItemPool.First.Value;
        _InActiveItemPool.RemoveFirst();

        item.transform.position = spawnPos;
        item.SetActive(true);

        _ActiveItem.AddLast(item);

        if (type == "monster")
        {
            GameStatus.Instance.AddActivedMonsterList(item);
            try
            {
                item.GetComponent<MacFSMManager>().SetState(MacState.POPUP);
            }
            catch
            {
                item.GetComponent<RedHatFSMManager>().SetState(RedHatState.POPUP);
            }
        }

        if (type == "Effect")
        {
            item.GetComponent<Effects>().EffectPlay();
            item.GetComponent<Effects>().targetPool = this;
        }

        return item;
    }


    public void ItemSetActive(Transform respawnTrans, bool bulletType)
    {
        if (_InActiveItemPool.Count == 0)
        {
            CreateItem();
        }

        var item = _InActiveItemPool.First.Value;
        _InActiveItemPool.RemoveFirst();

        item.transform.position = respawnTrans.position;
        item.GetComponent<RirisBullet>().directionType = bulletType;
        item.SetActive(true);

        _ActiveItem.AddLast(item);
    }

    public GameObject ItemSetActive(Vector3 respawnPos)
    {
        if (_InActiveItemPool.Count == 0)
        {
            CreateItem();
        }

        var item = _InActiveItemPool.First.Value;
        _InActiveItemPool.RemoveFirst();

        item.transform.position = respawnPos;
        item.SetActive(true);

        _ActiveItem.AddLast(item);

        return item;
    }

    public void ItemSetActive(Transform respawnTrans, CharacterController start, Collider target)
    {
        if (_InActiveItemPool.Count == 0)
        {
            CreateItem();
        }

        var item = _InActiveItemPool.First.Value;
        _InActiveItemPool.RemoveFirst();

        item.transform.position = respawnTrans.position;

        try
        {
            item.GetComponent<MacBullet>().LookAtTarget(
                GameObject.FindGameObjectWithTag("Player").transform);

            item.GetComponent<MacBullet>().dir =
                GameLib.DirectionToCharacter(start, target);

            item.GetComponent<MacBullet>()._Move = true;
            item.GetComponent<MacBullet>().mac = respawnTrans.GetComponentInParent<MacFSMManager>();
        }
        catch
        {

        }

        item.SetActive(true);

        _ActiveItem.AddLast(item);
    }

    /// <summary>
    /// 작동 중인 오브젝트의 활성화를 비활성화로 변경하는 매소드
    /// 변수 넣을예정 (활성화를 해제해야하는 경우)
    /// </summary>
    /// <param name="go"> 풀에 반환할 오브젝트 </param>
    public void ItemReturnPool(GameObject go)
    {
        if(_ActiveItem.Count == 0)
        {
            return;
        }

        _ActiveItem.Remove(go);

        go.transform.localPosition = this.transform.position;
        go.SetActive(false);

        _InActiveItemPool.AddLast(go);

    }

    public void ItemReturnPool(GameObject go, MonsterType type)
    {
        if (_ActiveItem.Count == 0)
        {
            return;
        }

        _ActiveItem.Remove(go);

        GameStatus.Instance.RemoveActivedMonsterList(go);

        go.transform.localPosition = this.transform.position;
        go.SetActive(false);

        _InActiveItemPool.AddLast(go);
    }
}
