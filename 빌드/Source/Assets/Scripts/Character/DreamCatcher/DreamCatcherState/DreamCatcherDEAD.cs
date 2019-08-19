using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcherDEAD : DreamCatcherFSMState
{
    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();

        ObjectManager.ReturnPoolMonster(this.gameObject, ObjectManager.MonsterType.DreamCatcher);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
