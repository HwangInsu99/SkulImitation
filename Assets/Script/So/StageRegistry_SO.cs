using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / StageRegistry", fileName = "StageRegistrySO")]
public class StageRegistry_SO : ScriptableObject
{
    [SerializeField]
    private List<StageData_SO> _stages = new List<StageData_SO>();

    public IReadOnlyList<StageData_SO> Stages => _stages;

    private Dictionary<string, StageData_SO> _dataDic = new Dictionary<string, StageData_SO>();
    private Dictionary<EScenes, string> _sceneDic = new Dictionary<EScenes, string>();
        
    void NullCheck()
    {
        if (_dataDic != null && _dataDic.Count != 0)
        {
            return;
        }

        MakeDic();
    }

    public void MakeDic()
    {
        _dataDic.Clear();
        _sceneDic.Clear();

        for (int i = 0; i < _stages.Count; i++)
        {
            _dataDic.Add(_stages[i].StageID, _stages[i]);
            _sceneDic.Add(_stages[i].SceneEnum, _stages[i].StageID);
        }
    }

    public StageData_SO GetStageDataByID(string ID)
    {
        NullCheck();

        if (_dataDic.TryGetValue(ID, out StageData_SO data))
        {
            return data;
        }

        Debug.LogError("StageRegistry - Cant Find");
        return null;
    }

    public string GetStageDataByEnum(EScenes Enum)
    {
        NullCheck();

        if (_sceneDic.TryGetValue(Enum, out string data))
        {
            return data;
        }

        Debug.LogError("StageRegistry - Cant Find");
        return null;
    }
}
