using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberHIT : TiberFSMState
{
    public bool hitEnd = false;

    public bool isBelowHalf = false;

    public override void BeginState()
    {
        base.BeginState();

        Vector3 direction = (_manager.PlayerCapsule.transform.forward).normalized;
        direction.y = 0;

        if(_manager.Stat.Hp/_manager.Stat.MaxHp < 0.5f && !isBelowHalf)
        {
            isBelowHalf = true;
            var voice = _manager.sound.monsterVoice;
            voice.PlayMonsterVoice(gameObject, voice.tiberDamageVoice);
        }

        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));

        _manager.agent.acceleration = 0;
        _manager.agent.velocity = Vector3.zero;
    }

    public override void EndState()
    {
        base.EndState();

        hitEnd = false;

        _manager.CurrentAttackType = AttackType.NONE;
        _manager.isChange = false;
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
        //Invoke("StopCoroutinesSet", 1f);
        StopAllCoroutines();        

    }
    void StopCoroutinesSet()
    {
        StopAllCoroutines();
    }

    protected override void Update()
    {
        base.Update();

        if (hitEnd && !PlayerFSMManager.Instance.isSpecial && !PlayerFSMManager.Instance.isSkill4)
            _manager.SetState(TiberState.CHASE);

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
