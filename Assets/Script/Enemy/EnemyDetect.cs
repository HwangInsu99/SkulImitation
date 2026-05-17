using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private string _playerTag = "Player";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_enemy.Target != null)
        {
            return;
        }
        if (other.CompareTag(_playerTag))
        {
            if (other.attachedRigidbody == null)
            {
                return;
            }
            _enemy.SetTarget(other.attachedRigidbody.transform);
        }
    }
}
