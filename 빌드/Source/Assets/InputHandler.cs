using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//크리티컬이면
//(기본공격 + 추가공격 - 방어력) * 크리티컬추가
//일반공격이면
//(기본공격 + 추가공격 - 방어력)
public class InputHandler : MonoBehaviour
{
    float vertical;
    float horizontal;
    StateManager states;
    CameraManager camManager;
    public Camera maincamera;
    float delta;
    public Animator anim1, anim2;
    float r_x = 0.00f;
    public bool isAlt;

    //[SerializeField]
    public bool isAttackOne, isAttackTwo, isAttackThree;
    [SerializeField]
    float Timer1;
    float Timer2;
    float Timer3;
    float Timer4;

    public bool isCantMove;


    public float FeverGauge = 0;
    bool isCanFever;
    bool isFever;

    bool isCamInit;

    Shake shake;

    public bool isAttackTwoReady;

    CapsuleCollider Attack_Capsule;
    private void Start()
    {
        states = GetComponent<StateManager>();
        states.Init();

        camManager = CameraManager.singleton;
        camManager.Init(this.transform);
        //camManager.Init(anim1.transform);
        //camManager.gameObject.SetActive(false);
        //anim1 = GetComponentInChildren<anim1ator>();

        anim1 = GameObject.Find("PC_Rig").GetComponentInChildren<Animator>();
        anim2 = GameObject.Find("Luda").GetComponentInChildren<Animator>();

        anim1.gameObject.SetActive(true);
        anim2.gameObject.SetActive(false);

        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
        maincamera = GameObject.Find("mainCam").GetComponent<Camera>();

        isAttackOne = false;
        isAttackTwo = false;
        isCantMove = false;
        isFever = false;

        Attack_Capsule = GameObject.FindGameObjectWithTag("Weapon").GetComponent<CapsuleCollider>();

        Attack_Capsule.enabled = false;
    }
    public void AttackCheck()
    {
        Attack_Capsule.enabled = true;
    }
    private void FixedUpdate()
    {

        r_x = Input.GetAxis("Mouse X");
        if (Input.GetKey(KeyCode.Q))
        {


            camManager.Tick(delta);

            camManager.gameObject.SetActive(true);
            maincamera.gameObject.SetActive(false);

        }
        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    camManager.camInit(this.transform);
        //}
        else
        {
            camManager.camInit(anim1.transform);
            //camManager.gameObject.SetActive(false);
            maincamera.gameObject.SetActive(true);
            //camManager.cams.targetDisplay = 1;
            //Camera.main.targetDisplay = 0;
            //camManager.Tick(delta);

            anim1.transform.Rotate(Vector3.up * 80 * Time.deltaTime * r_x);

            isCamInit = false;
        }

        delta = Time.fixedDeltaTime;

        //UpdateStates();
        states.FixedTick(delta);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("마우스누름");
        }
        

        GetInput();


        delta = Time.deltaTime;
        states.Tick(delta);
        if (isAttackOne || isAttackTwo || isAttackThree)
        {
            isCantMove = true;
        }
        else
        {
            isCantMove = false;
        }
        if (!isFever)
        {
            if (states.OnMove())
            {
                anim1.SetBool("isRun", true);
            }
            else
                anim1.SetBool("isRun", false);
        }
        if (isFever)
        {
            if (states.OnMove())
            {
                anim2.SetBool("isRun", true);
            }
            else
                anim2.SetBool("isRun", false);
        }

        //if (Input.GetKeyDown(KeyCode.Space) && !isAttackOne)
        if (Input.GetMouseButtonDown(0) && !isAttackOne)
        {
            if (!isFever)
                anim1.SetInteger("CurrentAttack", 1);
            if (isFever)
                anim2.SetInteger("CurrentAttack", 1);

            isAttackOne = true;

            StartCoroutine(shake.ShakeCamera());

        }


