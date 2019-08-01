using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedTarget : MonoBehaviour
{
    public Defence _Defence;

    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = _Defence._ProtectedTargetHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Bullet")
        {
            hp -= 10;
        }
    }
}
