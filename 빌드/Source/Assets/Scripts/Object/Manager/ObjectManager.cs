using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public ObjectPool[] _ObjectPool;
    public Transform[] _SpawnPosition;

    // 싱글턴 선언을 위한 인스턴스
    public static ObjectManager _Instance;

    Coroutine _SpawnCoroutine;

    int loopEscapeCount = 0;
    
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
    
    public void StopSpawn()
    {
        Debug.Log("Stop");
        StopCoroutine(_SpawnCoroutine);
    }

    public void CallSpawn()
    {
        _SpawnCoroutine = StartCoroutine("TestSpawn");
    }

    IEnumerator Spawn()
    {
        MissionData missionData = MissionManager._Instance._MissionDatas[0];
        Dungeon currentDungeon = DungeonManager.GetCurrentDungeon();

        int curSpawnPos;
        int NumberOfTimesSpawn = missionData.NumberOfTimesSpawn;

        for (int i = 0; i < NumberOfTimesSpawn; i++)
        {
            curSpawnPos = 0;

            //var setValue = UnityEngine.Random.Range(0, 5);

            //int RedHatCount = missionData.RedHatCount[setValue];
            //int macCount = missionData.MacCount[setValue];
            
            for (int j = 0; j < 3; j++)
            {
                SpawnMonster(MonsterType.RedHat, _SpawnPosition[curSpawnPos++]);
            }

            for (int j = 0; j < 2; j++)
            {
                SpawnMonster(MonsterType.Mac, _SpawnPosition[curSpawnPos++]);
            }

            //Debug.Log("소환횟수 : " + i + " 시각 : " + Time.realtimeSinceStartup);

            yield return new WaitForSeconds(missionData.CycleOfTimeRespawn);
        }
    }

    IEnumerator TestSpawn()
    {
        int curSpawnPos = 0;
        int level = 0;
        int maxLevel = 3;
        int wave = 0;
        int maxWave = 1;

        Vector3[] set = new Vector3[7];
        set[0] = new Vector3(3, 2, 0);

        set[1] = new Vector3(3, 2, 0);
        set[2] = new Vector3(2, 1, 1);

        set[3] = new Vector3(3, 2, 0);
        set[4] = new Vector3(3, 2, 0);
        set[5] = new Vector3(3, 2, 0);
        set[6] = new Vector3(1, 1, 2);

        int curSet = 0;

        for(int i=0; i<3;)
        {
            if (i == 0)
            {
                Debug.Log("1레벨 1웨이브 시작");
                int remainMonster = (int)(set[curSet].x + set[curSet].y + set[curSet].z);

                if (set[curSet].x > 0)
                {
                    set[curSet].x -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.RedHat, _SpawnPosition[curSpawnPos++]);


                }

                if (set[curSet].y > 0)
                {
                    set[curSet].y -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.Mac, _SpawnPosition[curSpawnPos++]);


                }

                if (set[curSet].z > 0)
                {
                    set[curSet].z -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.Tiber, _SpawnPosition[curSpawnPos++]);

                }

                if(remainMonster == 0)
                {
                    i++;
                    curSet++;
                    curSpawnPos = 0;

                    Debug.Log("1레벨 1웨이브 끝");
                    yield return new WaitForSeconds(20f);
                    continue;
                }
            }

            if (i == 1)
            {
                Debug.Log(curSet);
                Debug.Log(string.Format("2레벨 {0}웨이브 시작", wave + 1));

                int remainMonster = (int)(set[curSet].x + set[curSet].y + set[curSet].z);

                if (set[curSet].x > 0)
                {
                    set[curSet].x -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.RedHat, _SpawnPosition[curSpawnPos++]);


                }

                if (set[curSet].y > 0)
                {
                    set[curSet].y -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.Mac, _SpawnPosition[curSpawnPos++]);


                }

                if (set[curSet].z > 0)
                {
                    set[curSet].z -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.Tiber, _SpawnPosition[curSpawnPos++]);

                }

                if (remainMonster == 0)
                {
                    if (wave == 1)
                    {
                        Debug.Log(string.Format("2레벨 {0}웨이브 끝", wave + 1));
                        i++;
                        wave = 0;
                        curSet++;
                        curSpawnPos = 0;
                        continue;
                    }

                    else if(wave == 0)
                    {
                        Debug.Log(string.Format("2레벨 {0}웨이브 끝", wave + 1));
                        wave++;
                        curSet++;
                        curSpawnPos = 0;
                        yield return new WaitForSeconds(4f);
                        continue;
                    }

                }
            }

            if (i == 2)
            {
                Debug.Log(curSet);
                Debug.Log(string.Format("3레벨 {0}웨이브 시작", wave + 1));

                int remainMonster = (int)(set[curSet].x + set[curSet].y + set[curSet].z);

                if (set[curSet].x > 0)
                {
                    set[curSet].x -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.RedHat, _SpawnPosition[curSpawnPos++]);


                }

                if (set[curSet].y > 0)
                {
                    set[curSet].y -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.Mac, _SpawnPosition[curSpawnPos++]);


                }

                if (set[curSet].z > 0)
                {
                    set[curSet].z -= 1;

                    remainMonster--;
                    SpawnMonster(MonsterType.Tiber, _SpawnPosition[curSpawnPos++]);

                }

                if (remainMonster == 0)
                {
                    if (wave == 3)
                    {
                        Debug.Log(string.Format("3레벨 {0}웨이브 끝", wave + 1));
                        i++;
                        wave = 0;
                        curSet++;
                        curSpawnPos = 0;
                        yield return new WaitForSeconds(20f);
                        continue;
                    }

                    else if (wave < 4)
                    {
                        Debug.Log(string.Format("3레벨 {0}웨이브 끝", wave + 1));
                        wave++;
                        curSet++;
                        curSpawnPos = 0;
                        yield return new WaitForSeconds(4f);
                        continue;
                    }

                }
            }
        }

        Debug.Log("섬멸 끝");
        yield return new WaitForSeconds(4f);
    }

    /// <summary>
    /// 오브젝트 매니저의 오브젝트 리스폰 지역 변경을 도와주는 매소드
    /// </summary>
    /// <param name="spawnPosition"> 새로운 리스폰 지역의 배열 </param>
    public static void SetSpawnPosition(Transform[] spawnPosition)
    {
        _Instance._SpawnPosition = spawnPosition;
    }

    /// <summary>
    /// 오브젝트 매니저의 현재 오브젝트 리스폰 지역 정보를 반환하는 매소드
    /// </summary>
    /// <returns> 오브젝트 매니저의 현재 오브젝트 리스폰 지역 </returns>
    public static Transform[] GetSpawnPosition()
    {
        return _Instance._SpawnPosition;
    }

    public static void SpawnMonster(MonsterType monsterType, Transform spawnPosition)
    {
        int type = (int)monsterType;
        _Instance._ObjectPool[type].ItemSetActive(spawnPosition);
        
    }

    /// <summary>
    /// 오브젝트를 풀로 반환하는 함수
    /// </summary>
    /// <param name="go"> 풀로 반환을 할 GameObject </param>
    public static void ReturnPoolMonster(GameObject go, MonsterType monster)
    {
        _Instance._ObjectPool[(int)monster].ItemReturnPool(go);
    }

    /// <summary>
    /// 모든 몬스터를 풀에 반환하는 함수
    /// </summary>
    public static void ReturnPoolAllMonster()
    {
        for(int i = 0; i < (int)MonsterType.Length ; i++)
        {
            while(_Instance._ObjectPool[i]._ActiveItem.Count != 0)
            {
                _Instance._ObjectPool[i].ItemReturnPool(
                    _Instance._ObjectPool[i]._ActiveItem.First.Value);

                _Instance.loopEscapeCount++;
                if (_Instance.loopEscapeCount > 1000)
                {
                    break;
                }
            }
        }
    }

    public enum MonsterType
    {
        RedHat = 0,
        Mac = 1,
        Tiber = 2,
        Length
    }

    public enum ObjectType
    {

    }

}

