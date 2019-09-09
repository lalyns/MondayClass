using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TempMissionA : MonoBehaviour
{
    public MissionTrigger missionTrigger;

    public bool spawning = false;
    int waveLevel = 0;

    bool missionEnd = false;

    public GameObject EndTrigger;
    public GameObject PortalEffect;

    public Transform[] Wave1Locations;
    public ObjectManager.MonsterType[] Wave1MonsterType;
    public Transform[] Wave2Locations;
    public ObjectManager.MonsterType[] Wave2MonsterType;
    public Transform[] Wave3Locations;
    public ObjectManager.MonsterType[] Wave3MonsterType;

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                MissionEnd();
                missionEnd = true;
            }

        }

        if (missionEnd) return;

        if (spawning)
        {
            bool monsterCheck = GameStatus._Instance.ActivedMonsterList.Count == 0;

            if(monsterCheck)
            {
                spawning = false;
            }
        }


        if (missionTrigger.isStart)
        {
            if (!spawning)
            {
                Spawn();
                spawning = true;
            }
        }

    }

    void Spawn()
    {
        int i = 0;

        switch (waveLevel)
        {
            case 0:

                foreach (Transform location in Wave1Locations)
                {
                    switch (Wave1MonsterType[i])
                    {
                        case ObjectManager.MonsterType.Mac:
                            MonsterPoolManager._Instance._Mac.ItemSetActive(location, "monster");
                            break;
                        case ObjectManager.MonsterType.RedHat:
                            MonsterPoolManager._Instance._RedHat.ItemSetActive(location, "monster");
                            break;
                    }
                    i++;
                }

                break;
            case 1:

                foreach (Transform location in Wave2Locations)
                {
                    switch (Wave2MonsterType[i])
                    {
                        case ObjectManager.MonsterType.Mac:
                            MonsterPoolManager._Instance._Mac.ItemSetActive(location, "monster");
                            break;
                        case ObjectManager.MonsterType.RedHat:
                            MonsterPoolManager._Instance._RedHat.ItemSetActive(location, "monster");
                            break;
                    }
                    i++;
                }

                break;
            case 2:

                foreach (Transform location in Wave3Locations)
                {
                    switch (Wave3MonsterType[i])
                    {
                        case ObjectManager.MonsterType.Mac:
                            MonsterPoolManager._Instance._Mac.ItemSetActive(location, "monster");
                            break;
                        case ObjectManager.MonsterType.RedHat:
                            MonsterPoolManager._Instance._RedHat.ItemSetActive(location, "monster");
                            break;
                    }
                    i++;
                }

                break;
            case 3:
                MissionEnd();

                break;
        }

        waveLevel++;
    }

    public void MissionEnd()
    {
        EndTrigger.SetActive(true);
        PortalEffect.SetActive(true);

        GameStatus._Instance._MissionStatus = false;
        GameStatus._Instance.RemoveAllActiveMonster();
    }
}
