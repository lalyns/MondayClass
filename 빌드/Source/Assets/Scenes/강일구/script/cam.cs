using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartRutine());

    }

    IEnumerator StartRutine()
    {
        do
        {
            if (Input.GetMouseButtonUp(0))
            {
                //왼쪽 버튼 누르면 스타트 버튼으로 체킹
                StartButton
            }
        }


    }


    // Update is called once per frame
    void Update()
    {

        
        
    }
}
