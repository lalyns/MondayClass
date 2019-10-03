using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDEAD : FSMState
{
    bool Dead = false;
    float time = 0;

    public override void BeginState()
    {
        base.BeginState();
        //GetComponent<Collider>().enabled = false;
        

    }

    public override void EndState()
    {
        base.EndState();

    }
    private void Update()
    {
      
    }

    public void DeadSupport()
    {

    }
}
