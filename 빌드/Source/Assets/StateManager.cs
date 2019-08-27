using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateManager : MonoBehaviour
{
    [Header("HP")]
    public float _hp;
    public float _maxHp = 1000f;



    [Header("Init")]
    public GameObject activeModel;


    [Header("Inputs")]
    public float vertical;
    public float horizontal;
    public float moveAmount;
    public Vector3 moveDir;


    [Header("Stats")]
    public float moveSpeed = 2;
    public float runSpeed = 3.5f;
    public float rotateSpeed = 5;
    public float toGround = 0.5f;

    [Header("States")]
    public bool onGround;
    public bool run;


    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rigid;


    [HideInInspector]
    public float delta;
    [HideInInspector]
    public LayerMask ignoreLayers;

    private void FixedUpdate()
    {

    }

    public void Init()
    {
        //초기 체력 100 세팅
        _hp = _maxHp;
        SetupAnimator();
        rigid = GetComponent<Rigidbody>();
        rigid.angularDrag = 999;
        rigid.drag = 4;
        //rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        gameObject.layer = 8;
        ignoreLayers = ~(1 << 9);
    }

    void SetupAnimator()
    {
        if (activeModel == null)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim == null)
            {
                Debug.Log("No model");
            }
            else
            {
                activeModel = anim.gameObject;
            }
        }
        if (anim == null)
            anim = activeModel.GetComponent<Animator>();
    }

    public void FixedTick(float d)
    {
        delta = d;

        rigid.drag = (moveAmount > 0 || onGround == false) ? 0 : 4;


        float targetSpeed = moveSpeed;
        if (run)
            targetSpeed = runSpeed;

        rigid.velocity = moveDir * (targetSpeed * moveAmount);


        //Vector3 targetDir = moveDir;
        //targetDir.y = 0;
        //if (targetDir == Vector3.zero)
        //    targetDir = transform.forward;
        //Quaternion tr = Quaternion.LookRotation(targetDir);
        //Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * moveAmount * rotateSpeed);
        //transform.rotation = targetRotation;

        //  HandleMovementAnimations();
    }

    public void Tick(float d)
    {
        delta = d;
        //  onGround = OnGround();
    }

    void HandleMovementAnimations()
    {
        anim.SetFloat("vertical", moveAmount, 0.4f, delta);
    }

    public bool OnMove()
    {
        return Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Horizontal") <= -0.01f ||
            Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f;
    }
}
