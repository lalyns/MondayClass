﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;

    public static bool isPopUp = false;
    public bool _EditorCursorLock = true;

    public bool _CharacterControl = true;

    bool _SimpleMode = false;
    public GameObject _MissionSimple;
    public GameObject _MissionFull;

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!_SimpleMode)
            {
                _MissionSimple.SetActive(true);
                _MissionFull.SetActive(false);
                _SimpleMode = !_SimpleMode;
            }
            else
            {
                _MissionSimple.SetActive(false);
                _MissionFull.SetActive(true);
                _SimpleMode = !_SimpleMode;
            }
        }

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
    
    /// <summary>
    /// 마우스 커서가 보이게 하는 매소드
    /// </summary>
    /// <param name="isLock"></param>
    public static void CursorMode(bool isLock)
    {
        if (_Instance._EditorCursorLock)
        {
            if (isLock)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
