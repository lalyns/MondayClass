using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public Transform target;
    public float moveDamping = 99999f;
    public float rotateDamping = 10.0f;
    [Header("거리, 현재위치, 마우스위치(기본1)")]
    public float distance = 4.0f;
    public float height = 3.0f;

    public bool isWall = false;
    public float nearDistance = 1.0f;
    public float originDistance = 4f;
    public float targetOffset = 1.0f;

    [Header("벽 충돌 세팅, originHeight = 높이")]
    public float heightAboveWall = 7.0f; // 카메라가 올라갈 높이
    public float colliderRadius = 1f; // 충돌체의 반지름
    public float overDamping = 5.0f; // 이동속도 계수
    public float originHeight; // 최소 높이를 보관할 변수

    [Header("Etc Obstacle Setting")]
    //카메라가 올라갈 높이
    public float heightAboveObstacle = 12.0f;
    //플레이어 투사할 레이캐스트의 높이 옵셋
    public float castOffset = 1.0f;

    public PlayerFSMManager player;

    public bool isWallState;
    void Start()
    {
        player = PlayerFSMManager.Instance;

        originHeight = height;
        originDistance = distance;

        target = GameObject.Find("PC_Rig").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //구체 형태의 충돌체로 충돌 여부를 검사

        if (isWall)
        {
            //Debug.Log("iswall인상태");
        }
        if (isWallState)
        {
            if (Physics.CheckSphere(transform.position, 0))
            {
                //보간함수를 사용하여 카메라의 높이를 부드럽게 상승시킴.
                //height = Mathf.Lerp(height, heightAboveWall, Time.deltaTime * overDamping);
                //isWall = true;
                //if (!isMax)
                distance = Mathf.Lerp(distance, nearDistance, Time.deltaTime * overDamping * 10f);

                //Debug.Log("체크스페어상태");
            }
        }
        else// if (!isWall)
        {
            //보간함수를 이용하여 카메라의 높이를 부드럽게 하강시킨다.
            height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping * 5f);
            if (!isMax)
                distance = Mathf.Lerp(distance, originDistance, Time.deltaTime * overDamping * 10f);

            //Debug.Log("체크스페어아닌상태");
        }
        //플레이어가 장애물에 가려졌는지를 판단할 레이캐스트의 높낮이를 설정
        Vector3 castTarget = target.position + (target.up * castOffset);
        //castTarget 좌표로의 방향벡터 계산
        Vector3 castDir = (castTarget - transform.position).normalized;
        //충돌 정보를 반환받을 변수
        RaycastHit hit;

        //레이캐스트를 투사해 장애물 여부 판단
        if (Physics.Raycast(transform.position, castDir, out hit, Mathf.Infinity))
        {
            //플레이어가 레이캐스트에 맞지 않았을 경우
            if (!hit.collider.CompareTag("Player"))
            {
                isWall = true;
                //보간함수 사용 카메라 상승
                //height = Mathf.Lerp(height, heightAboveObstacle, Time.deltaTime * overDamping / 2f);
                if (!isMax)
                    distance = Mathf.Lerp(distance, nearDistance, Time.deltaTime * overDamping / 3.5f);
            }
            else
            {
                height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping * 10f);
                if(!isMax)
                    distance = Mathf.Lerp(distance, originDistance, Time.deltaTime * overDamping * 10f);
                isWall = false;
            }
        }
            
    }
    public float r_y = 0.00f;
    [Header("Y축 마우스감도, 최소, 최대")]
    public float mouseSpeedY = 5f;
    public float minAngle = 0.7f;
    public float maxAngle = 3f;
    public float minHeight = 0.4f;
    public float maxHeight = 3f;
    public float minDistance = 1.5f;
    public float maxDistance = 3f;
    public bool isMax, isMin;
    float tFollowH = 12.3f;
    bool islock = false;

    private void FixedUpdate()
    {
        if ((GameStatus.currentGameState == CurrentGameState.Select || GameStatus.currentGameState == CurrentGameState.Dialog) || player.isSkill4 || player.isDead)
            return;
        r_y = Input.GetAxis("Mouse Y");

        if (player.isMouseYLock)
        {
            maxDistance = 5.5f;
            maxHeight = 4f;
            originHeight = 4f;
            height = 4f;
            distance = 5.5f;
            targetOffset = 0.7f;
            islock = false;
            return;
        }
        if (!player.isMouseYLock && !islock && !isWall && !isMax)
        {
            maxDistance = 4f;
            maxHeight = 3f;
            islock = true;
        }
        if(distance <= 1)
        {
            distance = 1;
        }
        // 마우스 위치와 높이값
        if (!isMax && !isMin)
        {
            // 높이와 위치가 최대가 되지 않는 한 계속 이동시킴 마우스 위치의 Max값 까지.
            targetOffset += r_y * Time.fixedDeltaTime * mouseSpeedY;
            originHeight -= r_y * Time.fixedDeltaTime * mouseSpeedY * tFollowH;
        }
        // 예외처리
        if (targetOffset < minAngle)
        {
            targetOffset = minAngle;
        }
        // 예외처리 및 최대값 도달 시 더이상 높이와 마우스 위치는 변하지 않음.
        if (targetOffset >= maxAngle)
        {
            targetOffset = maxAngle;
            isMax = true;
        }
        if (targetOffset < maxAngle)
        {
            isMax = false;
        }
        // 예외처리 최저높이
        if (originHeight <= minHeight)
        {
            originHeight = minHeight; // 고정 시키고
        }
        // 예외처리 및 최대높이 도달 시 더이상 높이와 마우스 위치는 변하지 않음.
        if (originHeight >= maxHeight)
        {
            originHeight = maxHeight;
            //isMin = true;
        }
        if (isMax)
        {
            distance -= r_y * Time.fixedDeltaTime * mouseSpeedY * tFollowH;

            if (distance > maxDistance)
            {
                isMax = false;
            }
        }
        if (!isWall)
        {
            //if (distance <= minDistance)
            //{
            //    distance = minDistance;
            //}
        }
        if (isWall)
        {
            //if (distance <= nearDistance)
            //{
            //    distance = nearDistance;
            //}
        }
        if (isMin)
        {
            distance += r_y * Time.fixedDeltaTime * mouseSpeedY * tFollowH / 2f;
            if (distance > maxDistance)
            {
                isMin = false;
            }
        }
        if(isMax && (distance == 1) && (r_y >= 0.001f))
        {
            r_y = 0;
        }

    }

    private void LateUpdate()
    {

        var camPos = target.position - (target.forward * distance) + (target.up * height);
        transform.position = camPos;
        //transform.position = Vector3.Slerp(transform.position, camPos, Time.deltaTime * moveDamping);
        //transform.rotation = target.rotation;//Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * rotateDamping);
        transform.LookAt(target.position + (target.up * targetOffset));
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);
    //    Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);

    //    //카메라의 충돌체를 표현하기 위한 구체를 표시
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, colliderRadius);

    //    //플레이어가 장애물에 가려졌는지 판단할 레이를 표사ㅣ
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(target.position + (target.up * castOffset), transform.position);
    //}

    private void OnTriggerStay(Collider other)
    {
       if(other.transform.tag == "Wall")
        {
            Debug.Log("벽에닿은상태");
        }
    }
}
