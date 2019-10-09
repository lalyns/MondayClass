using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberStat : CharacterStat
{
    public MonsterData monsterData;

    public float[] damageCoefiiecient;

    public float addStrPerRound;
    public float addDefPerRound;

    protected override void Awake() {
        base.Awake();
        SetHp(_maxHp);
    }
}
