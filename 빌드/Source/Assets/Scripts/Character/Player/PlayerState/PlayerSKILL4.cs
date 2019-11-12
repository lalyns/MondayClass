using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Sound;

public class PlayerSKILL4 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();

        _manager.attackType = AttackType.SKILL4;

        // 시작해볼까?
        var voice = _manager._Sound.voice;
        voice.PlayPlayerVoice(_manager.gameObject, voice.skill4CastVoice);

        GameStatus.SetCurrentGameState(CurrentGameState.Product);

        StartCoroutine(MCSoundManager.BGMFadeOut(1f));
        StartCoroutine(MCSoundManager.AmbFadeOut(1f));

        UserInterface.SetAllUserInterface(false);
        _manager.isCanUltimate = false;
        _manager.isSkill4 = true;
        _manager.Skill1Return(_manager.Skill1_Effects, _manager.Skill1_Special_Effects, _manager.isNormal);
        _manager.Skill1Return(_manager.Skill1_Shoots, _manager.Skill1_Special_Shoots, _manager.isNormal);
        _manager.Skill1PositionSet(_manager.Skill1_Effects, _manager.Skill1_Shoots, _manager.Skill1_Special_Shoots, _manager.isNormal);
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;

        StartCoroutine(MCSoundManager.BGMFadeIn(1f));
        StartCoroutine(MCSoundManager.AmbFadeIn(1f));
        GameStatus.SetCurrentGameState(CurrentGameState.Start);

        UserInterface.SetAllUserInterface(true);
        _manager.TimeLine2.SetActive(false);
        _manager.isCantMove = false;
        _manager.isAttackOne = false;
        _manager.isAttackTwo = false;
        _manager.isAttackThree = false;
        _manager.isSkill4CTime = true;
        _manager.isSkill4 = false;
        _manager.isCantMove = false;
        _manager.isSkill2Dash = false;
        foreach (GameObject mob in GameStatus.Instance.ActivedMonsterList)
        {
            Debug.Log(mob.name);
            var stat = mob.GetComponent<CharacterStat>();
            if (stat.monsterType == MonsterType.Length)
            {
                float damage = _manager.Stat.dmgCoefficient[6];

                if (stat.Hp < _manager.Stat.dmgCoefficient[6])
                {
                    damage = stat.Hp - 1;
                }

                CharacterStat.ProcessDamage(_manager.Stat, stat, damage);

            }
            else
            {
                float damage = _manager.Stat.dmgCoefficient[6];
                CharacterStat.ProcessDamage(_manager.Stat, stat, damage);
            }
            CanvasInfo.Instance.enemyHP.hpBar.HitBackFun();

        }
    }


    private void Update()
    {
        _manager.isCantMove = _time <= 17.1f ? true : false;

        _time += Time.deltaTime;
       
        if (_time >= 17.2f)
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }

    }
}
