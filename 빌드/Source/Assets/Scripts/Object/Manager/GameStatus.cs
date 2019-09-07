using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus _Instance;

    public static bool _EditorMode = false;

    public GameObject _DummyLocationEffect;
    bool dummySet = false;

    public void Start()
    {
        if(_Instance == null)
        {
            _Instance = GetComponent<GameStatus>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // 게임 메뉴 정보

    // 던전 정보
    List<GameObject> _ActivedMonsterList = new List<GameObject>();

    public List<GameObject> ActivedMonsterList {
        get {
            return _ActivedMonsterList;
        }
    }

    /// <summary>
    /// 활동중인 몬스터 객체 추가 메소드, 항상 추가 해줄것.
    /// </summary>
    /// <param name="monster"></param>
    public void AddActivedMonsterList(GameObject monster)
    {
        // 해당객체가 몬스터인지 판별할것

        _ActivedMonsterList.Add(monster);

        int i = 0;
        foreach(GameObject mob in _ActivedMonsterList)
        {
            //Debug.Log(string.Format("{0} : {1} 번", mob.name, i++));
        }
    }

    /// <summary>
    /// 활동중인 몬스터 객체 리스트에서 몬스터 삭제 메소드
    /// </summary>
    /// <param name="monster"></param>
    public void RemoveActivedMonsterList(GameObject monster)
    {
        if (_ActivedMonsterList.Contains(monster))
        {
            _ActivedMonsterList.Remove(monster);
        }
    }

    public void Update()
    {

        // 유니티 에디터에서 작동하는 에디터 기능
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SummonReady();
            }
        }

        if (dummySet)
        {
            SummonEffect();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SummonMonster();
            }
        }
#endif
    }

#if UNITY_EDITOR
    public void SummonReady()
    {
        //Debug.Log("지정소환준비");
        dummySet = true;
        _EditorMode = true;
        _DummyLocationEffect.SetActive(true);
        GameManager.CursorMode(true);
    }

    public void SummonEffect()
    {
        Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z);
        Ray ray = Camera.main.ScreenPointToRay(mousePoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, 1 << 17))
        {
            _DummyLocationEffect.transform.position = hit.point;
            _DummyLocationEffect.transform.position += new Vector3(0, 0.1f, 0);
        }
    }

    // 몬스터 지정소환
    public void SummonMonster()
    {
        MonsterPoolManager._Instance._Mac.ItemSetActive(
            _DummyLocationEffect.transform,
            "monster");
        dummySet = false;
        _DummyLocationEffect.SetActive(false);
        GameManager.CursorMode(false);
    }
#endif

}
