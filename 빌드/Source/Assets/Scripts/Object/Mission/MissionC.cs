using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionC : Mission
{

    public ProtectedTarget protectedTarget;
    public int _ProtectedTargetHP;

    float spawnTime = 0;
    public float spawnCool = 3f;
    public Vector2[] mobSet;
    public Transform[] spawnLocation;

    int NumberOfMaxMonster = 20;

    public List<Transform> oldSpawnList = new List<Transform>();

    public override void OperateMission()
    {
        base.OperateMission();

        protectedTarget.hp = _ProtectedTargetHP;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                ClearMission();
                MissionEnd = true;
            }

        }

        if (MissionEnd) return;

        if (GameStatus._Instance.ActivedMonsterList.Count >= NumberOfMaxMonster) return;


        if (MissionOperate)
        {
            spawnTime += Time.deltaTime;

            if (spawnTime > spawnCool)
            {
                Spawn();
                spawnTime = 0;
            }
        }

        if (!MissionEnd && GameStatus._Instance._LimitTime <= 0 && protectedTarget.hp >= 0)
        {
            ClearMission();
            MissionEnd = true;
        }
    }

    public override void RestMission()
    {
        base.RestMission();

        Debug.Log(this.name.ToString() + "ResetC");

        MissionEnd = false;
        MissionOperate = false;
        Exit.Colliders.enabled = false;
        Exit._PortalEffect.SetActive(false);
        spawnTime = 0;
    }

    void Spawn()
    {
        int randSet = UnityEngine.Random.Range(0, mobSet.Length - 1);

        int loopCount = 0;
        oldSpawnList.Clear();

        for (int i=0; i < mobSet[randSet].x; loopCount++)
        {
            if(loopCount > 1000)
            {
                break;
            }

            int randPos = UnityEngine.Random.Range(0, spawnLocation.Length);

            if (oldSpawnList.Contains(spawnLocation[randPos]))
            {
                continue;
            }
            else
            {
                oldSpawnList.Add(spawnLocation[randPos]);

                MonsterPoolManager._Instance._RedHat.ItemSetActive(spawnLocation[randPos], "monster");

                i++;
            }
        }

        for (int i = 0; i < mobSet[randSet].y; loopCount++)
        {
            if (loopCount > 1000)
            {
                break;
            }

            int randPos = UnityEngine.Random.Range(0, spawnLocation.Length);

            if (oldSpawnList.Contains(spawnLocation[randPos]))
            {
                continue;
            }
            else
            {
                oldSpawnList.Add(spawnLocation[randPos]);

                MonsterPoolManager._Instance._Mac.ItemSetActive(spawnLocation[randPos], "monster");

                i++;
            }
        }
    }

}
