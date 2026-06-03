using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceData : MonoBehaviour
{
    [SerializeField] private StageSelectStat_SO[] _stats;
    [SerializeField] private UpgradeButton[] _buttons = new UpgradeButton[3];

    private List<StageSelectStat_SO> _statPool;

    public void StageClear()
    {
        _statPool = new List<StageSelectStat_SO>(_stats);

        for (int i = 0; i < _buttons.Length; i++)
        {
            int rand = Random.Range(0, _statPool.Count);            
            _buttons[i].SetStat(_statPool[rand]);
            _statPool.RemoveAt(rand);
        }
    }
}
