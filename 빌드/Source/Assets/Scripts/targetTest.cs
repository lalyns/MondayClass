using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetTest : MonoBehaviour
{
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x,
            player.position.y, this.transform.position.z);
        this.transform.LookAt(player);
    }
}
