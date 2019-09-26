using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // 버프관련 코드 작성후 삽입 요망

    PlayerFSMManager player;

    private void Awake()
    {
        player = PlayerFSMManager.instance;    
    }

    /*void ShieldPlayer()
    {
        player.isShield = true;
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            ShieldPlayer();
            gameObject.SetActive(false);
        }
    }
}
