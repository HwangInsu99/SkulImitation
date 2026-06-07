using TMPro;
using UnityEngine;

public class StageUINormal : MonoBehaviour
{
    [SerializeField] private TMP_Text _remainText;
    [SerializeField] private StageManager _manager;

    void Start()
    {
        _manager = StageManager.Instance;
        _manager.EnemyChange += ChangeNum;
        ChangeNum(_manager.RemainEnemy);
    }

    void ChangeNum(int num)
    {
        _remainText.text = "³²Àº Àû : " + num.ToString();
    }
}
