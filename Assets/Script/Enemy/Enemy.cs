using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    public enum EEState
    {
        Idle,
        Chase,
        Patrol,
        Attack,
    }

    [SerializeField] protected Transform _target;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected string _paramSpeedX = "aSpeedX";
    [SerializeField] protected string _paramAttack = "tAttack";

    protected int _hashSpeedX;
    protected int _hashAttack;
    protected float _dir;
    protected float _speed = 3.0f;
    protected float _hp;
    protected float _maxHp;
    [SerializeField]protected EEState _state;
    protected float _patrolTimeOrigin = 2.0f;
    protected float _patrolTime;
    protected float _waitingTimeOrigin = 5.0f;
    protected float _waitingTime;

    public Transform Target => _target;

    private void Awake()
    {
        _hashSpeedX = Animator.StringToHash(_paramSpeedX);
        _hashAttack = Animator.StringToHash(_paramAttack);
    }

    void Start()
    {
        _waitingTime = _waitingTimeOrigin;
        _hp = _maxHp;
        _dir = 1;
    }

    protected virtual void Update()
    {
        if (_state == EEState.Attack)
        {

            return;
        }

        if (_state == EEState.Chase)
        {
            _dir = _target.position.x > transform.position.x ? 1 : -1;
            _renderer.flipX = _dir > 0;
            return;
        }

        if (_state == EEState.Patrol)
        {
            _patrolTime -= Time.deltaTime;
            if (_patrolTime <= 0)
            {
                _waitingTime = _waitingTimeOrigin;
                _state = EEState.Idle;
            }
            return;
        }

        if (_state == EEState.Idle)
        {
            _waitingTime -= Time.deltaTime;
            if (_waitingTime <= 0)
            {
                _patrolTime = _patrolTimeOrigin;
                _state = EEState.Patrol;
                _dir = Random.value < 0.5f ? 1 : -1;
                _renderer.flipX = _dir > 0;
            }
            return;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (_state == EEState.Patrol || _state == EEState.Chase)
        {
            _rb.velocity = new Vector2(_dir * _speed, _rb.velocity.y);
            return;
        }
    }

    protected virtual void Attack() { }

    public void Damaged(float damage)
    {
        _hp -= damage;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _rb.velocity = Vector2.zero;
        _state = EEState.Chase;
    }
}
