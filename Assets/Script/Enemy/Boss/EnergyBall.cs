using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [SerializeField] private Boss _boss;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private float _damageMultiply;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Vector3 _offset = new Vector3(0, 0.6f, 0);
    [SerializeField] private string _paramExplode = "tExplode";

    private float _aliveTime = 2.0f;
    private float _remainTime;
    private int _hashExplode;
    private Transform _target;
    private bool _isExplode = false;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        if (_boss  == null)
        {
            _boss = GetComponentInParent<Boss>();
        }
        _hashExplode = Animator.StringToHash(_paramExplode);
    }

    void Update()
    {
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            gameObject.SetActive(false);
        }

        if (_target == null || _isExplode)
        {
            return;
        }

        Vector2 dir = ((_target.position + _offset) - transform.position).normalized;

        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, _rotateSpeed * Time.deltaTime);

        transform.position += transform.right * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            IDamagable player = other.GetComponentInParent<IDamagable>();
            if (player != null)
            {
                _animator.SetTrigger(_hashExplode);
                _isExplode = true;
                player.Damaged(_boss.AttackDamage * _damageMultiply);
            }
        }
    }

    public void Fire(Transform target)
    {
        _target = target;
        _remainTime = _aliveTime;
        _isExplode = false;

        Vector2 dir = ((_target.position + _offset) - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        gameObject.SetActive(true);
    }

    void ExplodeEnd()
    {
        gameObject.SetActive(false);
    }
}
