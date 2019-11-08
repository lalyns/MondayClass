using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacRUNAWAY : MacFSMState
{
    public bool setTarget = false;
    public Transform[] targets;
    Vector3 targetPos;

    public override void BeginState()
    {
        base.BeginState();
        targetPos = Vector3.zero;
        _manager.agent.acceleration = 0.5f;
        SetTarget();
    }

    public override void EndState()
    {
        base.EndState();
        setTarget = false;
        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.isStopped = true;
        _manager.agent.acceleration = 0.0f;
    }

    protected override void Update()
    {
        base.Update();

        if (!setTarget) return;

        if (_manager.agent.remainingDistance > 0.5f)
        {
            //transform.position = Vector3.Lerp(this.transform.position, TargetPos, 0.5f * Time.deltaTime);
            _manager.agent.destination = targetPos;
            _manager.agent.isStopped = false;
        }

        else
        {
            _manager.SetState(MacState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void SetTarget()
    {
        //List<Vector3> lists = FindObjectOfType<MapGrid>().mapPositions;
        //var rand = UnityEngine.Random.Range(0, lists.Count);
        //targetPos = lists[rand];

        while (!SettingRunAway())
        {

        }

        _manager.agent.destination = targetPos;
        setTarget = true;
    }

    public bool SettingRunAway()
    {
        int random = UnityEngine.Random.Range(0, targets.Length - 1);

        Ray ray = new Ray();
        ray.origin = targets[random].position;
        ray.direction = Vector3.down;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.5f, 1 << 17))
        {
            transform.LookAt(targets[random]);
            targetPos = hit.point;
            return true;
        }

        return false;
    }

}
