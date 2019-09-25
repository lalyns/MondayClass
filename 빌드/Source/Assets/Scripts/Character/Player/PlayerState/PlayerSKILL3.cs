using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSKILL3 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();
        _manager.Skill3_Start.SetActive(true);
        _manager.Skill3_End.SetActive(false);
        isLock = false;
        isMax = false;
        isMin = false;

        _manager.attackType = AttackType.SkILL2;
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
        _manager.isSkill3 = false;
        _manager.Skill3_Start.SetActive(false);
        isAttack = false;
        _manager.isCantMove = false;
        _manager.isAttackOne = false;
        _manager.isAttackTwo = false;
        _manager.isAttackThree = false;
        _manager.vignette.opacity.value = 0;
        _manager.CMvcam2.m_Lens.FieldOfView = 60f;
        //isLock = false;
        _viewTimer = 0f;
        _opacityTimer = 0f;
    }

    bool isAttack;
    bool isLock;

    public float _opacityTimer;
    public float _viewTimer;
    public bool isMax, isMin;
    private void Update()
    {

        //1.7초동안 못움직임.
        _manager.isCantMove = _time <= 4.7f ? true : false;

        _time += Time.deltaTime;



        if (!isMax)
        {
            _opacityTimer += Time.deltaTime;
        }
        if (isMax)
        {
            _opacityTimer -= Time.deltaTime;
        }
        if (!isMin)
        {
            _viewTimer += Time.deltaTime;
            _manager.CMvcam2.m_Lens.FieldOfView = 60 + (_viewTimer * 8.3f);
        }
        if (isMin)
        {
            _viewTimer -= Time.deltaTime;
            _manager.CMvcam2.m_Lens.FieldOfView = 60 + (_viewTimer * 50f);
        }

        
        _manager.vignette.opacity.value = _opacityTimer / 15000f;

        if (_manager.CMvcam2.m_Lens.FieldOfView >= 70f && !isMin)
        {
            _manager.CMvcam2.m_Lens.FieldOfView = 70f;
        }
        if(_manager.CMvcam2.m_Lens.FieldOfView <= 60f && isMin)
        {
            _manager.CMvcam2.m_Lens.FieldOfView = 60f;
        }
        if (_viewTimer >= 4.6f && !isMin)
        {
            _viewTimer = 0.2f;
            isMin = true;
        }


        
                     

        if (_manager.vignette.opacity.value >= 0.00008f)
        {
            isMax = true;
        }
        if (_manager.vignette.opacity.value < 0.00001f)
        {
            isMax = false;
        }








        if (isAttack)
            _manager.Skill3Attack();
        else
            _manager.Skill3Cancel();


        if ((_time >= 1.2f && _time < 1.4f)
            || (_time >= 1.7f && _time < 1.9f)
            || (_time >= 2.2f && _time < 2.4f)
            || (_time >= 2.7f && _time < 2.9f)
            || (_time >= 3.2f && _time < 3.4f)
            || (_time >= 3.7f && _time < 3.9f))
        {
            isAttack = true;
        }

        else
            isAttack = false;

        if (_time >= 1.7f)// && !isLock)
        {
            if (_manager.isFlash)
            {
                _manager.Skill3_End.transform.position = _manager.Skill3_Start.transform.position;
                _manager.Skill3_End.transform.rotation = _manager.Skill3_Start.transform.rotation;
                _manager.Skill3_End.SetActive(true);
            }//isLock = true;
            //if (_manager.OnMove())
            //{
            //    _manager.SetState(PlayerState.RUN);
            //}
        }
        if (_time >= 4.2f && !isLock)
        {
            _manager.Skill3_End.transform.position = _manager.Skill3_Start.transform.position;
            _manager.Skill3_End.transform.rotation = _manager.Skill3_Start.transform.rotation;
            _manager.Skill3_End.SetActive(true);
            isLock = true;
            //if (_manager.OnMove())
            //{
            //    _manager.SetState(PlayerState.RUN);
            //}
        }
        if (_time >= 4.8f)
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }

    }
}
