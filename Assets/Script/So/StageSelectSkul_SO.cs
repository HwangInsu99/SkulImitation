using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / StageSelectSkul", fileName = "Skul_")]
public class StageSelectSkul_SO : ScriptableObject
{
    [SerializeField] private Skul _skulPrefab;
    [SerializeField] private string _info;
    [SerializeField] private Sprite _icon;

    public Skul SkulPrefab => _skulPrefab;
    public string Info => _info;
    public Sprite Icon => _icon;
}
