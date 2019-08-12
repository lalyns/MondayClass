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
        _SpawnCoroutine = StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        MissionData missionData = MissionManager.GetMissionData(MissionManager._Instance._CurrentMission);
        Dungeon currentDungeon = DungeonManager.GetCurrentDungeon();

        int curSpawnPos;
        int NumberOfTimesSpawn = missionData.NumberOfTimesSpawn;

        for (int i = 0; i < NumberOfTimesSpawn; i++)
        {
            curSpawnPos = 0;

            var setValue = UnityEngine.Random.Range(0, missionData.NumberOfMeleeMonsterOnWaves.Length);

            int meleeCount = missionData.NumberOfMeleeMonsterOnWaves[setValue];
            int rangeCount = missionData.NumberOfRangeMonsterOnWaves[setValue];
            
            for (int j = 0; j < meleeCount; j++)
            {
                SpawnMonster(MonsterType.Melee, _SpawnPosition[curSpawnPos++]);
            }

            for (int j = 0; j < rangeCount; j++)
            {
                SpawnMonster(MonsterType.Range, _SpawnPosition[curSpawnPos++]);
            }

            //Debug.Log("소환횟수 : " + i + " 시각 : " + Time.realtimeSinceStartup);

            yield return new WaitForSeconds(missionData.CycleOfTimeRespawn);
        }
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
    /// 오브젝트를 풀로 반환하기위한 매니저 지원함수
    /// </summary>
    /// <param name="go"> 풀로 반환을 할 GameObject </param>
    public static void ReturnPoolMonster(GameObject go, bool isRange)
    {
        if (!isRange)
        {
            _Instance._ObjectPool[0].ItemReturnPool(go);
        }
        else
        {
            _Instance._ObjectPool[1].ItemReturnPool(go);
        }
    }

    public static void ReturnPoolAllMonster()
    {
        for(int i=0; i<2; i++)
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
        Melee = 0,
        Range = 1,
    }
}

