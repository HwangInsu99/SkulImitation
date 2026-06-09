using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlash : MonoBehaviour
{
    [SerializeField] private Boss _boss;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private float _damageMultiply;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            IDamagable player = other.GetComponentInParent<IDamagable>();
            if (player != null)
            {
                SoundManager.Instance.PlaySfx(ESfxType.Hit_Slash);
                player.Damaged(_boss.AttackDamage * _damageMultiply);
            }
        }
    }
}
