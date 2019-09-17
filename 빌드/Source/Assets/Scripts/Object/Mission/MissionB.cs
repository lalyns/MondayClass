using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionB : Mission
{
    bool missionEnd = false;

    public GameObject EndTrigger;
    public GameObject PortalEffect;

    public int goalScore = 5;

    float starDropTime = 0;
    public float starDropCool = 5f;
    public GameObject star;
    public Transform[] dropLocation;

    float spawnTime = 0;
    public float spawnCool = 3f;
    public Vector2[] mobSet;
    public Transform[] spawnLocation;

    int NumberOfMaxMonster = 20;

    public List<Transform> oldSpawnList = new List<Transform>();

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        starDropTime += Time.deltaTime;
        spawnTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ClearMission();
                missionEnd = true;
            }

        }

        if (missionEnd) return;

        if (GameStatus._Instance.ActivedMonsterList.Count >= NumberOfMaxMonster) return;

        if (MissionOperate)
        {
            if (starDropTime > starDropCool)
            {
                DropStar();
                starDropTime = 0;
            }

            if(spawnTime > spawnCool)
            {
                Spawn();
                spawnTime = 0;
            }
        }

        if (GameManager._Instance.curScore >= goalScore)
        {
            ClearMission();
            missionEnd = true;
        }
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

    void DropStar()
    {
        int randPos = UnityEngine.Random.Range(0, dropLocation.Length - 1);

        GameObject temp = Instantiate(star, dropLocation[randPos].position, Quaternion.identity);
    }
}
