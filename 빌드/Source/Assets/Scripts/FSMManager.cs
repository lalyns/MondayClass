using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FSMManager 전역에 대한 내용을 처리하는 클래스
// 기즈모에 대한 처리가 있음.
public class FSMManager : MonoBehaviour
{
    private bool _bOnSight = true;
    public bool _Player = true;
    private Camera _sight;
    public Camera Sight { get { return _sight; } }
    public int sightAspect = 3;
    private Color _gizmoColor;
    protected void SetGizmoColor(Color color) { _gizmoColor = color; }
    protected void ShowSight(bool isOn) { _bOnSight = isOn; }
    [SerializeField]
    protected StatData _statData;
    public CharacterStat stats;
    public StatData MyStatData { get { return _statData; } }
    //[HideInInspector]
    //public CharacterStat _lastAttack;

    [HideInInspector] public MonsterType monsterType;

    protected virtual void Awake()
    {
    }

    private void OnDrawGizmos()
    {
        if (_Player)
        {
            if (!_bOnSight) return;
            if (_sight != null)
            {
                Gizmos.color = _gizmoColor;
                Matrix4x4 temp = Matrix4x4.identity;

                Gizmos.matrix = Matrix4x4.TRS(
                    _sight.transform.position,
                    _sight.transform.rotation,
                    Vector3.one
                    );

                Gizmos.DrawFrustum(
                    Vector3.zero,
                    _sight.fieldOfView,
                    _sight.farClipPlane,
                    _sight.nearClipPlane,
                    _sight.aspect
                    );

                Gizmos.matrix = temp;
            }
        }
    }

    public virtual void LastHitBy()
    {

    }

    public virtual void NotifyTargetKilled() { }

    public virtual void SetDeadState() {

        if (!_Player)
        {
        }
    }

    public virtual bool IsDie() { return false; }

    public virtual void OnHitForMonster(AttackType attackType)
    {

    }

    public virtual void OnHitForPlayer()
    {

    }
    PlayerStat stat;

    public virtual IEnumerator Skill2Timer()
    {
        stat = PlayerFSMManager.Instance.stat;
        float attackTime = 0.0f;
        while (attackTime<0.3f) {
            
            //stats.TakeDamage(PlayerFSMManager.Instance.stats, 30);
            CharacterStat.ProcessDamage(stat, stats, 200);
            attackTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public virtual IEnumerator Skill3Timer()
    {
        while (PlayerFSMManager.Instance.isSkill3)
        {
            OnHitForMonster(AttackType.SKILL3);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public virtual void ForHit()
    {

    }

}
