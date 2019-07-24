using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public Transform target;
    public float moveDamping = 99999f;
    public float rotateDamping = 10.0f;
    public float distance = 5.0f;
    public float height = 3.0f;    
    public float targetOffset = 1.0f;

    [Header("Wall Obstacle Setting")]
    public float heightAboveWall = 7.0f; // 카메라가 올라갈 높이
    public float colliderRadius = 1.8f; // 충돌체의 반지름
    public float overDamping = 5.0f; // 이동속도 계수
    public float originHeight; // 최소 높이를 보관할 변수

    [Header("Etc Obstacle Setting")]
    //카메라가 올라갈 높이
    public float heightAboveObstacle = 12.0f;
    //플레이어 투사할 레이캐스트의 높이 옵셋
    public float castOffset = 1.0f;
    
    void Start()
    {
        originHeight = height;
        target = GameObject.Find("PC_Rig").GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //구체 형태의 충돌체로 충돌 여부를 검사

        if (Physics.CheckSphere(transform.position, colliderRadius))
        {
            //보간함수를 사용하여 카메라의 높이를 부드럽게 상승시킴.
            height = Mathf.Lerp(height, heightAboveWall, Time.deltaTime * overDamping);
        }
        else
        {
            //보간함수를 이용하여 카메라의 높이를 부드럽게 하강시킨다.
            height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
        }
        //플레이어가 장애물에 가려졌는지를 판단할 레이캐스트의 높낮이를 설정
        Vector3 castTarget = target.position + (target.up * castOffset);
        //castTarget 좌표로의 방향벡터 계산
        Vector3 castDir = (castTarget - transform.position).normalized;
        //충돌 정보를 반환받을 변수
        RaycastHit hit;

        //레이캐스트를 투사해 장애물 여부 판단
        if(Physics.Raycast(transform.position, castDir, out hit, Mathf.Infinity))
        {
            //플레이어가 레이캐스트에 맞지 않았을 경우
            if (!hit.collider.CompareTag("Player"))
            {
                //보간함수 사용 카메라 상승
                height = Mathf.Lerp(height, heightAboveObstacle, Time.deltaTime * overDamping);
            }
            else
            {
                height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
            }
        }
        
    }
    public float r_y = 0.00f;
    [Header("Y축 마우스감도, 최소각, 최대각")]
    public float mouseSpeedY = 5f;
    public float minAngle = 0.7f;
    public float maxAngle = 3f;
    private void FixedUpdate()
    {
        r_y = Input.GetAxis("Mouse Y");
        targetOffset += r_y * Time.fixedDeltaTime * mouseSpeedY;
        if (targetOffset < minAngle)
            targetOffset = minAngle;
        if (targetOffset >= maxAngle)
            targetOffset = maxAngle;
    }
    
    private void LateUpdate()
    {
        
        var camPos = target.position - (target.forward * distance) + (target.up * height);
        transform.position = camPos;
        //transform.position = Vector3.Slerp(transform.position, camPos, Time.deltaTime * moveDamping);
        //transform.rotation = target.rotation;//Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * rotateDamping);
        transform.LookAt(target.position + (target.up * targetOffset));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);

        //카메라의 충돌체를 표현하기 위한 구체를 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, colliderRadius);

        //플레이어가 장애물에 가려졌는지 판단할 레이를 표사ㅣ
        Gizmos.color = Color.red;
        Gizmos.DrawLine(target.position + (target.up * castOffset), transform.position);
    }

   
}
