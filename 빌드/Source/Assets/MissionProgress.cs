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
        slider = GetComponent<Slider>();
    }

    public void Update()
    {
        slider.value = GameStatus.Instance.StageLevel;
        text.text = GameStatus.Instance.StageLevel + "/8";
    }
}
