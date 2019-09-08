using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacDEAD : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();


        if (!GameManager._Instance._IsDummyScene)
        {
            ObjectManager.ReturnPoolMonster(this.gameObject, ObjectManager.MonsterType.Mac);
        }
        else
        {
            MonsterPoolManager._Instance._Mac.ItemReturnPool(gameObject, "monster");
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
    void Update()
    {
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
