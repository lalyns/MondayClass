using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.SceneDirector;
using MC.UI;

public class DeveloperEditor : MonoBehaviour
{
    public static DeveloperEditor Instance => FindObjectOfType<DeveloperEditor>().GetComponent<DeveloperEditor>();
    
    public bool usingKeward = false;

    public static string setEditor;
    public InputField editorConsole;
    public static bool _EditorMode = false;

    bool dummySet = false;

    public GameObject _DummyLocationEffect;
    MonsterType summonType;

    // 파워업
    bool playerSet = false;
    public GameObject playerStat;

    // 스테이지 이동
    bool stageMoveSet = false;
    public GameObject stageMove;

    // 몬스터 제거/생성
    bool monsterSet = false;
    public GameObject monster;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    _EditorMode = !_EditorMode;
                    editorConsole.gameObject.SetActive(_EditorMode);
                }
            }
        }

        if (GameStatus.currentGameState != CurrentGameState.Pause)
        {
            GameManager.Instance.IsPuase = editorConsole.gameObject.activeSelf;
        }

        if (editorConsole.text != "WCMode") {
            stageMoveSet = false;
            stageMove.SetActive(stageMoveSet);
            return;
        }
        

        // 유니티 에디터에서 작동하는 에디터 기능
        if (Input.GetKey(KeyCode.LeftAlt) /*&& currentGameState == CurrentGameState.Start*/)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                stageMoveSet = !stageMoveSet;
                UserInterface.SetPointerMode(stageMoveSet);
                stageMove.SetActive(stageMoveSet);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                monsterSet = !monsterSet;
                UserInterface.SetPointerMode(monsterSet);
                monster.SetActive(monsterSet);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                playerSet = !playerSet;
                UserInterface.SetPointerMode(playerSet);
                playerStat.SetActive(playerSet);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GameManager.Instance.OnInspectating = !GameManager.Instance.OnInspectating;
            }
        }

        if (dummySet)
        {
            SummonEffect();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SummonMonster(summonType);
            }
        }

    }

    #region Move
    public void MoveSetSelector()
    {
        MissionManager.ExitMission();
        MissionManager.PopUpMission();
        stageMoveSet = false;
        stageMove.SetActive(stageMoveSet);
        UserInterface.SetPointerMode(stageMoveSet);
    }
    public void MoveStage1()
    {
        MCSceneManager.Instance.NextScene(MCSceneManager.ANNIHILATION, 1f, true);
        stageMoveSet = false;
        stageMove.SetActive(stageMoveSet);
        UserInterface.SetPointerMode(stageMoveSet);
    }
    public void MoveStage2()
    {
        MCSceneManager.Instance.NextScene(MCSceneManager.SURVIVAL, 1f, true);
        stageMoveSet = false;
        stageMove.SetActive(stageMoveSet);
        UserInterface.SetPointerMode(stageMoveSet);
    }
    public void MoveStage3()
    {
        MCSceneManager.Instance.NextScene(MCSceneManager.DEFENCE, 1f, true);
        stageMoveSet = false;
        stageMove.SetActive(stageMoveSet);
        UserInterface.SetPointerMode(stageMoveSet);
    }
    public void MoveStageBoss()
    {
        MCSceneManager.Instance.NextScene(MCSceneManager.BOSS, 1f, true);
        stageMoveSet = false;
        stageMove.SetActive(stageMoveSet);
        UserInterface.SetPointerMode(stageMoveSet);
    }

    public void StageClear()
    {
        PlayerFSMManager.Instance.CurrentClear = Random.Range((int)0, (int)2);
        PlayerFSMManager.Instance.SetState(PlayerState.CLEAR);
        MissionManager.Instance.CurrentMission.ClearMission();
        MissionManager.Instance.CurrentMission.missionEnd = true;
        stageMoveSet = false;
        stageMove.SetActive(stageMoveSet);
        UserInterface.SetPointerMode(stageMoveSet);
    }
    #endregion

    #region Monster
    public void CreateMac()
    {
        SummonReady(MonsterType.Mac);
        monster.SetActive(false);
    }
    public void CreateRedHat()
    {
        SummonReady(MonsterType.RedHat);
        monster.SetActive(false);
    }
    public void CreateTiber()
    {
        SummonReady(MonsterType.Tiber);
        monster.SetActive(false);
    }
    
    public void DestroyAll()
    {
        usingKeward = true;
        GameStatus.Instance.RemoveAllActiveMonster();
        monsterSet = false;
        monster.SetActive(monsterSet);
        UserInterface.SetPointerMode(monsterSet);
    }

    public void SummonReady(MonsterType type)
    {
        //Debug.Log("지정소환준비");
        summonType = type;
        dummySet = true;
        _EditorMode = true;
        _DummyLocationEffect.SetActive(true);
        UserInterface.SetPointerMode(true);
    }

    public void SummonEffect()
    {
        Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z);
        Vector3 cameraForward = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));

        Ray ray = Camera.main.ScreenPointToRay(mousePoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, 1 << 17))
        {
            _DummyLocationEffect.transform.position = hit.point;
            _DummyLocationEffect.transform.position += new Vector3(0, 0.1f, 0);
        }
    }

    // 몬스터 지정소환
    public void SummonMonster(MonsterType type)
    {
        switch (type)
        {
            case MonsterType.RedHat:
                MonsterPoolManager._Instance._RedHat.ItemSetActive(
                    _DummyLocationEffect.transform.position,
                    MonsterType.RedHat);
                break;
            case MonsterType.Mac:
                MonsterPoolManager._Instance._Mac.ItemSetActive(
                    _DummyLocationEffect.transform.position,
                    MonsterType.Mac);
                break;
            case MonsterType.Tiber:
                MonsterPoolManager._Instance._Tiber.ItemSetActive(
                    _DummyLocationEffect.transform.position,
                    MonsterType.Tiber);
                break;
        }
        monsterSet = false;
        dummySet = false;
        _DummyLocationEffect.SetActive(false);
        UserInterface.SetPointerMode(false);
    }
    #endregion

    #region Player
    public void GaugeFull()
    {
        PlayerFSMManager.Instance.SpecialGauge = 100.0f;
    }

    public void STRUp()
    {
        if (GameSetting.rewardAbillity.strLevel >= 8) return;

        GameSetting.rewardAbillity.strLevel++;
    }
    public void STRDown()
    {
        if (GameSetting.rewardAbillity.strLevel <= 0) return;

        GameSetting.rewardAbillity.strLevel--;
    }

    public void DEFUp()
    {
        if (GameSetting.rewardAbillity.defLevel >= 8) return;

        GameSetting.rewardAbillity.defLevel++;
    }
    public void DEFDown()
    {
        if (GameSetting.rewardAbillity.defLevel <= 0) return;

        GameSetting.rewardAbillity.defLevel--;
    }

    public void HpUp()
    {
        if (GameSetting.rewardAbillity.hpLevel >= 8) return;

        GameSetting.rewardAbillity.hpLevel++;
    }
    public void HpDown()
    {
        if (GameSetting.rewardAbillity.hpLevel <= 0) return;

        GameSetting.rewardAbillity.hpLevel--;
    }

    public void Skill1DamageUp()
    {
        if (GameSetting.rewardAbillity.skill1DMGLevel >= 8) return;

        GameSetting.rewardAbillity.skill1DMGLevel++;
    }
    public void Skill1DamageDown()
    {
        if (GameSetting.rewardAbillity.skill1DMGLevel <= 0) return;

        GameSetting.rewardAbillity.skill1DMGLevel--;
    }

    public void Skill2DamageUp()
    {
        if (GameSetting.rewardAbillity.skill2DMGLevel >= 8) return;

        GameSetting.rewardAbillity.skill2DMGLevel++;
    }
    public void Skill2DamageDown()
    {
        if (GameSetting.rewardAbillity.skill2DMGLevel <= 0) return;

        GameSetting.rewardAbillity.skill2DMGLevel--;
    }

    public void Skill3DamageUp()
    {
        if (GameSetting.rewardAbillity.skill3DMGLevel >= 8) return;

        GameSetting.rewardAbillity.skill3DMGLevel++;
    }
    public void Skill3DamageDown()
    {
        if (GameSetting.rewardAbillity.skill3DMGLevel <= 0) return;

        GameSetting.rewardAbillity.skill3DMGLevel--;
    }

    public void Skill1BounceUp()
    {
        if (GameSetting.rewardAbillity.skill1BounceLevel >= 8) return;

        GameSetting.rewardAbillity.skill1BounceLevel++;
    }
    public void Skill1BounceDown()
    {
        if (GameSetting.rewardAbillity.skill1BounceLevel <= 0) return;

        GameSetting.rewardAbillity.skill1BounceLevel--;
    }

    public void Skill3SpeedUp()
    {
        if (GameSetting.rewardAbillity.skill3TurnLevel >= 8) return;

        GameSetting.rewardAbillity.skill3TurnLevel++;
    }
    public void Skill3SpeedDown()
    {
        if (GameSetting.rewardAbillity.skill3TurnLevel <= 0) return;

        GameSetting.rewardAbillity.skill3TurnLevel--;
    }

    public void PlayerExit()
    {
        playerSet = false;
        UserInterface.SetPointerMode(false);
        playerStat.SetActive(false);
    }
    #endregion
}
