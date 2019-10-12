using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCWeapon : MonoBehaviour
{

    CapsuleCollider WeaponCollider;
    float realTime;
    float processTime;
    public bool _Damaged = false;
    float _time;
    public static PCWeapon Instance;

    // Start is called before the first frame update
    PlayerFSMManager player => PlayerFSMManager.Instance;
    void Start()
    {
        WeaponCollider = GetComponent<CapsuleCollider>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float timeNow = Time.realtimeSinceStartup;
        Debug.Log(timeNow + " : " + realTime);

        if (!player.isHardAttack)
        {
            if (timeNow > realTime + GameManager.Instance.softDuration)
            {
                //Time.timeScale = 1;
                GameManager.Instance.TimeMagnificationMode = false;
                GameManager.Instance.TimeMagnificationValue = 1;
            }
        }
        if (player.isHardAttack)
        {
            if (timeNow > realTime + GameManager.Instance.hardDuration)
            {
                //Time.timeScale = 1;
                GameManager.Instance.TimeMagnificationMode = false;
                GameManager.Instance.TimeMagnificationValue = 1;
            }
        }
        if (_Damaged)
        {
            _time += Time.deltaTime;
            if (_time >= 0.1f)
            {
                _time = 0;
                _Damaged = false;
            }
        }
    }


    void BreakTime()
    {
        GameManager.Instance.TimeMagnificationMode = true;
        GameManager.Instance.TimeMagnificationValue = 0;

        if (GameManager.Instance.TimeMagnificationValue == 0)
        {
            realTime = Time.realtimeSinceStartup;
            processTime = 0;



        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_Damaged) return;

        if (other.transform.tag == "Monster")
        {
            BreakTime();
            _Damaged = true;
        }
    }
}