        if (isAttackOne)
        {
            Timer1 += Time.deltaTime;
            // 애니는 0.733초지만 미리 눌러 둘 수 있게 세팅함.
            if (Timer1 >= 0.4f)
            {
                //스페이스바를 누르면
                //if (Input.GetKeyDown(KeyCode.Space))
                if (Input.GetMouseButtonDown(0))
                {
                    isAttackTwoReady = true;                   
                }
                if (isAttackTwoReady)
                {
                    if (Timer1 >= 0.733f)
                    {
                        if (!isFever)
                            anim1.SetInteger("CurrentAttack", 2);
                        if (isFever)
                            anim2.SetInteger("CurrentAttack", 2);

                        isAttackOne = false;
                        isAttackTwo = true;
                        //시간 초기화
                        Timer1 = 0;
                        StartCoroutine(shake.ShakeCamera());
                        isAttackTwoReady = false;
                        return;
                    }
                }
            }
            if (Timer1 >= 0.85f)
            {
                //IDLE 상태로 돌려줌
                if (!isFever)
                {
                    anim1.SetInteger("CurrentAttack", 4);
                    Attack_Capsule.enabled = false;
                    if (Timer1 >= 1.333f)
                    {
                        anim1.SetInteger("CurrentAttack", 0);
                        Timer1 = 0;
                        isAttackOne = false;
                        return;
                    }
                }
                if (isFever)
                {
                    anim2.SetInteger("CurrentAttack", 0);
                    Timer1 = 0;
                    isAttackOne = false;
                }
            }
        }
        if (isAttackTwo)
        {
            Timer2 += Time.deltaTime;
            //1초가 넘어야 3타를 할 수 있음.
            if (Timer2 >= 0.733f)
            {
                anim1.SetInteger("CurrentAttack", 0);
                isAttackTwo = false;
                Timer2 = 0;
                Attack_Capsule.enabled = false;
                return;
              /*
                //스페이스바를 누르면
                if (Input.GetMouseButtonDown(0))
                {
                    //3타 애니메이션 실행.
                    if (!isFever)
                        //anim1.SetInteger("CurrentAttack", 3);
                    if (isFever)
                        anim2.SetInteger("CurrentAttack", 3);

                    //2타 완료.
                    isAttackTwo = false;
                    isAttackThree = true;
                    //시간 초기화
                    Timer2 = 0;

                    StartCoroutine(shake.ShakeCamera(0.3f, 0.2f, 0.5f));

                    return;
                }

                if (Timer2 >= 2f)
                {
                    //IDLE 상태로 돌려줌
                    if (!isFever)
                        anim1.SetInteger("CurrentAttack", 0);
                    if (isFever)
                        anim2.SetInteger("CurrentAttack", 0);
                    isAttackTwo = false;

                    //시간 초기화
                    Timer2 = 0;
                    return;
                }
                */
            }
        }
        if (isAttackThree)
        {
            Timer3 += Time.deltaTime;



            if (Timer3 > 1.3f)
            {
                if (!isFever)
                    anim1.SetInteger("CurrentAttack", 0);
                if (isFever)
                    anim2.SetInteger("CurrentAttack", 0);
                isAttackThree = false;
                Timer3 = 0;
                return;
            }
        }
        //if(Input.GetMouseButtonDown(0) && isAttackOne && !isAttackTwo)
        //{
        //    anim1.SetInteger("CurrentAttack", 2);
        //    isAttackTwo = true;
        //}
        //if (Input.GetMouseButtonDown(0) && isAttackTwo)
        //{
        //    anim1.SetInteger("CurrentAttack", 3);
        //    isAttackOne = false;
        //}


        if (FeverGauge == 100)
        {
            isCanFever = true;

        }
        else
        {
            isCanFever = false;
        }

        if (isCanFever)
        {
            isFever = true;
            anim1.gameObject.SetActive(false);
            anim2.gameObject.SetActive(true);
        }
        if (!isCanFever)
        {
            isFever = false;
            anim1.gameObject.SetActive(true);
            anim2.gameObject.SetActive(false);
        }

        if (isGauge)
        {
            Timer4 += Time.deltaTime;
            if (Timer4 > 10f)
            {
                FeverGauge = 0;
                Timer4 = 0;
                isGauge = false;
                return;
            }
        }
    }

    void GetInput()
    {
        if (!isCantMove)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }
        else if (isCantMove)
        {
            vertical = 0;
            horizontal = 0;
        }

        if (vertical >= 0.1f)
        {
            //전진애니메이션
            anim1.SetFloat("Direction_Y", vertical);
            anim1.SetFloat("Direction_X", horizontal);
        }
        else if (vertical <= -0.1f)
        {
            anim1.SetFloat("Direction_Y", vertical);
            anim1.SetFloat("Direction_X", horizontal);
        }
        else if (horizontal >= 0.1f && vertical == 0)
        {
            //오른쪽
            anim1.SetFloat("Direction_Y", 0);
            anim1.SetFloat("Direction_X", horizontal);
        }
        else if (horizontal <= -0.1f && vertical == 0)
        {
            //왼쪽
            anim1.SetFloat("Direction_Y", 0);
            anim1.SetFloat("Direction_X", horizontal);
        }

    }

    //Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    void UpdateStates()
    {
        //moveDirection = transform.TransformDirection(moveDirection);
        //moveDirection *= Time.fixedDeltaTime;
        //moveDirection *= 5f;

        //moveDirection += transform.position;
        //transform.position = moveDirection;

        states.horizontal = horizontal;
        states.vertical = vertical;

        Vector3 v = states.vertical * states.transform.forward;
        Vector3 h = states.horizontal * states.transform.right;
        states.moveDir = (v + h).normalized;
        float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        states.moveAmount = Mathf.Clamp01(m);

    }
    bool isGauge;
    public void FeverButton()
    {
        FeverGauge = 100;
        isGauge = true;
    }



    public static InputHandler singleton;
    void Awake()
    {
        singleton = GetComponent<InputHandler>();
    }
    public static InputHandler FindInputHandler()
    {
        return GameObject.FindWithTag("Player").GetComponent<InputHandler>();
    }
}
