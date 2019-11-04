using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatHIT : RedHatFSMState
{
    public bool hitEnd = false;


    public override void BeginState()
    {
        base.BeginState();

        Vector3 direction = (_manager.PlayerCapsule.transform.forward).normalized;
        direction.y = 0;

        if(_manager.CurrentAttackType != AttackType.SKILL2)
            StartCoroutine(GameLib.Blinking(_manager.materialList, Color.white));

        if (!PlayerFSMManager.Instance.isSkill4)
        {
            var voice = _manager.sound.monsterVoice;
            voice.PlayMonsterVoice(this.gameObject, voice.redhatDamegedVoice);
        }

        _manager.agent.acceleration = 0;
        _manager.agent.velocity = Vector3.zero;
    }

    public override void EndState()
    {
        base.EndState();

        hitEnd = false;

        StopAllCoroutines();

        _manager.CurrentAttackType = AttackType.NONE;
        _manager.isChange = false;

        
    }

    protected override void Update()
    {
        base.Update();

        if (!hitEnd)
        {
        }

        if (hitEnd && !PlayerFSMManager.Instance.isSpecial && !PlayerFSMManager.Instance.isSkill4)
            _manager.SetState(RedHatState.CHASE);

        //if (PlayerFSMManager.Instance.isSkill4)
        //{
        //    PlayerStat playerStat = PlayerFSMManager.Instance.Stat;
        //    if (!PlayerFSMManager.Instance.isCantMove && !isHit)
        //    {
        //        _manager.Stat.TakeDamage(playerStat, playerStat.dmgCoefficient[6]);
        //        isHit = true;
        //    }
        //}

        if (_manager.Stat.Hp <= 0)
            _manager.SetDeadState();
    }
    bool isHit = false;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    public void HitEnd()
    {
        hitEnd = true;
    }
}
