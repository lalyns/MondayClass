using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survive : Mission
{
    public float _Progress = 0.0f;

    public Transform[] _BeadStartPosition;
    public GameObject[] _BeadPositionEffect;
    public GameObject _Bead;

    public int pos;

    public float _BeadGenerationTime = 10f;
    public float _Time = 0f;

    protected override void Awake()
    {
        base.Awake();

        foreach(GameObject effect in _BeadPositionEffect)
        {
            effect.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_IsMissionStart) return;

        _Time += Time.deltaTime;
        if(_Time >= _BeadGenerationTime)
        {
            pos = UnityEngine.Random.Range(0, _BeadStartPosition.Length - 1);
            _Time = 0.0f;
            Instantiate(_Bead, _BeadStartPosition[pos].position, Quaternion.identity);
            _BeadPositionEffect[pos].SetActive(true);
        }


        if (CheckForClear())
        {
            MissionClear();
        }
    }

    private bool CheckForClear()
    {
        bool isClear = false;

        // 현재 데이터를 가져오는 구조가없기때문에 보류
        //if (_CurrentMissionLevel != 0) return isClear;

        if (_Progress >= 30.0f)
        {
            isClear = true;
        }

        return isClear;
    }

    public override void MissionClear()
    {
        base.MissionClear();

        LinkedList<GameObject> activeMelees = ObjectManager.GetObjectManager()._ObjectPool[0]._ActiveItem;
        LinkedList<GameObject> activeRanges = ObjectManager.GetObjectManager()._ObjectPool[1]._ActiveItem;

        foreach(GameObject melees in activeMelees)
        {
            ObjectManager.ReturnPoolMonster(melees, false);
        }
        foreach(GameObject ranges in activeRanges)
        {
            ObjectManager.ReturnPoolMonster(ranges, true);
        }
    }

    public override void MissionEnd()
    {
        base.MissionEnd();

        _Progress = 0.0f;
    }
}
