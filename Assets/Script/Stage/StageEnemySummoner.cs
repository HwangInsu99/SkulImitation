using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnemySummoner : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyList;
    [SerializeField] private float _summonCycle;

    private Queue<Enemy> _enemyQueue = new Queue<Enemy>();
    private float _summonTime;
    private bool _isEnd = false;

    void Start()
    {
        _summonTime = _summonCycle;
        foreach (Enemy enemy in _enemyList)
        {
            _enemyQueue.Enqueue(enemy);
            enemy.AddEnemyCount();
            enemy.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (_isEnd)
        {
            return;
        }
        _summonTime -= Time.deltaTime;
        if (_summonTime <= 0)
        {
            _summonTime = _summonCycle;
            EnemySummon();
        }
    }

    void EnemySummon()
    {
        if (_enemyQueue.Count <= 0)
        {
            _isEnd = true;
            return;
        }
        int summonCount = _enemyList.Count / 3;
        if (summonCount > _enemyQueue.Count)
        {
            summonCount = _enemyQueue.Count;
        }
        for (int i = 0; i < summonCount; i++)
        {
            Enemy enemy = _enemyQueue.Dequeue();
            enemy.gameObject.SetActive(true);
        }
    }
}
