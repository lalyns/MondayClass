using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;

    public static bool isPopUp = false;
    public bool _EditorCursorLock = true;

    public bool _CharacterControl = true;

    private void Awake()
    {
        if(_Instance == null)
        {
            _Instance = GetComponent<GameManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (_EditorCursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
       


    }

    private void OnGUI()
    {
        if (GUI.RepeatButton(new Rect(Screen.width / 100f * 80f , Screen.height * 0.8f, 230, 85), 
            "조작법 \n" +
            "이동 : W A S D \n" +
            "공격 : 마우스 좌클릭\n" +
            "회피 : LeftShift \n" +
            "스킬 : F"
            )) { }

    }

    public static void CursorMode(bool isLock)
    {
        if (_Instance._EditorCursorLock)
        {
            if (isLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
