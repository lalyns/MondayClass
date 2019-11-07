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
            if(!player.isSkill3)
                cam.isWallState = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Wall")
        {
            cam.isWallState = false;
        }

    }
}
