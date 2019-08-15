using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    FollowCam followCam;
    Camera maincamera;
    float delta;
    public Animator anim1, anim2;
    float r_x = 0.00f;
    float r_y = 0.00f;
    public bool isAlt;

    //[SerializeField]
    public bool isAttackOne, isAttackTwo, isAttackThree;
    [SerializeField]
    float Timer1, Timer2, Timer3;
    float Timer4;

    public bool isCantMove;
    public bool isInputLock;

    public float FeverGauge = 0;
    bool isCanFever;
    bool isFever;

    bool isCamInit;

    Shake shake;

    public bool isAttackTwoReady, isAttackThreeReady;

    //충돌처리 콜라이더 및 공격 검귀 이펙트?
    CapsuleCollider Attack_Capsule;
    Transform SwingEffect;
    Transform ballStartPos;

    [Header("X축 마우스 감도")]
    public float mouseSpeed = 80f;

    int attackCount;
    [SerializeField]
    float attackOne, attackTwo, attackThree, backOne, backTwo;
    float animWaitTime = 0.1f;

    float AnimationLength(string name)
    {
        float time = 0;

        RuntimeAnimatorController ac = anim1.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)  
            if (ac.animationClips[i].name == name)
                time = ac.animationClips[i].length;
        return time;
    }

    public List<GameObject> _monster = new List<GameObject>();
    public List<GameObject> Monster { get { return _monster; } }

    public float playerHP;
    public float playerMaxHP;
    private void Start()
    {
        _monster.Clear();
       

        states = GetComponent<StateManager>();
        states.Init();        
        playerHP = states._hp;
        playerMaxHP = states._maxHp;

        playerHP--;
        Debug.Log(states._hp+ "," + playerHP);
        
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
        //maincamera = GameObject.Find("mainCam").GetComponent<Camera>();
        followCam = shake.GetComponent<FollowCam>();
        isAttackOne = false;
        isAttackTwo = false;
        isCantMove = false;
        isFever = false;

        Attack_Capsule = GameObject.FindGameObjectWithTag("Weapon").GetComponent<CapsuleCollider>();
        try
        {
            SwingEffect = GameObject.Find("SwingEffect").GetComponent<Transform>();
            SwingEffect.gameObject.SetActive(false);
        }
        catch
        {

        }
        Attack_Capsule.enabled = false;

        ball1 = anim1.GetComponentInChildren<SphereCollider>();

        //ball1.gameObject.SetActive(false);
        ballStartPos = GameObject.Find("BallStartPos").GetComponent<Transform>();

        Skill1_CoolTime = GameObject.Find("Skill1_CoolTime").GetComponent<Image>();
        Skill1_CoolTime.fillAmount = 1f;
        Skill1_CoolTime.gameObject.SetActive(false);

        isInputLock = false;

        //1~3타 애니의 재생 길이
        attackOne = AnimationLength("PC_Attack_001");
        attackTwo = AnimationLength("PC_Attack_002");
        attackThree = AnimationLength("PC_Attack_003");
        backOne = AnimationLength("PC_Attack_Back_001");
        //backTwo = AnimationLength("PC_Attack_Back_002");
    }
    Image Skill1_CoolTime;
    public void AttackCheck()
    {
        Attack_Capsule.enabled = true;
    }
    public void AttackCancel()
    {
        Attack_Capsule.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isInputLock)
            return;
        
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
            //maincamera.gameObject.SetActive(true);
            //camManager.cams.targetDisplay = 1;
            //Camera.main.targetDisplay = 0;
            //camManager.Tick(delta);

            anim1.transform.Rotate(Vector3.up * mouseSpeed * Time.deltaTime * r_x);
            //followCam.targetOffset = 1 + (1 * r_y);
            //followCam.transform.LookAt(transform.position + (transform.up * ( 1 *r_y)));
            isCamInit = false;
        }

        delta = Time.fixedDeltaTime;

        //UpdateStates();
        states.FixedTick(delta);
        //앞
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            anim1.transform.position = anim1.transform.position + (anim1.transform.forward * 10f);
        }
        //왼
        if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            anim1.transform.position = anim1.transform.position + (anim1.transform.right * -10f);
        }
        //뒤
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim1.transform.position = anim1.transform.position + (anim1.transform.forward * -10f);
        }
        //오
        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
        {
            anim1.transform.position = anim1.transform.position + (anim1.transform.right * 10f);
        }
        //왼앞
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            anim1.transform.position = anim1.transform.position + (anim1.transform.forward * 5f) + (anim1.transform.right * -5f);
        }
        //오앞
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            anim1.transform.position = anim1.transform.position + (anim1.transform.forward * 5f) + (anim1.transform.right * 5f);
        }
        //왼뒤
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim1.transform.position = anim1.transform.position + (anim1.transform.forward * -5f) + (anim1.transform.right * -5f);
        }
        //오뒤
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            anim1.transform.position = anim1.transform.position + (anim1.transform.forward * -5f) + (anim1.transform.right * 5f);
        }

    }
    public SphereCollider ball1;
    bool isBall, isShoot;
    public Vector3 target;
    float shootTimer;
    public float skill1CoolTimer = 10f;
    bool isSkill1CoolTime;

    public int randomShoot;

    public GameObject[] Skill1Effects;
    private void Update()
    {

        if (isInputLock)
            return;

        
        //target = new Vector3(anim1.transform.position.x + 10f, anim1.transform.position.y, anim1.transform.position.z + 5f);


        if (attackCount >= 10)
        {
            attackCount = 0;
            Skill1Effects[0].gameObject.SetActive(true);
            //ball1.gameObject.SetActive(true);
            isBall = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("마우스누름");
            SwingEffect.gameObject.SetActive(true);

            attackCount++;
        }
        
        if (isBall)
        {
            if (Input.GetKey(KeyCode.F))
            {
                _monster.AddRange(GameObject.FindGameObjectsWithTag("Monster"));
                randomShoot = Random.Range((int)0, (int)_monster.Count +1);
                Skill1Effects[0].gameObject.SetActive(false);
                Skill1Effects[1].gameObject.SetActive(true);
                isShoot = true;
                isBall = false;
                isSkill1CoolTime = true;
                Skill1_CoolTime.gameObject.SetActive(true);
            }
        }
        if (isShoot)
        {
            target = _monster[randomShoot].transform.position;
            //Vector3 dis = target - ball1.transform.position;
            //dis.Normalize();
            //Quaternion.LookRotation(dis);
            //ball1.transform.Translate(Vector3.forward * 20f * Time.deltaTime);
            //ball1.transform.position = Vector3.MoveTowards(ball1.transform.position, target, 20f * Time.deltaTime);
            Skill1Effects[1].transform.position = Vector3.MoveTowards(Skill1Effects[1].transform.position, target, 20f * Time.deltaTime);
            shootTimer += Time.deltaTime;

            if (shootTimer > 2f)
            {
                //ball1.transform.position = ballStartPos.position;
                Skill1Effects[1].transform.position = ballStartPos.position;
                Skill1Effects[1].SetActive(false);
                //ball1.gameObject.SetActive(false);
                shootTimer = 0;
                isShoot = false;
                _monster.Clear();
            }
        }
        if (isSkill1CoolTime)
        {
            skill1CoolTimer -= Time.deltaTime;
            Skill1_CoolTime.fillAmount = skill1CoolTimer / 10f;
            if (skill1CoolTimer <= 0)
            {
                skill1CoolTimer = 10f;
                Skill1_CoolTime.fillAmount = 1f;
                Skill1_CoolTime.gameObject.SetActive(false);

                isSkill1CoolTime = false;
            }
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

            if (!isAttackTwo && !isAttackThree)
                isAttackOne = true;

            //StartCoroutine(shake.ShakeCamera());

        }


        if (isAttackOne)
        {
            Timer1 += Time.deltaTime;
            // 애니는 0.666초지만 미리 눌러 둘 수 있게 세팅함.
            if (Timer1 >= attackOne - 0.3f)
            {
                //스페이스바를 누르면
                //if (Input.GetKeyDown(KeyCode.Space))
                if (Input.GetMouseButtonDown(0))
                {
                    isAttackTwoReady = true;
                }
                if (isAttackTwoReady)
                {
                    if (Timer1 >= attackOne)
                    {
                        if (!isFever)
                            anim1.SetInteger("CurrentAttack", 2);
                        if (isFever)
                            anim2.SetInteger("CurrentAttack", 2);

                        isAttackOne = false;
                        isAttackTwo = true;
                        //시간 초기화
                        Timer1 = 0;
                        // StartCoroutine(shake.ShakeCamera());
                        isAttackTwoReady = false;
                        return;
                    }
                }
            }
            if (Timer1 >= attackOne + animWaitTime)
            {
                //IDLE 상태로 돌려줌
                if (!isFever)
                {
                    anim1.SetInteger("CurrentAttack", 4);
                    Attack_Capsule.enabled = false;
                    //SwingEffect.gameObject.SetActive(false);
                    if (Timer1 >= attackOne+animWaitTime+backOne)
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

            if (Input.GetMouseButtonDown(0))
            {
                isAttackThreeReady = true;
            }
            if (isAttackThreeReady)
            {
                //0.333f 안에 마우스 눌렀으면 0.333f초 후 3타 시작.
                if (Timer2 >= attackTwo)
                {
                    if (!isFever)
                        anim1.SetInteger("CurrentAttack", 3);
                    if (isFever)
                        anim2.SetInteger("CurrentAttack", 3);

                    isAttackTwo = false;
                    isAttackThree = true;
                    //시간 초기화
                    Timer2 = 0;
                    // StartCoroutine(shake.ShakeCamera());
                    isAttackThreeReady = false;
                    return;
                }
            }

            //0.4초가 넘어가면 IDLE로 돌아옴.
            if (Timer2 >= attackTwo+animWaitTime)
            {

                anim1.SetInteger("CurrentAttack", 0);
                isAttackTwo = false;
                Timer2 = 0;
                Attack_Capsule.enabled = false;
                SwingEffect.gameObject.SetActive(false);
                return;
            }


        }
        if (isAttackThree)
        {
            Timer3 += Time.deltaTime;

            if (Timer3 > attackThree)
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

        if (vertical >= 0.1f && horizontal == 0)
        {
            //전진애니메이션
            anim1.SetFloat("Direction_Y", vertical);
            anim1.SetFloat("Direction_X", 0);
        }
        else if (vertical <= -0.1f && horizontal == 0)
        {
            anim1.SetFloat("Direction_Y", vertical);
            anim1.SetFloat("Direction_X", 0);
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
        else if (!(horizontal == 0f && vertical == 0f))
        {
            anim1.SetFloat("Direction_Y", vertical);
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


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "MonsterWeapon")
        {
            Debug.Log("맞았다.");
            playerHP--;
        }
    }

    public void OnHit()
    {

    }


    public static InputHandler instance;
    void Awake()
    {
        instance = GetComponent<InputHandler>();
    }
    public static InputHandler FindInputHandler()
    {
        return GameObject.FindWithTag("Player").GetComponent<InputHandler>();
    }
}
