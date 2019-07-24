using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionScript : MonoBehaviour
{
    public float JumpForce = 10f;
    Rigidbody rbody;
    Animator mator;
    float horizontal; //좌우 방향키
    float vertical; //상하 방향키
    bool Accel; //가속여부
    bool isBottom; //현재 땅에 있는지 여부
    bool Jumping; //점프했는지 여부 (정확히는 점프를 위해 addforce를 했는지 여부)
    int AccSpeed = 0; //애니메이션에 따른 이동여부
    string ver = "Vertical";
    string motion = ""; //현재 재생중인 이름을 받을 문자열
    bool Jump;
    void Awake()//객체가 준비된 순간 실행, Start보다 빠르다, 리지드바디와 애니메이터를 가져온다.
    {
        //rbody = GetComponent<Rigidbody>();
        mator = GetComponent<Animator>();
    }
    //void OnCollisionStay(Collision collision) //여기서는 캐릭터가 땅과 접촉중인지를 확인
    //{
    //    if (collision.gameObject.tag == "Bottom") isBottom = true;
    //}
    //void OnCollisionExit(Collision collision) //여기서는 캐릭터가 땅과 접촉이 끊겼는지 확인
    //{
    //    if (collision.gameObject.tag == "Bottom") isBottom = false;
    //}
    void FixedUpdate() // 키보드쪽을 입력받고, 캐릭터를 회전시킨다 (이동은 Onanimatormove에서)
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

       // Accel = Input.GetKey(KeyCode.X) && Vertical == 1;
       // Jump = JumpCondition();
       // AnimeParameterSet();
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, this.transform.eulerAngles.y + Horizontal, 0), 1f);
    }
    void OnAnimatorMove()//애니메이션 동작을 처리할때마다 실행
    {
        //motion = CurrentAni(); //현재 모션을 받는다
        // AnimeSwitching(); //모션에 따른 스크립트 실행
        Vector3 v = transform.forward * vertical; //* 10f * Time.fixedDeltaTime;
        Vector3 h = transform.right * horizontal;// * 5f * Time.fixedDeltaTime;
        Vector3 moveDir = (v + h).normalized;

        float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        float moveAmount = Mathf.Clamp01(m);
        transform.position = transform.position + (moveDir * moveAmount * 0.1f);
        //if (Accel) pos *= 3f; //가속중인경우 3배 더 빠르게
        //rbody.MovePosition(transform.position + pos);
    }
    //string CurrentAni()//현재 애니메이션 이름을 받아온다
    //{
    //    foreach (AnimationClip clip in mator.runtimeAnimatorController.animationClips) //애니메이터에 존재하는 모든 애니메이션 클립을 전체배열로 foreach문 실행
    //        if (mator.GetCurrentAnimatorStateInfo(0).IsName(clip.name)) return clip.name.ToString();
    //    return null; //찾지못한경우 null반환
    //}
    //string NextAni()//다음 예정 애니메이션 이름을 받아온다
    //{
    //    foreach (AnimationClip clip in mator.runtimeAnimatorController.animationClips)
    //        if (mator.GetNextAnimatorStateInfo(0).IsName(clip.name)) return clip.name.ToString();
    //    return "";
    //}
    //void AnimeSwitching()//모션 상태에 따른 제어 스크립트
    //{
    //    if (motion != "JUMP00") Jumping = false; //처음 점프모션이 아닌경우 Addfoce부여를 가능하게 한다.
    //    switch (motion)
    //    {
    //        case "WALK00_F"://걷기와 뛰기모두 이동을 가능케한다
    //        case "RUN00_F":
    //            AccSpeed = 1;
    //            break;
    //        case "WAIT00"://대기 상태, Vertical에 상관없이 이동불가
    //            AccSpeed = 0;
    //            break;
    //        case "JUMP00":
    //            if (isBottom && !Jumping) //바닥에 있고, Addforce부여가 가능한경우(두 조건을 모두 체크하지않을경우, 점프모션이 끝나기전에 바닥에 착지할경우 다시 addforce를 할수있다)
    //            {
    //                rbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    //                Jumping = true; //Addforce를 중복되지 않게 한다.
    //            }
    //            if (Vertical == 1) AccSpeed = 1; //앞으로 가는경우에만 이동을 가능하게 한다.
    //            else AccSpeed = 0;
    //            break;
    //        case "JUMP01":
    //            if (Vertical == 1) AccSpeed = 1; //앞으로 가는경우에만 이동을 가능하게한다.
    //            else AccSpeed = 0;
    //            break;
    //        case "JUMP02":
    //            AccSpeed = 0;//착지모션중에는 이동이 불가능하게 한다.
    //            break;
    //    }
    //}
    //void AnimeParameterSet()//Animator에 변수 전달
    //{
    //    mator.SetBool("isBottom", isBottom);
    //    mator.SetBool("Jump", Jump);
    //    mator.SetInteger(ver, Vertical);
    //    mator.SetBool("Accel", Accel);
    //}
    //bool JumpCondition() //점프 조건검사
    //{
    //    bool a1 = Input.GetKeyDown(KeyCode.Z); //Z키를 누르고
    //    bool a2 = !motion.Contains("JUMP"); //현재 점프와 관련된 모션이 아니며
    //    bool a3 = isBottom; //바닥인경우
    //    if (a1 && a2 && a3) return true; //세조건이 모두 허용될때만 true반환
    //    else return false; //세조건중 하나라도 아닌경우 false 반환
    //}
}