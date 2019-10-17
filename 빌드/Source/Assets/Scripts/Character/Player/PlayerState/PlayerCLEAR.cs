using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCLEAR : FSMState
{
    [SerializeField]
    float _time = 0;
    bool isOne = false;

    public GameObject CMSet;
    public override void BeginState()
    {
        base.BeginState();

        if(_manager.CurrentClear == 0)
        {
            _manager.ClearTimeLine.SetActive(true);
        }
        else
        {
            _manager.ClearTimeLine2.SetActive(true);
        }
       
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
        isOne = false;
        _manager.ClearTimeLine.SetActive(false);
        _manager.ClearTimeLine2.SetActive(false);
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_manager.CurrentClear == 1)
        {
            if(_time >= 3.3f && !isOne)
            {
                _manager.CurrentClear = 2;
                _manager.Anim.SetFloat("CurrentClear", _manager.CurrentClear);
                isOne = true;
            }
        }      

        if (GameStatus.currentGameState == CurrentGameState.Wait)
        {
            return;
        }
    }
}
