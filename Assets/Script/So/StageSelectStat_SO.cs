using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / StageSelectStat", fileName = "Select_")]
public class StageSelectStat_SO : ScriptableObject
{
    [SerializeField] private EPlayerStat _stat;
    [SerializeField] private float _value;
    [SerializeField] private string _info;
    [SerializeField] private Sprite _icon;

    public EPlayerStat Stat => _stat;
    public float Value => _value;
    public string Info => _info;
    public Sprite Icon => _icon;
}
