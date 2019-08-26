using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    public GameObject nextDungeon;
    /// <summary>
    /// 플레이어가 던전 출구에 도착했을때 호출함
    /// </summary>
    /// <param name="other"> 충돌체 </param>
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if (GameManager._Instance._IsDummyScene)
            {
                try
                {
                    other.transform.position = nextDungeon.GetComponent<TempDungeon>().StartPos.position;
                    other.transform.LookAt(nextDungeon.GetComponent<TempDungeon>().EndTrigger.transform);
                    GameManager.stageLevel++;
                }
                catch
                {

                }
            }
            else
            {
                MissionManager.MissionClear();
            }
        }
    }
}
