using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    RedHat = 0,
    Mac = 1,
    Tiber = 2,
    Length
}

public enum ObjectType
{

}

public static class GameLib
{
    // 단순하게 적을 찾고, 적에게 피해를 입히는 함수
    public static CharacterStat SimpleDamageProcess(Transform transform, float Range, string targetTag, CharacterStat ownerStat)
    {
        return AttackProcess(
            AttackTargetsByRange(transform, Range),
            targetTag, ownerStat);
    }

    public static CharacterStat SimpleDamageProcess(Transform transform, float Range, string targetTag, CharacterStat ownerStat
        , MonsterType type)
    {
        return AttackProcess(
            AttackTargetsByRange(transform, Range),
            targetTag, ownerStat, type);
    }

    // 단순하게 적을 찾고, 적에게 피해를 입히는 함수
    public static CharacterStat SimpleDamageProcess(Transform transform, float Range, string targetTag, CharacterStat ownerStat, int damage)
    {
        return AttackProcess(
            AttackTargetsByRange(transform, Range),
            targetTag, ownerStat, damage);
    }


    // 간단한 사각형 Raycasting을 하는 함수.
    public static RaycastHit[] AttackTargetsByRange(Transform transform, float Range)
    {
        return Physics.BoxCastAll(transform.position, transform.lossyScale / 2,
            Range * transform.forward);
    }
    // 여러 오브젝트들에 대해 간단한 정보로 피해를 입히는 함수.
    public static CharacterStat AttackProcess(RaycastHit[] hitObjects, string targetTag, CharacterStat ownerStat)
    {

        var color = new Color(1, 0.3725f, 0.3725f);
        PlayerFSMManager.Instance.StartCoroutine(Blinking(PlayerFSMManager.Instance.materialList, color));

        if (PlayerFSMManager.Instance.ShieldCount > 0)
        {
            PlayerFSMManager.Instance.ShieldCount--;
            return null;
        }
        else
        {
            if (PlayerFSMManager.Instance.isIDLE)
            {
                PlayerFSMManager.Instance.SetState(PlayerState.HIT);
            }

            CharacterStat lastHit = null;
            foreach (var hitObject in hitObjects)
            {
                if (hitObject.collider.gameObject.tag == targetTag)
                {
                    CharacterStat targetStat =
                        hitObject.collider.GetComponentInParent<CharacterStat>();

                    CharacterStat.ProcessDamage(ownerStat, targetStat);
                    lastHit = targetStat;
                }
            }
            return lastHit;
        }
    }

    public static CharacterStat AttackProcess(RaycastHit[] hitObjects, string targetTag, CharacterStat ownerStat, MonsterType type)
    {

        var color = new Color(1, 0.3725f, 0.3725f);
        PlayerFSMManager.Instance.StartCoroutine(Blinking(PlayerFSMManager.Instance.materialList, color));

        if (PlayerFSMManager.Instance.ShieldCount > 0)
        {
            PlayerFSMManager.Instance.ShieldCount--;
            return null;
        }
        else
        {
            if (PlayerFSMManager.Instance.isIDLE)
            {
                PlayerFSMManager.Instance.SetState(PlayerState.HIT);
            }

            CharacterStat lastHit = null;
            foreach (var hitObject in hitObjects)
            {
                if (hitObject.collider.gameObject.tag == targetTag)
                {
                    CharacterStat targetStat =
                        hitObject.collider.GetComponentInParent<CharacterStat>();

                    CharacterStat.ProcessDamage(ownerStat, targetStat);
                    lastHit = targetStat;

                    if (type == MonsterType.RedHat)
                    {
                        Transform transform = 
                            EffectPoolManager._Instance._RedHatAttackEffect.
                            ItemSetActive(PlayerFSMManager.instance.Anim.transform, "Effect");

                        transform.rotation = ownerStat.transform.rotation;
                    }
                }
            }
            return lastHit;
        }
    }
    
