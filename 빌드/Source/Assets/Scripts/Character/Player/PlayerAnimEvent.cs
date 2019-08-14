using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    InputHandler input;
    private void Start()    
    {
        input = InputHandler.instance;        
    }

    void hitCheck()
    {
        if(null!=input)
            input.AttackCheck();
    }
    void hitCancel()
    {
        if (null != input)
            input.AttackCancel();
    }
}
