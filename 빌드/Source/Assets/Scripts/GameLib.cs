using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLib
{
    // 단순하게 적을 찾고, 적에게 피해를 입히는 함수
    public static CharacterStat SimpleDamageProcess(Transform transform, float Range, string targetTag, CharacterStat ownerStat)
    {
        return AttackProcess(
            AttackTargetsByRange(transform, Range),
            targetTag, ownerStat);
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
        CharacterStat lastHit = null;
        foreach (var hitObject in hitObjects)
        {
            if (hitObject.collider.gameObject.tag == targetTag)
            {
                CharacterStat targetStat =
                    hitObject.collider.GetComponent<CharacterStat>();

                CharacterStat.ProcessDamage(ownerStat, targetStat);
                lastHit = targetStat;
            }
        }
        return lastHit;
    }

    // 여러 오브젝트들에 대해 간단한 정보로 피해를 입히는 함수.
    public static CharacterStat AttackProcess(RaycastHit[] hitObjects, string targetTag, CharacterStat ownerStat, int damage)
    {
        CharacterStat lastHit = null;
        foreach (var hitObject in hitObjects)
        {
            if (hitObject.collider.gameObject.tag == targetTag)
            {
                CharacterStat targetStat =
                    hitObject.collider.GetComponent<CharacterStat>();

                CharacterStat.ProcessDamage(ownerStat, targetStat, damage);
                lastHit = targetStat;
            }
        }
        return lastHit;
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
}