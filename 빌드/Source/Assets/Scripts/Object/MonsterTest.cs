using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{
    public MonsterData _MonsterData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            ObjectManager.ReturnPoolMonster(gameObject, _MonsterData._IsRagne);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Weapon")
        {
            ObjectManager.ReturnPoolMonster(gameObject, _MonsterData._IsRagne);
        }
    }
}
