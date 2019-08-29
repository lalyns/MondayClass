using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InputHandler))]
public class HPUI : MonoBehaviour
{
    InputHandler input;
    // Start is called before the first frame update
    void Start()
    {
        input = InputHandler.instance;

    }
    //438
    //1427 16.19
    // Update is called once per frame
    void Update()
    {
        //this.transform.localPosition = new Vector3(-294 + 297 * (input.playerHP / input.playerMaxHP), 0f, 0f);

    }
}
