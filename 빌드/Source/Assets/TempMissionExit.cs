using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMissionExit : MonoBehaviour
{
    public static TempMissionExit _Instance;
    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
    }

    public GameObject nextDungeon;
    public GameObject _MissionSelector;

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (GameManager.Instance._IsDummyScene)
            {
                MissionManager.PopUpMission();
            }
        }
    }

    public void MissionStart()
    {
        _MissionSelector.SetActive(true);
        GameManager.CursorMode(true);
        Time.timeScale = 0.0f;

    }

    public void NextMission()
    {
        nextDungeon.SetActive(true);

        GameStatus._Instance._PlayerInstance.
            GetComponentInChildren<Animator>().
            transform.position
            = nextDungeon.GetComponentInChildren<DungeonEnter>()
            .transform.position;

        MissionManager.Instance.CurrentMission =
            MissionManager.Instance.Missions[GameManager.stageLevel];

        _MissionSelector.SetActive(false);
        GameManager.CursorMode(false);
        Time.timeScale = 1.0f;
    }
}
