using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class targetTest : MonoBehaviour
{
    public GameObject Controller;
    public Slider _HpBar;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(Controller.transform.position);
        _HpBar.transform.localPosition = screenPoint;
    }
}
