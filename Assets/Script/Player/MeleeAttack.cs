using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamagable>(out IDamagable target))
        {
            if (target != null)
            {
                target.Damaged(Player.Instance.Attack);
            }
        }
    }
}
