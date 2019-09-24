using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDungeon : MonoBehaviour
{
    public Transform StartPos;

    public GameObject[] Waves;
    public int CurWave = 0;

    public bool MissionStart = false;
    public int MissionType = 0;
    public float MissionStartTime = 0;

    public float MissionTime = 180;

    public GameObject EndTrigger;
    public GameObject PortalEffect;

    public Transform[] LightSpherePos;
    public float LightSphereTime = 0;
    public GameObject LightSphere;
    public int clearCount;

    public Transform[] SpawnPos;
    public List<Transform> oldSpawnList = new List<Transform>();
    public float MonsterRespawnTime = 0;
    public GameObject Mac;
    public GameObject RedHat;

    void Update()
    {
        if(MissionType == 0)
        {
            Mission1();
        }

        if(MissionType == 1)
        {
            Mission2();
        }

    }

    void Mission1()
    {
        if (CurWave == 0 && !MissionStart) return;

        if (CurWave == 3 && !MonsterCheck())
        {
            MissionEnd();
        }

        if (CurWave != 0 && !MissionStart)
        {
            MissionStartTime += Time.deltaTime;

            if (MissionStartTime >= 5f)
            {
                MissionStartTime = 0;
                WaveStart();
            }
        }

        if (MissionStart)
        {
            MonsterCheck();
        }

        MissionTime -= Time.deltaTime;

        if (MissionTime < 0)
        {
            MissionTime = 0;
        }
    }

    void Mission2()
    {
        if (!MissionStart) return;

        LightSphereTime += Time.deltaTime;
        MonsterRespawnTime += Time.deltaTime;

        if (LightSphereTime >= 5f)
        {
            Debug.Log("구슬 생성");

            LightSphereTime = 0;
            int RandomPos = UnityEngine.Random.Range(0, LightSpherePos.Length);
            GameObject lightShpere = Instantiate(LightSphere, LightSpherePos[RandomPos].position, Quaternion.identity);
        }

        if(MonsterRespawnTime >= 10f)
        {
            Debug.Log("몹 생성");
            int RandomMonsterCount = UnityEngine.Random.Range(1, 4);

            int loopCount = 0;
            oldSpawnList.Clear();

            for(int i=0; i<RandomMonsterCount; loopCount++)
            {
                if (loopCount > 1000)
                {
                    break;
                }

                int RandomPos = 0;
                try
                {
                    RandomPos = UnityEngine.Random.Range(0, SpawnPos.Length);
                }
                catch
                {
                    continue;
                }

                if (oldSpawnList.Contains(SpawnPos[RandomPos]))
                {
                    continue;
                }
                else
                {
                    oldSpawnList.Add(SpawnPos[RandomPos]);

                    int RandomType = UnityEngine.Random.Range(0, 9999) % 2;
                    if (RandomType == 0)
                    {
                        //GameObject temp = Instantiate(RedHat, SpawnPos[RandomPos].position, Quaternion.identity);
                        //temp.transform.parent = Waves[0].transform;
                    }
                    else if(RandomType == 1)
                    {
                        //GameObject temp = Instantiate(Mac, SpawnPos[RandomPos].position, Quaternion.identity);
                        //temp.transform.parent = Waves[0].transform;
                    }

                    i++;
                }

            }

            MonsterRespawnTime = 0;
        }

        if(GameManager.Instance.curScore >= clearCount)
        {
            MissionStart = false;
            MissionEnd();
        }

        MissionTime -= Time.deltaTime;

        if (MissionTime < 0)
        {
            MissionTime = 0;
        }
    }

    bool MonsterCheck()
    {
        int mobCount = Waves[CurWave - 1].transform.childCount;

        if(mobCount == 0)
        {
            MissionStart = false;
            return false;
        }

        return true;

    }

    public void StartMission()
    {
        MissionStart = true;

        if (MissionType == 0) WaveStart();
    }

    public void WaveStart() {
        switch (CurWave)
        {
            case 0:
                Waves[0].SetActive(true);
                MissionStart = true;
                CurWave++;
                break;
            case 1:
                Waves[1].SetActive(true);
                MissionStart = true;
                CurWave++;
                break;
            case 2:
                Waves[2].SetActive(true);
                MissionStart = true;
                CurWave++;
                break;
        }
    }

    public void MissionEnd()
    {
        EndTrigger.SetActive(true);
        PortalEffect.SetActive(true);
        CurWave = 0;

        FSMManager[] allMob = Waves[0].GetComponentsInChildren<FSMManager>();

        foreach(FSMManager mob in allMob)
        {
            mob.SetDeadState();
        }
        
    }
}
