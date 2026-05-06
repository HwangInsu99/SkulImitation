using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / StageData", fileName = "StageDataSO")]
public class StageData_SO : ScriptableObject
{
    [SerializeField] private string stageID;
    [SerializeField] private EScenes sceneEnum;
    [SerializeField] private string stageName;

    public string StageID => stageID;
    public EScenes SceneEnum => sceneEnum;
    public string StageName => stageName;
}
