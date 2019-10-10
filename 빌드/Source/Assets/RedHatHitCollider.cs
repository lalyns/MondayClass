using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatHitCollider : MonoBehaviour
{
    RedHatFSMManager redhat;

    // Start is called before the first frame update
    void Start()
    {
        redhat = GetComponentInParent<RedHatFSMManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHitForMonster(AttackType attackType)
    {

        if ((attackType == AttackType.ATTACK1
            || attackType == AttackType.ATTACK2
            || attackType == AttackType.ATTACK3)
            && ((int)redhat.CurrentAttackType & (int)attackType) != 0)
        {
            return;
        }

        if (redhat.CurrentState == RedHatState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            EffectPoolManager._Instance._PlayerEffectPool[0].ItemSetActive(redhat.hitLocation, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            EffectPoolManager._Instance._PlayerEffectPool[1].ItemSetActive(redhat.hitLocation, "Effect");

        redhat.CurrentAttackType = attackType;
        int value = GameLib.TransformTypeToInt(attackType);
        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

        float damage = (playerStat.Str * playerStat.dmgCoefficient[value] * 0.01f) - redhat.Stat.Defense;
        CharacterStat.ProcessDamage(playerStat, redhat.Stat, damage);

        //SetKnockBack(playerStat, value);
        Invoke("AttackSupport", 0.5f);

        if (attackType == AttackType.ATTACK1)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.15f, 0.1f));
        if (attackType == AttackType.ATTACK2)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.18f, 0.1f));
        if (attackType == AttackType.ATTACK3)
            StartCoroutine(Shake.instance.ShakeCamera(0.1f, 0.3f, 0.1f));
        if (attackType == AttackType.SKILL1)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.1f, 0.1f));
        if (attackType == AttackType.SKILL2)
            StartCoroutine(Shake.instance.ShakeCamera(0.15f, 0.1f, 0.1f));
        //if (attackType == AttackType.SKILL3)
        //    StartCoroutine(Shake.instance.ShakeCamera(0.01f, 0.01f, 0.01f));

        if (redhat.Stat.Hp > 0)
        {
            if (redhat.CurrentState == RedHatState.HIT) return;

            redhat.SetState(RedHatState.HIT);

            //플레이어 쳐다본 후
            //try
            //{
            //    transform.localEulerAngles = Vector3.zero;
            //    transform.LookAt(PlayerFSMManager.Instance.Anim.transform);
            //    //플레이어피버게이지증가?
            //}
            //catch
            //{

            //}
        }
        else
        {
            redhat.SetDeadState();
        }
    }

    public void AttackSupport()
    {
        redhat._HPBar.HitBackFun();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon" && !PlayerFSMManager.Instance.isSkill3)
        {
            if (redhat.Stat.Hp > 0)
                OnHitForMonster(PlayerFSMManager.Instance.attackType);
        }

        if (other.transform.tag == "Ball")
        {
            if (PlayerFSMManager.Instance.isNormal)
                Instantiate(redhat.hitEffect_Skill1, redhat.hitLocation.transform.position, Quaternion.identity);
            if (!PlayerFSMManager.Instance.isNormal)
                Instantiate(redhat.hitEffect_Skill1_Special, redhat.hitLocation.transform.position, Quaternion.identity);

            other.transform.gameObject.SetActive(false);

            if (redhat.Stat.Hp > 0)
            {
                //OnHit();
                OnHitForMonster(AttackType.SKILL1);
            }
        }
        if (other.transform.tag == "Skill2" && PlayerFSMManager.Instance.isSkill2)
        {
            StartCoroutine("Skill2Timer");

            redhat.SetState(RedHatState.HIT);
        }
        if (other.transform.tag == "Weapon" && PlayerFSMManager.Instance.isSkill3)
        {
            StartCoroutine("Skill3Timer");
        }
    }

    public IEnumerator Skill2Timer()
    {
        PlayerStat stat = PlayerFSMManager.Instance.Stat;
        float attackTime = 0.0f;
        while (attackTime < 0.3f)
        {

            //stats.TakeDamage(PlayerFSMManager.Instance.stats, 30);
            CharacterStat.ProcessDamage(stat, redhat.Stat, 200);
            attackTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator Skill3Timer()
    {
        while (PlayerFSMManager.Instance.isSkill3)
        {
            OnHitForMonster(AttackType.SKILL3);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Skill2")
        {
            if (redhat.Stat.Hp > 0)
            {
                try
                {
                    OnHitForMonster(AttackType.SKILL2);
                }
                catch
                {

                }
            }
        }
    }

}
