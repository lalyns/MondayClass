    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour {

    [SerializeField]
    UIHpManager _uiManager;
    PlayerFSMManager _manager;
    float _currentHp, _currentEnemyHp;
    CharacterStat _currentTarget;
	// Use this for initialization
	void Start () {
        _manager = GetComponent<PlayerFSMManager>();
        _currentHp = _manager.Stat.Hp;
        _currentEnemyHp = 0f;
        _currentTarget = _manager._lastAttack;
        _uiManager.ShowEnemyHpBar(false);
        StartCoroutine(MyHpNotifyer());
        StartCoroutine(TargetNotifyer());
    }

    // 내 캐릭터의 체력 변경을 추적
    IEnumerator MyHpNotifyer()
    {
        while (true)
        {
            yield return new WaitUntil(() => _currentHp != _manager.Stat.Hp);
            _currentHp = _manager.Stat.Hp;
            _uiManager.SetMyHpRate(_currentHp / _manager.Stat.MaxHp);
        }
    }

    // 적 캐릭터의 체력 변경을 추적
    IEnumerator EnemyHpNotifyer()
    {
        while (true)
        {
            yield return new WaitUntil(() => _currentTarget == null || _currentEnemyHp != _currentTarget.Hp);
            if (_currentTarget == null) break;
            _currentEnemyHp = _currentTarget.Hp;
            _uiManager.SetEnemyRate(_currentEnemyHp / _currentTarget.MaxHp);
        }
    }

    // 공격 타겟 변경을 추적
    IEnumerator TargetNotifyer()
    {
        while (true)
        {
            yield return new WaitUntil(() => _currentTarget != _manager._lastAttack);
            _currentTarget = _manager._lastAttack;
            if (_currentTarget != null)
            {
                _uiManager.SetTargetName(_currentTarget.name);
                _currentEnemyHp = -1f;
                StartCoroutine(EnemyHpNotifyer());
                _uiManager.ShowEnemyHpBar(true);
            }
            else _uiManager.ShowEnemyHpBar(false);
        }
    }
}
