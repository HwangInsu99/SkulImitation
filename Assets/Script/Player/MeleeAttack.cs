using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float _mutiplyDamage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamagable>(out IDamagable target))
        {
            if (target != null)
            {
                target.Damaged(Player.Instance.Attack * _mutiplyDamage);
            }
        }
    }

    public void DamageMul(float mul)
    {
        _mutiplyDamage = mul;
    }
}
