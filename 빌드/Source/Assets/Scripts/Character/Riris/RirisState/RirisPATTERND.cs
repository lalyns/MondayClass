using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisPATTERND : RirisFSMState
{
    public Transform[] TornadoLoc1;
    public Transform[] TornadoLoc2;
    public Transform[] TornadoLoc3;

    public float time = 0;
    public float duration = 3.3f;

    public bool set1Play = false;
    public bool set2Play = false;
    public bool set3Play = false;

    public float endTime = 12f;

    public override void BeginState()
    {
        base.BeginState();

        _manager.Anim.Play("PatternD");

        var voice = _manager.sound.ririsVoice;
        voice.PlayRirisVoice(this.gameObject, voice.darkblast);
    }

    public override void EndState()
    {
        base.EndState();
        time = 0;
        set1Play = false;
        set2Play = false;
        set3Play = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        time += Time.deltaTime;

        if(time >= duration)
        {
            var sound = _manager.sound.ririsSFX;

            if (!set1Play)
            {
                sound.PlayRirisSFX(this.gameObject, sound.tornaedoCastSFX);
                sound.PlayRirisSFX(_manager.gameObject, sound.tornaedoFirstSFX);
                foreach(Transform a in TornadoLoc1)
                {
                    // 대충 이펙트를 꺼내는 행위
                    var ob = BossEffects.Instance.tornaedo.ItemSetActive(a);
                    // 대충 꺼낸 이펙트 애니메이션을 실행하는 행위
                    ob.GetComponentInChildren<Animator>().Play("Toenaedo");
                }
                set1Play = true;
            }

            if (time >= duration * 2f)
            {
                if (!set2Play)
                {
                    sound.PlayRirisSFX(_manager.gameObject, sound.tornaedoLastSFX);
                    foreach (Transform a in TornadoLoc2)
                    {
                        // 대충 이펙트를 꺼내는 행위
                        var ob = BossEffects.Instance.tornaedo.ItemSetActive(a);
                        // 대충 꺼낸 이펙트 애니메이션을 실행하는 행위
                        ob.GetComponentInChildren<Animator>().Play("Toenaedo");
                    }
                    set2Play = true;
                }
            }
        }

        if (time >= endTime)
        {
            _manager.SetState(RirisState.PATTERNEND);
        }
        
    }




}
