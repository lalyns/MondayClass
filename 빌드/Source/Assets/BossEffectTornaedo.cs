using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffectTornaedo : MonoBehaviour
{
    float time = 0;
    public float tornaedoCount = 1.2f;

    public GameObject circle;
    public GameObject tornaedo;

    public bool isPlay = false;

    private void OnEnable()
    {
        circle.SetActive(true);
        tornaedo.SetActive(false);
        isPlay = false;
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > tornaedoCount - 0.2f)
        {
            if (time > tornaedoCount)
            {
                circle.SetActive(false);
            }

            if (!isPlay)
            {

                tornaedo.SetActive(true);
                tornaedo.GetComponentInChildren<Animator>().Play("Play");
                isPlay = true;
            }
        }
    }

}
