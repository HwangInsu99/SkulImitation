using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUltimate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _speed;
    [SerializeField] private Boss _boss;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private float _damageMultiply;

    private float _remainTime = 2f;
    private float _dir;
    private bool _isMove = false;

    void Update()
    {
        if (!_isMove)
        {
            return;
        }

        _remainTime -= Time.deltaTime;
        transform.position += Vector3.right * _dir * _speed * Time.deltaTime;

        if (_remainTime <= 0)
        {
            _isMove = false;
            gameObject.SetActive(false);
        }
    }

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

    public void Fire(float dir)
    {
        _renderer.flipX = dir < 0;
        _dir = dir;
        gameObject.SetActive(true);
    }

    void Move()
    {
        _isMove = true;
    }
}
