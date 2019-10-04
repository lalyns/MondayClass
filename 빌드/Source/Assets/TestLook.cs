using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLook : MonoBehaviour
{
    public Animator[] set1;
    bool set1Play;
    public Animator[] set2;
    bool set2Play;
    public Animator[] set3;
    bool set3Play;

    public float duration;

    float time = 0;
    public float replayTime = 10f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time <= replayTime)
        {
            if (!set1Play)
            {
                foreach (Animator a in set1)
                {
                    a.gameObject.SetActive(true);
                    a.Play("Tornaedo");
                }
                set1Play = true;
            }
            
            if(time >= duration)
            {

                if (!set2Play)
                {
                    foreach (Animator a in set2)
                    {
                        a.gameObject.SetActive(true);
                        a.Play("Tornaedo");
                    }
                    set2Play = true;
                }
                if (time >= duration *2)
                {
                    if (!set3Play)
                    {
                        foreach (Animator a in set3)
                        {
                            a.gameObject.SetActive(true);
                            a.Play("Tornaedo");
                        }

                        set3Play = true;
                    }
                    
                }
            }
        }
        else
        {
            time = 0;
            set1Play = false;
            set2Play = false;
            set3Play = false;
        }
    }
}
