using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    Dungeon dungeon;

    private void Awake()
    {
        dungeon = GetComponentInParent<Dungeon>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
        }
    }
}
