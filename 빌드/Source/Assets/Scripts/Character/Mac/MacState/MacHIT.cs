using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacHIT : MacFSMState
{
    bool hitEnd = false;

    public override void BeginState()
    {
        base.BeginState();

        Vector3 direction = (_manager.PlayerCapsule.transform.forward).normalized;
        direction.y = 0;

        try
        {
            StartCoroutine(GameLib.Blinking(_manager.materialList, Color.white));
        }
        catch
        {

        }

        var voice = _manager.sound.monsterVoice;
        voice.PlayMonsterVoice(this.gameObject, voice.macDamageVoice);

        _manager.agent.acceleration = 0;
        _manager.agent.velocity = Vector3.zero;

    }

    public override void EndState()
    {
        base.EndState();

        hitEnd = false;

        _manager.currentAttackType = AttackType.NONE;
        _manager.isChange = false;

        StopAllCoroutines();
    }
    
    protected override void Update()
    {
        base.Update();

        if (hitEnd && !PlayerFSMManager.Instance.isSpecial && !PlayerFSMManager.Instance.isSkill4)
            _manager.SetState(MacState.CHASE);

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
