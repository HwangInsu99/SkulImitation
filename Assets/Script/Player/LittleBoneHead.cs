using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBoneHead : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private LittleBone _skul;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private float _speed;
    [SerializeField] private float _attackMultiply = 1.5f;
    [SerializeField] private float _maxFly = 3.0f;
    [SerializeField] private float _rotateSpeed = 720f;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private string _attackLayer = "PlayerAttack";
    [SerializeField] private string _falledLayer = "Default";
    
    private float _flyTime;
    private float _remainTime;
    private float _dir;
    private float _damage;
    private bool _isAttack;
    private float _origingravity;
    private bool _isReturn;
    private int _attack;
    private int _falled;

    private void Awake()
    {
        _origingravity = _rb.gravityScale;
        _attack = LayerMask.NameToLayer(_attackLayer);
        _falled = LayerMask.NameToLayer(_falledLayer);
    }

    private void Update()
    {
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            ReturnSkul();
            return;
        }
        if (_isAttack)
        {
            _flyTime -= Time.deltaTime;
            transform.Rotate(0, 0, _rotateSpeed * -_dir * Time.deltaTime);
            if (_flyTime <= 0)
            {
                _flyTime = 0;
                IsFalling();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isAttack)
        {
            _rb.velocity = new Vector2(_dir * _speed, 0f);
        }
    }

    public void SetHead(float dir, float time, float damage)
    {
        _dir = dir;
        _renderer.flipX = dir < 0;
        _remainTime = time;
        _damage = damage * _attackMultiply;
        _isAttack = true;
        _isReturn = false;
        gameObject.layer = _attack;
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
        _collider.isTrigger = true;
        _flyTime = _maxFly;
    }

    void ReturnSkul()
    {
        if (_isReturn)
        {
            return;
        }
        _isReturn = true;
        _skul.CollectProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAttack && other.TryGetComponent<IDamagable>(out IDamagable target))
        {
            target.Damaged(_damage);
        }

        if (_isAttack)
        {
            IsFalling();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(_playerTag))
        {
            ReturnSkul();
        }      
    }

    void IsFalling()
    {
        gameObject.layer = _falled;
        _collider.isTrigger = false;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = _origingravity;
        _isAttack = false;
    }
}