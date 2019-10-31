using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisULTIMATE : RirisFSMState
{
    [System.Serializable]
    public class UltiPattern
    {
        public Transform[] startTrans;
        public Transform[] targetTrans;
    }

    [System.Serializable]
    public class BigCircle
    {
        public GameObject gameObject;
        public ParticleSystem[] particle;
        public Animator anim;
    }

    [System.Serializable]
    public class SmallCircle
    {
        public GameObject gameObject;
        public ParticleSystem[] particle;
    }

    public BigCircle bigCircle;
    public SmallCircle[] smallCircle = new SmallCircle[8];

    public UltiPattern[] ultiPatterns = new UltiPattern[5];

    Vector3 bossUltPos = new Vector3(-20.0f, 0.12f, -20.5f);

    int currentList = 0;
    List<GameObject>[] flowerLists = new List<GameObject>[5];
    List<GameObject>[] beamLists = new List<GameObject>[5];

    public UltiPattern currentPattern;

    public override void BeginState()
    {
        base.BeginState();

        useGravity = false;
        this.transform.position = bossUltPos;
        _manager.Anim.transform.LookAt
            (PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));
        BigCircleCast();
    }

    public override void EndState()
    {
        base.EndState();

        useGravity = true;
        this.transform.position = Vector3.up * 0.12f;
        for (int i = 0; i < flowerLists.Length; i++)
        {
            flowerLists[i].Clear();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        flowerLists = new List<GameObject>[5];
        beamLists = new List<GameObject>[5];
        currentList = 0;

        for (int i = 0; i < flowerLists.Length; i++)
        {
            flowerLists[i] = new List<GameObject>();
            beamLists[i] = new List<GameObject>();
        }

    }

    public void BigCircleCast()
    {
        var voice = _manager.sound.ririsVoice;
        voice.PlayRirisVoice(_manager.gameObject, voice.special);

        bigCircle.gameObject.SetActive(true);
        bigCircle.particle[0].Play();
        bigCircle.particle[1].Play();
        bigCircle.anim.Play("play");

        Invoke("SoundCast", 1.5f);

        Invoke("SmallCircleCast", 2.8f);
    }

    void SoundCast()
    {
        var sound = _manager.sound.ririsSFX;
        sound.PlayRirisSFX(_manager.gameObject, sound.ultGate);
    }

    public void SmallCircleCast()
    {
        var sound = _manager.sound.ririsSFX;
        sound.PlayRirisSFX(_manager.gameObject, sound.ultSmallGate);

        foreach (SmallCircle small in smallCircle)
        {
            small.gameObject.SetActive(true);
            for(int i =0; i<small.particle.Length; i++)
            {
                small.particle[i].Play();
            }
        }

        First();
        Invoke("Second", 1.2f);
        Invoke("Third", 2.4f);
        Invoke("Fourth", 3.6f);
        Invoke("Fifth", 4.8f);
    }

    public IEnumerator UltimatePlay(UltiPattern pattern, float delay)
    {
        for (int i = 0; i < pattern.startTrans.Length; i++)
        {
            yield return new WaitForSeconds(delay);

            GameObject flower = BossEffects.Instance.flower.ItemSetActive(pattern.targetTrans[i].position);
            flower.GetComponent<BossUltEffect>().setEffect.PlayEffects();

            flowerLists[currentList].Add(flower);

            // 대충 이펙트 호출하는 코드

        }

        Invoke("SetBeams", 0.2f);
    }

    public void SetBeams()
    {
        StartCoroutine(SetBeam(currentPattern));
    }

    public IEnumerator SetBeam(UltiPattern pattern)
    {
        var sound = _manager.sound.ririsSFX;

        int value = currentList;
        for (int i = 0; i < pattern.startTrans.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);

            GameObject beam = BossEffects.Instance.beam.ItemSetActive(pattern.startTrans[i].position);
            beamLists[value].Add(beam);
            beam.transform.LookAt(pattern.targetTrans[i].position);
            StartCoroutine(Shake.instance.ShakeCamera(0.15f, 0.1f, 0.1f));

            sound.PlayRirisSFX(_manager.gameObject, sound.ultBeam);
            Invoke("BlastSound", 0.8f);

            flowerLists[value][i].GetComponent<BossUltEffect>().impactEffect.PlayEffects();
        }

        yield return StartCoroutine(SetFalse(beamLists[value], flowerLists[value]));
    }

    void BlastSound()
    {
        var sound = _manager.sound.ririsSFX;
        sound.PlayRirisSFX(_manager.gameObject, sound.ultBlast);
    }

    public IEnumerator SetFalse(List<GameObject> beam, List<GameObject> flower)
    {
        yield return new WaitForSeconds(2f);

        for(int i=0; i<beam.Count; i++)
        {
            BossEffects.Instance.beam.ItemReturnPool(beam[i]);
            BossEffects.Instance.flower.ItemReturnPool(flower[i]);
        }
    }

    void First()
    {
        currentPattern = ultiPatterns[0];
        currentList = 0;
        StartCoroutine(UltimatePlay(currentPattern, 0.01f));
    }

    void Second()
    {
        currentPattern = ultiPatterns[1];
        currentList = 1;
        StartCoroutine(UltimatePlay(currentPattern, 0.01f));
    }

    void Third()
    {
        currentPattern = ultiPatterns[2];
        currentList = 2;
        StartCoroutine(UltimatePlay(currentPattern, 0.01f));
    }

    void Fourth()
    {
        currentPattern = ultiPatterns[3];
        currentList = 3;
        StartCoroutine(UltimatePlay(currentPattern, 0.01f));
    }

    void Fifth()
    {
        currentPattern = ultiPatterns[4];
        currentList = 4;
        StartCoroutine(UltimatePlay(currentPattern, 0.01f));
    }

    public void PatternEnd()
    {
        _manager.SetState(RirisState.PATTERNEND);
    }
}
