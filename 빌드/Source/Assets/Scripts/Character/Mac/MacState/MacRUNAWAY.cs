using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacRUNAWAY : MacFSMState
{
    public bool _SetTarget = false;
    public Transform[] Targets;
    Vector3 TargetPos;

    public override void BeginState()
    {
        base.BeginState();
        TargetPos = Vector3.zero;
        _manager.agent.acceleration = 0.5f;
        SetTarget();
    }

    public override void EndState()
    {
        base.EndState();
        _SetTarget = false;
        _manager.agent.acceleration = 0.0f;
    }

    protected override void Update()
    {
        base.Update();

        if (!_SetTarget) return;

        if (_manager.agent.remainingDistance > 0.5f)
        {
            //transform.position = Vector3.Lerp(this.transform.position, TargetPos, 0.5f * Time.deltaTime);
            _manager.agent.destination = TargetPos;
            _manager.agent.isStopped = false;
        }

        else
        {
            _manager.agent.Stop();
            _manager.agent.destination = this.transform.position;
            _manager.agent.isStopped = true;
            _manager.SetState(MacState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void SetTarget()
    {
        //bool set = false;

        //int loopEscape = 0;

        //while (!set)
        //{
        //    set = Setting();
        //    loopEscape++;

        //    if (loopEscape >= 100)
        //    {
        //        _manager.SetState(MacState.ATTACK);
        //        return;
        //    }
        //}

        //_SetTarget = true;

        List<Vector3> lists = FindObjectOfType<MapGrid>().mapPositions;
        var rand = UnityEngine.Random.Range(0, lists.Count);
        TargetPos = lists[rand];

        _manager.agent.destination = TargetPos;
        _SetTarget = true;
    }

    public bool Setting()
    {
        int random = UnityEngine.Random.Range(0, Targets.Length - 1);

        Ray ray = new Ray();
        ray.origin = Targets[random].position;
        ray.direction = Vector3.down;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.5f, 1 << 17))
        {
            transform.LookAt(Targets[random]);
            TargetPos = hit.point;
            return true;
        }

        return false;
    }

}
