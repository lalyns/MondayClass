using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public ObjectPool[] _ObjectPool;
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
        
    }

    public void CallSpawn()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        MissionData missionData = MissionManager.GetMissionData(MissionManager._Instance._CurrentMission);
        Dungeon currentDungeon = DungeonManager.GetCurrentDungeon();

        int curSpawnPos;
        int NumberOfTimesRespawn = missionData.NumberOfTimesRespawn;

        for (int i = 0; i < NumberOfTimesRespawn; i++)
        {
            curSpawnPos = 0;

            var setValue = UnityEngine.Random.Range(0, missionData.NumberOfMeleeMonsterOnWaves.Length);

            int meleeCount = missionData.NumberOfMeleeMonsterOnWaves[setValue];
            int rangeCount = missionData.NumberOfRangeMonsterOnWaves[setValue];

            Debug.Log(meleeCount + "        " + rangeCount);

            for (int j = 0; j < meleeCount; j++)
            {
                RespawnMonster(MonsterType.Melee, _RespawnPositions[curSpawnPos++]);
            }

            for (int j = 0; j < rangeCount; j++)
            {
                RespawnMonster(MonsterType.Range, _RespawnPositions[curSpawnPos++]);
            }

            Debug.Log("소환횟수 : " + i + " 시각 : " + Time.realtimeSinceStartup);

            yield return new WaitForSeconds(missionData.CycleOfTimeRespawn);
        }
    }

    /// <summary>
    /// 오브젝트 매니저의 오브젝트 리스폰 지역 변경을 도와주는 매소드
    /// </summary>
    /// <param name="respawnPositions"> 새로운 리스폰 지역의 배열 </param>
    public static void SetRespawnPosition(Transform[] respawnPositions)
    {
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

    public static void RespawnMonster(MonsterType monsterType, Transform respawnPosition)
    {
        int type = (int)monsterType;
        _Instance._ObjectPool[type].ItemSetActive(respawnPosition);
        
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

    public enum MonsterType
    {
        Melee = 0,
        Range = 1,
    }
}

