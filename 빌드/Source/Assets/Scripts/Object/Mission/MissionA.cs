using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissionA : Mission
{
    public bool spawning = false;

    int waveLevel = 0;

    public Transform[] Wave1Locations;
    public MonsterType[] Wave1MonsterType;
    public Transform[] Wave2Locations;
    public MonsterType[] Wave2MonsterType;
    public Transform[] Wave3Locations;
    public MonsterType[] Wave3MonsterType;

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

        //if (spawning)
        //{
        //    bool monsterCheck = GameStatus.Instance.ActivedMonsterList.Count == 0;

        //    if(monsterCheck)
        //    {
        //        spawning = false;
        //    }
        //}

        if (MissionOperate)
        {
            if (!spawning)
            {
                if (SpawnMode)
                {
                    Spawn();
                }
                else
                {
                    NewSpawn();
                }
                spawning = true;
            }
        }

    }

    public void MonsterCheck()
    {
        Debug.Log("Check Call");
        if (spawning)
        {
            bool monsterCheck = GameStatus.Instance.ActivedMonsterList.Count == 0;

            if (monsterCheck)
            {
                spawning = false;
            }
        }
    }

    public override void RestMission()
    {
        base.RestMission();

        waveLevel = 0;
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
                        case MonsterType.Mac:
                            MonsterPoolManager._Instance._Mac.ItemSetActive(location, "monster");
                            break;
                        case MonsterType.RedHat:
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
                        case MonsterType.Mac:
                            MonsterPoolManager._Instance._Mac.ItemSetActive(location, "monster");
                            break;
                        case MonsterType.RedHat:
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
                        case MonsterType.Mac:
                            MonsterPoolManager._Instance._Mac.ItemSetActive(location, "monster");
                            break;
                        case MonsterType.RedHat:
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

    void NewSpawn()
    {
        switch (waveLevel)
        {
            case 0:
                StartCoroutine(SetSommonLocation(Wave1MonsterType));
                break;
            case 1:
                StartCoroutine(SetSommonLocation(Wave2MonsterType));
                break;
            case 2:
                StartCoroutine(SetSommonLocation(Wave3MonsterType));
                break;
            case 3:
                ClearMission();
                break;
        }

        waveLevel++;
    }

    //public IEnumerator SetSommonLocation(MonsterType[] monsterTypes)
    //{

    //    int i = 0;
    //    GameObject a = null;
    //    foreach (MonsterType monsterType in monsterTypes)
    //    {
    //        var position = Grid._MapPosition.Count;
    //        var rand = UnityEngine.Random.Range(0, position);

    //        switch (monsterType)
    //        {
    //            case MonsterType.Mac:
    //                a = MonsterPoolManager._Instance._Mac.ItemSetActive(Grid._MapPosition[rand], monsterType);
    //                break;
    //            case MonsterType.RedHat:
    //                a = MonsterPoolManager._Instance._RedHat.ItemSetActive(Grid._MapPosition[rand], monsterType);
    //                break;
    //            case MonsterType.Tiber:
    //                break;
    //        }

    //        yield return new WaitForSeconds(0.1f);
    //    }
        
    //}
}
