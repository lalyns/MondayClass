using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionProgress : MonoBehaviour
{
    Slider slider;
    public Text text;
    public Image boss;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    public void Update()
    {
        this.slider.gameObject.SetActive(GameStatus.currentGameState != CurrentGameState.Dialog &&
            GameStatus.currentGameState != CurrentGameState.Product &&
            GameStatus.currentGameState != CurrentGameState.Dead &&
            GameStatus.currentGameState != CurrentGameState.MissionClear);

        slider.value = GameStatus.Instance.StageLevel;
        text.text = GameStatus.Instance.StageLevel + "/8";
    }
}
