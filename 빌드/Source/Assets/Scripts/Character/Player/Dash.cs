using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    public float distance;
    int maxDash = 3;
    float dashCoolTime = 3f;
    float currentDashCoolTime = 0f;
    int remainingDash = 0;

    public List<Image> dashImages = new List<Image>();
    private void Awake()
    {
        
    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0) && currentDashCoolTime >=3f)
        //{
        //    currentDashCoolTime -= 3f;
        //}
        if(Input.GetMouseButtonDown(0))
        {
            if (remainingDash == 1)
            {
                dashImages[1].fillAmount = 0;
                remainingDash--;
            }
            if(remainingDash == 2)
            {
                dashImages[2].fillAmount = 0;
                remainingDash--;
            }
            if (remainingDash == 3)
            {
                remainingDash--;
            }
        }

        if (remainingDash < maxDash)
        {
            currentDashCoolTime += Time.deltaTime;
            if(currentDashCoolTime >= dashCoolTime)
            {
                remainingDash++;
                currentDashCoolTime = 0;
            }
            for(int i=0; i<3; i++)
            {
                if(remainingDash == i)
                {
                    dashImages[i].fillAmount = currentDashCoolTime / 3f;
                }
            }
        }




        //// 현재 대쉬 가능한 횟수가 최대치가 아니면
        //if(remainingDash < maxDash)
        //{
        //    // 타이머 작동 후

        //    // 일정시간 지나면 대쉬 게이지 증가.
        // 총 9초 동안 모을 수 있음.
        //if (currentDashCoolTime <= dashCoolTime)
        //{            
        //    currentDashCoolTime += Time.deltaTime;

        //    if (currentDashCoolTime <= 3f)
        //    {
        //        dashImages[0].fillAmount = currentDashCoolTime / 3f;
        //    }
        //    if (currentDashCoolTime > 3f || currentDashCoolTime <= 6f)
        //    {
        //        dashImages[0].fillAmount = 1f;
        //        dashImages[1].fillAmount = currentDashCoolTime / 6f;
        //    }
        //    if (currentDashCoolTime > 6f)
        //    {
        //        dashImages[0].fillAmount = 1f;
        //        dashImages[1].fillAmount = 1f;
        //        dashImages[2].fillAmount = currentDashCoolTime / 9f;
        //    }

        //    //remainingDash++;
        //    //currentDashCoolTime = 0f;
        //    //   }
        //}
        //if (currentDashCoolTime >= 9f)
        //    currentDashCoolTime = 9f;
    }

}
