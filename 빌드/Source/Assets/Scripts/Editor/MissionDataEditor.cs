using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MissionManager))]
public class CreateMissionDataClass : Editor
{

    [MenuItem("Assets/Resources/00.System/01.Datas/01.MissionData")]
    public static MissionData CreateMissionData()
    {
        MissionData asset = ScriptableObject.CreateInstance<MissionData>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/00.System/01.Datas/01.MissionData/MissionData.asset");
        AssetDatabase.SaveAssets();

        ParsingMissionData(asset);

        return asset;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("데이터 생성"))
        {
            CreateMissionData();
        }

    }

    public static void ParsingMissionData(MissionData asset)
    {
        List<Dictionary<string, object>> missionData = CSVReader.Read("MissionDatas");

        asset.MissionTypes = new MissionManager.MissionType[missionData.Count];
        asset.MissionLength = new int[(int)MissionManager.MissionType.Last];
        asset.MissionLevel = new int[missionData.Count];
        asset.DreamCatcherCount = new int[missionData.Count];
        asset.MacCount = new int[missionData.Count];
        asset.TiberCount = new int[missionData.Count];

        MissionManager.MissionType oldType = MissionManager.MissionType.Annihilation;
        int length = 0;

        for (var i = 0; i < missionData.Count; i++)
        {
            asset.MissionTypes[i] = (MissionManager.MissionType)((int)missionData[i]["MissionType"] - 1);

            if (asset.MissionTypes[i] == oldType)
            {
                length++;
            }
            else
            {
                asset.MissionLength[(int)oldType] = length;
                length = 1;
                oldType = asset.MissionTypes[i];
            }


            asset.MissionLevel[i] = (int)missionData[i]["Level"];
            asset.DreamCatcherCount[i] = (int)missionData[i]["Monster1"];
            asset.MacCount[i] = (int)missionData[i]["Monster2"];
            asset.TiberCount[i] = (int)missionData[i]["Monster3"];

        }
    }
}
