using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    [SerializeField] private Boss _boss;
    [SerializeField] private Image _gauge;

    private float _maxHp;

    void Start()
    {
        _maxHp = _boss.MaxHp;
        _boss.HpChange += HPChange;
    }

    void HPChange(float hp)
    {
        _gauge.fillAmount = hp / _maxHp;
    }
}
