using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            Player target = other.GetComponentInParent<Player>();
            if (target != null)
            {
                target.Damaged(target.MaxHp * 10);
            }
        }
    }
}
