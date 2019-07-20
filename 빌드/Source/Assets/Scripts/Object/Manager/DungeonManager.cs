using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    private static DungeonManager _Instance;

    public delegate void DungeonClearCallback();
    public DungeonClearCallback _ClearCallback;

    public Dungeon[] _Dungeons;
    public Dungeon _CurrentDungeon;
    public Dungeon _PrevDungeon;

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

    public static DungeonManager GetDungeonManager()
    {
        return _Instance;
    }

    /// <summary>
    /// 이전 던전의 정보를 변경하는 매소드
    /// </summary>
    /// <param name="dungeon"> 변경할 던전의 정보 </param>
    public static void SetPrevDungeon(Dungeon dungeon)
    {
        Debug.Log("이전 던전 정보를 변경합니다.");
        _Instance._PrevDungeon = dungeon;
    }

    /// <summary>
    /// 이전 던전의 정보를 반환하는 매소드
    /// </summary>
    /// <returns> 현재 던전의 정보 </returns>
    public static Dungeon GetPrevDungeon()
    {
        return _Instance._PrevDungeon;
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

        // 현재 던전 및 새로운 던전 정보를 생성하고 던전 정보를 변경합니다.
        Dungeon curDungeon = GetCurrentDungeon();
        Dungeon newDungeon = _Instance.SetRandomDungeon();

        DungeonManager.SetPrevDungeon(curDungeon);
        DungeonManager.SetCurrentDungeon(newDungeon);
        
        // 던전의 미션 데이터를 설정합니다.
        newDungeon.SetMissionData(missionData);

        return newDungeon;
    }

    /// <summary>
    /// 던전을 랜덤으로 정하는 매소드.
    /// </summary>
    /// <returns> 던전의 정보 </returns>
    public Dungeon SetRandomDungeon()
    {
        var temp = UnityEngine.Random.Range(0, 999999) % _Dungeons.Length;
        return _Dungeons[temp];
    }

    /// <summary>
    /// 현재 던전의 클리어를 알려줍니다.
    /// </summary>
    public void DungeonClearCallBack()
    {
        _CurrentDungeon._ExitPosition.gameObject.SetActive(true);
    }

}
