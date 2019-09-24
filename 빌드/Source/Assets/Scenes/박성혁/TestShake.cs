using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShake : MonoBehaviour
{

    public float duration =0.05f;
    public float magitudePos = 0.03f;
    public float magtitudeRot = 0.1f;

    public Transform mainCamera;
    private Vector3 originPos;
    private Quaternion originRot;
    public static TestShake instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        originPos = mainCamera.localPosition;
        originRot = mainCamera.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ShakeCamera(duration, magitudePos, magtitudeRot));
        }
    }

    public IEnumerator ShakeCamera(float duration = 0.05f, float magnitudePos = 0.03f, float magitudeRot = 0.1f)
    {
        float passTime = 0.0f;


        while (passTime < duration)
        {
            Vector3 shakePos = Random.insideUnitSphere;
            mainCamera.localPosition = shakePos * magnitudePos;

            //if (shakeRotate)
            //{
            //    Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magitudeRot, 0.0f));

            //    mainCamera.localRotation = Quaternion.Euler(shakeRot);

            //}
            passTime += Time.deltaTime;

            yield return null;
        }

        mainCamera.localPosition = originPos;
        mainCamera.localRotation = originRot;
    }
}
