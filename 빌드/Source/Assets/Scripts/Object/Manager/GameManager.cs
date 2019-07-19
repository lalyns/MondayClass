using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isPopUp = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Menu");

            isPopUp = !isPopUp;
            if (isPopUp)
            {
                DungeonManager.MissionPopUp();
            }
            else
            {
                DungeonManager.MissionDisappear();
            }
        }
    }
}
