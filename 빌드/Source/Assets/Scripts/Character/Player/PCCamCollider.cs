using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCamCollider : MonoBehaviour
{
    PlayerFSMManager player;
    FollowCam cam;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerFSMManager.Instance;
        cam = player.followCam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Wall")
        {
            cam.isWallState = true;
            Debug.Log("벽");

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Wall")
        {
            cam.isWallState = false;
            Debug.Log("벽끝");
        }

    }
}
