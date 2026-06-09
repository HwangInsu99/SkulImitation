using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRush : MonoBehaviour
{
    [SerializeField] private Boss _boss;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private string _wallLayer = "Wall";
    [SerializeField] private float _damageMultiply;

    private int _targetLayer;

    private void Awake()
    {
        _targetLayer = LayerMask.NameToLayer(_wallLayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            IDamagable player = other.GetComponentInParent<IDamagable>();
            if (player != null)
            {
                SoundManager.Instance.PlaySfx(ESfxType.Hit_Blow);
                player.Damaged(_boss.AttackDamage * _damageMultiply);
            }
        }

        if (other.gameObject.layer == _targetLayer)
        {
            _boss.RushEnd();
        }
    }
}
