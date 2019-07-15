using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateStatDataClass
{
    [MenuItem("Assets/FSM/StatData")]
    public static StatData CreateStatData()
    {
        StatData asset = ScriptableObject.CreateInstance<StatData>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/StatData.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
