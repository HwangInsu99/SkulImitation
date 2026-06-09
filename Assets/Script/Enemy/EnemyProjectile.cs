using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // 직선으로 날아가는 투사체
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private float _speed = 8;
    [SerializeField] private float _remainTime = 1.5f;
    private float _damage;

    private void Update()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            IDamagable player = other.GetComponentInParent<IDamagable>();
            if (player != null)
            {
                player.Damaged(_damage);
                SoundManager.Instance.PlaySfx(ESfxType.Hit_Penetrate);
            }            
        }
    }

    public void SetProjectile(float damage, float dir)
    {
        _damage = damage;
        transform.rotation = Quaternion.Euler(0, 0, dir > 0 ? 0 : 180);
    }
}
