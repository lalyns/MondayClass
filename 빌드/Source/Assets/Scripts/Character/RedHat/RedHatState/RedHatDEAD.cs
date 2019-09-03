using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatDEAD : RedHatFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        if (!GameManager._Instance._IsDummyScene)
        {
            ObjectManager.ReturnPoolMonster(this.gameObject, ObjectManager.MonsterType.RedHat);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public override void EndState()
    {
        base.EndState();
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
