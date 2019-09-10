using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MissionManager))]
public class CreateMissionDataClass : Editor
{
    //[MenuItem("Assets/Resources/00.System/01.Datas/01.MissionData")]
    //public static MissionData CreateMissionData()
    //{
    //    MissionData asset = ScriptableObject.CreateInstance<MissionData>();
    //    AssetDatabase.CreateAsset(asset, "Assets/Resources/00.System/01.Datas/01.MissionData/MissionData.asset");
    //    AssetDatabase.SaveAssets();

    //    //MissionManager._Instance._MissionDatas[0] = asset;

    //    ParsingMissionData(asset);

    //    return asset;
    //}

    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();

    //    if(GUILayout.Button("데이터 생성"))
    //    {
    //        CreateMissionData();
    //    }

    //}

    //public static void ParsingMissionData(MissionData asset)
    //{
    //    List<Dictionary<string, object>> missionData = CSVReader.Read("MissionDatas");

    //    asset.MissionTypes = new MissionManager.MissionType[missionData.Count];

    //    asset.MissionString = new string[(int)MissionManager.MissionType.Last];
    //    asset.MissionSubject = new string[(int)MissionManager.MissionType.Last];
    //    asset.MissionName = new Sprite[(int)MissionManager.MissionType.Last];
    //    for(var i = 0; i<asset.MissionName.Length; i++)
    //    {
    //        string path = @"01.UI/01.Sprites/UI_Mission_Name_" + ((MissionManager.MissionType)i).ToString();
    //        asset.MissionName[i] = Resources.Load<Sprite>(path);
    //        asset.MissionString[i] = "카카";
    //        asset.MissionSubject[i] = "히히";
    //    }

    //    asset.MissionIcon = new Sprite[(int)MissionManager.MissionType.Last];
    //    asset.MissionText = new string[(int)MissionManager.MissionType.Last];
    //    for (var i = 0; i < asset.MissionIcon.Length; i++)
    //    {
    //        string path = @"01.UI/01.Sprites/UI_Mission_Icon_" + ((MissionManager.MissionType)i).ToString();
    //        asset.MissionIcon[i] = Resources.Load<Sprite>(path);
    //        asset.MissionText[i] = "하하";
    //    }


    //    asset.MissionLength = new int[(int)MissionManager.MissionType.Last];
    //    asset.MissionLevel = new int[missionData.Count];

    //    asset.RedHatCount = new int[missionData.Count];
    //    asset.MacCount = new int[missionData.Count];
    //    asset.TiberCount = new int[missionData.Count];

    //    MissionManager.MissionType oldType = MissionManager.MissionType.Annihilation;
    //    int length = 0;

    //    for (var i = 0; i < missionData.Count; i++)
    //    {
    //        asset.MissionTypes[i] = (MissionManager.MissionType)((int)missionData[i]["MissionType"] - 1);

    //        if (asset.MissionTypes[i] == oldType)
    //        {
    //            length++;
    //        }
    //        else
    //        {
    //            asset.MissionLength[(int)oldType] = length;
    //            length = 1;
    //            oldType = asset.MissionTypes[i];
    //        }

    //        asset.MissionLevel[i] = (int)missionData[i]["Level"];
    //        asset.RedHatCount[i] = (int)missionData[i]["Monster1"];
    //        asset.MacCount[i] = (int)missionData[i]["Monster2"];
    //        asset.TiberCount[i] = (int)missionData[i]["Monster3"];

    //    }

    //    asset.NumberOfTimesSpawn = 5;
    //    asset.CycleOfTimeRespawn = 20;
    //}
}
