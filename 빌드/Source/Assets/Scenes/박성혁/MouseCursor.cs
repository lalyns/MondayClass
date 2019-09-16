using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    private void Start()
    {
    }

    public bool cursorMode = true;

    private void Update()
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        GameManager.CursorMode(cursorMode);

        if (Input.GetKeyDown(KeyCode.K))
        {
            cursorMode = !cursorMode;
        }
    }
}
