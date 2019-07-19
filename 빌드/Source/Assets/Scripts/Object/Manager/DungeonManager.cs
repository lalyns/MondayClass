using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _Instance;

    public Dungeon[] _Dungeons;
    public Dungeon _CurrentDungeon;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = GetComponent<DungeonManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 현재 던전의 정보를 변경하는 매소드
    /// </summary>
    /// <param name="dungeon"> 변경할 던전의 정보 </param>
    public static void SetCurrentDungeon(Dungeon dungeon)
    {
        Debug.Log("현재 던전 정보를 변경합니다.");
        _Instance._CurrentDungeon = dungeon;
    }

    /// <summary>
    /// 현재 던전의 정보를 반환하는 매소드
    /// </summary>
    /// <returns> 현재 던전의 정보 </returns>
    public static Dungeon GetCurrentDungeon()
    {
        return _Instance._CurrentDungeon;
    }

    /// <summary>
    /// 선택지가 선택된 후 던전을 생성하는 매소드
    /// </summary>
    /// <param name="missionData"> 정해진 미션 </param>
    public static Dungeon CreateDungeon(MissionData missionData)
    {
        Debug.Log("새로운 던전을 생성합니다.");

        Dungeon dungeon = _Instance.SetDungeon();

        DungeonManager.SetCurrentDungeon(dungeon);
        // 던전의 미션 데이터를 설정합니다.
        dungeon.SetMissionData(missionData);

        // 오브젝트에게 리스폰 지역변경을 요청합니다.
        ObjectManager.SetRespawnPosition(dungeon._RespawnPositions);

        return dungeon;
    }

    /// <summary>
    /// 던전 변경에 대한 함수. 연출도 여기서 처리할 것
    /// </summary>
    public static void ChangeDungeon(Dungeon dungeon)
    {
        Debug.Log("던전을 변경합니다.");
    }

    /// <summary>
    /// 던전을 랜덤으로 정하는 매소드.
    /// </summary>
    /// <returns> 던전의 정보 </returns>
    public Dungeon SetDungeon()
    {
        var temp = UnityEngine.Random.Range(0, 999999) % _Dungeons.Length;
        return _Dungeons[temp];
    }

    /// <summary>
    /// 현재 던전의 클리어를 알려줍니다.
    /// </summary>
    public void DungeonClearCallBack()
    {

    }
}
