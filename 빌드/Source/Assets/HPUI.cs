using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    public float MaxHP;
    public float HP;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //438
    //1427
    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(-431 + 419 * (HP / MaxHP), 0f, 0f);
    }
}
