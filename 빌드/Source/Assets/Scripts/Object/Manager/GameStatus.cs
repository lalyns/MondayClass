using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus _Instance;
    public static GameStatus Instance {
        get {
            if (_Instance == null)
            {
                _Instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
            }
            return _Instance;
        }
        set {
            _Instance = value;
        }
    }

    public static bool _EditorMode = false;

    public float _LimitTime = 180;
    public bool _MissionStatus = false;

    public GameObject _DummyLocationEffect;

    public PlayerFSMManager _PlayerInstance;


    bool dummySet = false;
    public void Awake()
    {
        _PlayerInstance = PlayerFSMManager.instance;
    }

    public void Start()
    {
        if(_Instance == null)
        {
            Instance = GetComponent<GameStatus>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // 게임 메뉴 정보

    // 던전 정보
    private List<GameObject> _ActivedMonsterList = new List<GameObject>();
    public List<GameObject> ActivedMonsterList {
        get {
            return _ActivedMonsterList;
        }
        internal set { _ActivedMonsterList = value; }
    }

    private int _StageLevel = 0;
    public int StageLevel {
        get { return _StageLevel; }
        internal set { _StageLevel = value; }
    }

    /// <summary>
    /// 활동중인 몬스터 객체 추가 메소드, 항상 추가 해줄것.
    /// </summary>
    /// <param name="monster"></param>
    public void AddActivedMonsterList(GameObject monster)
    {
        // 해당객체가 몬스터인지 판별할것

        ActivedMonsterList.Add(monster);

    }

    /// <summary>
    /// 활동중인 몬스터 객체 리스트에서 몬스터 삭제 메소드
    /// </summary>
    /// <param name="monster"></param>
    public void RemoveActivedMonsterList(GameObject monster)
    {
        if (_ActivedMonsterList.Contains(monster))
        {
            ActivedMonsterList.Remove(monster);
        }
    }

    /// <summary>
    /// 현재 활동중인 몬스터 전부를 풀로 반환시키는 매소드
    /// </summary>
    public void RemoveAllActiveMonster()
    {
        // 몬스터 타입에따라서
        // 풀로 반환한다.
        List<GameObject> active = ActivedMonsterList;
        //ActivedMonsterList.Clear();

        foreach(GameObject mob in active)
        {
            ObjectManager.MonsterType type = mob.GetComponent<FSMManager>().monsterType;

            switch (type)
            {
                case ObjectManager.MonsterType.RedHat:
                    MonsterPoolManager._Instance._RedHat.ItemReturnPool(mob);
                    break;
                case ObjectManager.MonsterType.Mac:
                    MonsterPoolManager._Instance._Mac.ItemReturnPool(mob);
                    break;
            }
        }

        ActivedMonsterList.Clear();
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

            if (Input.GetKeyDown(KeyCode.X))
            {
                RemoveAllActiveMonster();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                GameManager.Instance.OnInspectating = !GameManager.Instance.OnInspectating;
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

#if UNITY_STANDALONE

#endif
        if (MissionManager.Instance.CurrentMission.MissionOperate)
        {
            _LimitTime -= Time.deltaTime;
        }

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
        Vector3 cameraForward = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));

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
