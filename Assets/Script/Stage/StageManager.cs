using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance {  get; private set; }

    [SerializeField] private int _remainEnemy;

    public event Action<int> EnemyChange;

    public int RemainEnemy => _remainEnemy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void IncreaseEnemy()
    {
        _remainEnemy++;
        EnemyChange?.Invoke(_remainEnemy);
    }

    public void DecreaseEnemy()
    {
        _remainEnemy--;
        EnemyChange?.Invoke(_remainEnemy);
        if (_remainEnemy <= 0)
        {
            _remainEnemy = 0;
            GameManager.Instance.StageClear();
        }
    }
}