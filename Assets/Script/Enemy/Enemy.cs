using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    public enum EEState
    {
        Idle,
        Chase,
        Patrol,
        Attack,
        CoolDown,
        Dead,
    }

    [SerializeField] protected Transform _target;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected CircleCollider2D _detectArea;
    [SerializeField] protected string _paramSpeedX = "aSpeedX";
    [SerializeField] protected string _paramAttack = "tAttack";
    [SerializeField] protected string _paramDie = "bDie";
    [SerializeField] protected EEState _state;
    [SerializeField] protected float _attackCool;
    [SerializeField] protected float _attackDist = 1.5f;
    [SerializeField] protected float _attack;

    protected int _hashSpeedX;
    protected int _hashAttack;
    protected int _hashDie;
    protected float _dir;
    protected float _speed = 3.0f;
    protected float _hp;
    protected float _maxHp;
    protected float _patrolTimeOrigin = 2.0f;
    protected float _patrolTime;
    protected float _waitingTimeOrigin = 5.0f;
    protected float _waitingTime;
    protected float _missDist;
    protected float _remainTime = 1.0f;
    protected float _coolTime;

    public Transform Target => _target;
    public float AttackDamage => _attack;

    private void Awake()
    {
        _hashSpeedX = Animator.StringToHash(_paramSpeedX);
        _hashAttack = Animator.StringToHash(_paramAttack);
        _hashDie = Animator.StringToHash(_paramDie);
    }

    protected virtual void Start()
    {
        _hp = _maxHp;
        _dir = 1;
        _missDist = _detectArea.radius * transform.lossyScale.x;
        ChangeState(EEState.Idle);
    }

    protected virtual void Update()
    {
        if (_state == EEState.Dead)
        {
            _remainTime -= Time.deltaTime;
            if (_remainTime <= 0)
            {
                Disappear();
            }
            return;
        }

        _animator.SetFloat(_hashSpeedX, Mathf.Abs(_rb.velocity.x));

        switch (_state)
        {
            case EEState.Idle:
                Idle();
                break;
            case EEState.Patrol:
                Patrol();
                break;
            case EEState.Chase:
                Chase();
                break;
            case EEState.Attack: // 공격도중 업데이트 진행 안하기 위함
                return;
            case EEState.CoolDown:
                CoolDown();
                return;
        }

        if (_target != null && Vector2.Distance(transform.position, _target.position) > _missDist * 1.5f)
        {
            _target = null;
            ChangeState(EEState.Idle);
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

    protected virtual void Chase()
    {
        _dir = Mathf.Sign(_target.position.x - transform.position.x);
        _renderer.flipX = _dir < 0;
    }

    protected virtual void Patrol()
    {
        _patrolTime -= Time.deltaTime;
        if (_patrolTime <= 0)
        {
            ChangeState(EEState.Idle);
        }
    }

    protected virtual void CoolDown()
    {
        _coolTime -= Time.deltaTime;
        if (_coolTime <= 0)
        {
            if (_target != null)
            {
                ChangeState(EEState.Chase);
            }
            else
            {
                ChangeState(EEState.Idle);
            }
        }
    }

    protected virtual void Idle()
    {
        _waitingTime -= Time.deltaTime;
        if (_waitingTime <= 0)
        {
            ChangeState(EEState.Patrol);
            _dir = Random.value < 0.5f ? 1 : -1;
            _renderer.flipX = _dir < 0;
        }
    }

    protected void ChangeState(EEState state)
    {
        _state = state;
        _rb.velocity = Vector2.zero;

        switch (_state)
        {
            case EEState.Idle:
                _waitingTime = _waitingTimeOrigin;
                break;
            case EEState.Patrol:
                _patrolTime = _patrolTimeOrigin;
                break;
        }
    }

    public void Damaged(float damage)
    {
        if (_state == EEState.Dead)
        {
            return;
        }
        _hp -= damage;
        if (_hp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        _animator.SetBool(_hashDie, true);
        ChangeState(EEState.Dead);
        _rb.velocity = Vector2.zero;
        _rb.simulated = false;
        _detectArea.enabled = false;
        _target = null;
    }

    protected void Disappear()
    {
        Destroy(gameObject);
    }

    public void SetTarget(Transform target)
    {
        if (_state == EEState.Dead)
        {
            return;
        }
        _target = target;
        ChangeState(EEState.Chase);
    }
}
