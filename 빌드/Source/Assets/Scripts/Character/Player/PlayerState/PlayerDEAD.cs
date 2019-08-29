using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDEAD : FSMState
{
    public override void BeginState()
    {
        base.BeginState();
        GetComponent<Collider>().enabled = false;
    }

    public override void EndState()
    {
        base.EndState();
    }
}
