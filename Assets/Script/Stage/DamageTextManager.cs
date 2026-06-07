using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance { get; private set; }

    [SerializeField] private DamageText[] _texts;

    private readonly Queue<DamageText> _queue = new Queue<DamageText>();
    private List<DamageText> _aliveList = new List<DamageText>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        foreach (DamageText target in _texts)
        {
            _queue.Enqueue(target);
            target.gameObject.SetActive(false);
        }
    }

    public void ShowDamage(float damage, Vector3 pos)
    {
        if (_queue.Count <= 0 && _aliveList.Count <= 0)
        {
            return;
        }
        DamageText target;
        if (_queue.Count > 0)
        {
             target = _queue.Dequeue();
        }
        else
        {
            // 가장 먼저 활성화된 텍스트 queue로 회수
            _aliveList[0].ReturnPool();
            target = _queue.Dequeue();
        }
        target.SetText(damage, pos);
        _aliveList.Add(target);
        target.gameObject.SetActive(true);
    }

    public void ReturnPool(DamageText obj)
    {
        obj.gameObject.SetActive(false);
        _queue.Enqueue(obj);
        _aliveList.Remove(obj);
    }
}
