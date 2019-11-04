using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    public Text[] texts;

    Vector3 startPos = new Vector3(0.3f, 0.058f, 0.0f);

    public Vector3 direction;

    int currentCount = 0;
    public float speed = 2f;

    [Range(0,2)] public float slope = 1.1f;

    public IEnumerator DamageDisplaying(float damage)
    {
        Debug.Log("Start!");

        currentCount++;
        if(currentCount >= texts.Length)
        {
            currentCount = 0;
        }
        yield return null;

        int count = currentCount;

        float time = 0;
        texts[count].gameObject.SetActive(true);
        texts[count].text = damage + "!";
        texts[count].rectTransform.localPosition = startPos;

        while(time < 1f)
        {
            time += 0.05f;
            texts[count].rectTransform.localPosition =
                new Vector3(startPos.x + time, -slope * (startPos.x + time) * (startPos.x + time) + startPos.y, 0);
            
            yield return new WaitForSeconds(0.05f);
        }
        texts[count].gameObject.SetActive(false);
    }

    
}