    // 여러 오브젝트들에 대해 간단한 정보로 피해를 입히는 함수.
    public static CharacterStat AttackProcess(RaycastHit[] hitObjects, string targetTag, CharacterStat ownerStat, int damage)
    {
        var color = new Color(1, 0.3725f, 0.3725f);
        PlayerFSMManager.Instance.StartCoroutine(Blinking(PlayerFSMManager.Instance.materialList, color));

        if (PlayerFSMManager.Instance.ShieldCount > 0)
        {
            PlayerFSMManager.Instance.ShieldCount--;
            return null;
        }
        else
        {
            if (PlayerFSMManager.Instance.isIDLE)
            {
                PlayerFSMManager.Instance.SetState(PlayerState.HIT);
            }
            CharacterStat lastHit = null;
            foreach (var hitObject in hitObjects)
            {
                if (hitObject.collider.gameObject.tag == targetTag)
                {
                    CharacterStat targetStat =
                        hitObject.collider.GetComponentInParent<CharacterStat>();

                    CharacterStat.ProcessDamage(ownerStat, targetStat, damage);
                    lastHit = targetStat;
                }
            }
            return lastHit;
        }
    }

    public static void CKMove(this CharacterController cc,
        Vector3 targetPosition,
        CharacterStat stat)
    {
        Transform t = cc.transform;

        Vector3 deltaMove = Vector3.zero;
        Vector3 moveDir = targetPosition - t.position;
        moveDir.y = 0.0f;
        if (moveDir != Vector3.zero)
        {
            t.rotation = Quaternion.RotateTowards(
                t.rotation,
                Quaternion.LookRotation(moveDir),
                stat.TurnSpeed * Time.deltaTime);
        }

        Vector3 nextMove = Vector3.MoveTowards(
            t.position,
            targetPosition,
            stat.MoveSpeed * Time.deltaTime);

        deltaMove = nextMove - t.position;
        deltaMove += Physics.gravity * Time.deltaTime;
        cc.Move(deltaMove);
    }

    public static bool DetectCharacter(Camera sight, CapsuleCollider cc)
    {
        Plane[] ps = GeometryUtility.CalculateFrustumPlanes(sight);
        return GeometryUtility.TestPlanesAABB(ps, cc.bounds);
    }

    public static float DistanceToCharacter(CharacterController monster, Collider target)
    {
        return Vector3.Distance(monster.transform.position, target.transform.position);
    }

    public static Vector3 DirectionToCharacter(CharacterController monster, Collider target)
    {
        return (target.transform.position - monster.transform.position).normalized;
    }

    public static Vector3 DirectionToCharacter(Collider monster, Collider target)
    {
        return (target.transform.position - monster.transform.position).normalized;
    }

    public static float AnimationLength(Animator anim, string name)
    {
        float time = 0;

        RuntimeAnimatorController ac = anim.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
            if (ac.animationClips[i].name == name)
                time = ac.animationClips[i].length;
        return time;
    }

    public static void DissoveActive(List<Material> mats, bool active)
    {
        float value = active ? 8 : 0;

        for (int i = 0; i < mats.Count; i++)
        {
            mats[i].SetFloat("_DissolveEdgeMultiplier", value);
            mats[i].SetFloat("_DissolveIntensity", 0);
        }
    }

    public static IEnumerator Dissolving(List<Material> mats, float value = 0.45f, float range = 0.2f)
    {
        float time = 0;

        for(int i=0; i<mats.Count; i++)
        {
            time += value * Time.deltaTime;
            mats[i].SetFloat("_DissolveIntensity", time);
            mats[i].SetFloat("_DissolveEdgeRange", range);

            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    public static IEnumerator Blinking(List<Material> mats, Color color, int duration = 6, float timer = 0.15f)
    {
        int i = 0;
        bool blink = false;

        while (i++ < duration)
        {
            float value = blink ? 0.0f : 1.0f;

            for (int j = 0; j < mats.Count; j++)
            {
                mats[j].SetFloat("_Hittrigger", value);                
            }

            blink = !blink;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        for (int j = 0; j < mats.Count; j++)
        {
            mats[j].SetFloat("_Hittrigger", 0);
        }
    }

  


    public static IEnumerator KnockBack(Transform trans, AttackType attackType, Vector3 direction
        )
    {
        for (int time = 0; time < 4; time++)
        {
            //trans.position += direction * (power);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public static int TransformTypeToInt(AttackType type)
    {
        switch (type)
        {
            case AttackType.ATTACK1:
                return 0;

            case AttackType.ATTACK2:
                return 1;

            case AttackType.ATTACK3:
                return 2;

            case AttackType.SKILL1:
                return 3;

            case AttackType.SKILL2:
                return 4;

            case AttackType.SKILL3:
                return 5;

            case AttackType.SKILL4:
                return 6;

            default:
                return -1;
        }
    }


}