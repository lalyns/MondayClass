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
    float speed = 0.3f;

    [Range(0,2)] float slope = 2f;

    public IEnumerator DamageDisplaying(float damage)
    {
        currentCount++;
        if(currentCount >= texts.Length)
        {
            currentCount = 0;
        }
        yield return null;

        int count = currentCount;

        float time = 0;
        texts[count].gameObject.SetActive(true);
        texts[count].text = damage - GetComponentInParent<CharacterStat>().Defense + "!";
        texts[count].rectTransform.localPosition = startPos;

        while(time < 1f)
        {
            time += 0.05f;
            texts[count].rectTransform.localPosition =
                new Vector3(startPos.x, slope * (startPos.x + time * speed) * (startPos.x + time * speed) + startPos.y, 0);
            
            yield return new WaitForSeconds(0.05f);
        }
        texts[count].gameObject.SetActive(false);
    }

    
}
