using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissionA : Mission
{
    public bool spawning = false;
    int waveLevel = 0;

    public Transform[] Wave1Locations;
    public ObjectManager.MonsterType[] Wave1MonsterType;
    public Transform[] Wave2Locations;
    public ObjectManager.MonsterType[] Wave2MonsterType;
    public Transform[] Wave3Locations;
    public ObjectManager.MonsterType[] Wave3MonsterType;

    protected override void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ClearMission();
                MissionEnd = true;
            }

        }

        if (MissionEnd) return;

        if (spawning)
        {
            bool monsterCheck = GameStatus._Instance.ActivedMonsterList.Count == 0;

            if(monsterCheck)
            {
                spawning = false;
            }
        }


        if (MissionOperate)
        {
            if (!spawning)
            {
                Spawn();
                spawning = true;
            }
        }

    }

    public override void RestMission()
    {
        base.RestMission();

        Debug.Log("ResetA");

        MissionOperate = false;
        Exit.Colliders.enabled = false;
        Exit._PortalEffect.SetActive(false);
        waveLevel = 0;
        MissionEnd = false;
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
                ClearMission();

                break;
        }

        waveLevel++;
    }
}
