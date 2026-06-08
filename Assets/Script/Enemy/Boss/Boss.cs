using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamagable
{
    public enum EBState
    {
        Idle,
        Chase,
        Active,
        Dead,
    }

    public enum EType
    {
        Slash,
        Rush,
        Magic,
        Ultimate
    }

    [SerializeField] private Transform _target;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private BoxCollider2D _slash;
    [SerializeField] private BoxCollider2D _rush;
    [SerializeField] private BossUltimateEffect _effect;
    [SerializeField] private BossUltimate _ultimate;
    [SerializeField] private EnergyBall _eBall;
    [SerializeField] private Transform _eBallFirePoint;
    [SerializeField] private string _paramCombo = "tCombo";
    [SerializeField] private string _paramMagic = "tMagic";
    [SerializeField] private string _paramPotion = "tPotion";
    [SerializeField] private string _paramAttack = "tAttack";
    [SerializeField] private string _paramESlash = "tESlash";
    [SerializeField] private string _paramDead = "bDead";
    [SerializeField] private string _paramMove = "bMove";
    [SerializeField] private string _paramRush = "bRush";
    [SerializeField] private string _paramGroggy = "bGroggy";
    [SerializeField] private EBState _state;
    [SerializeField] private float _activeCool = 3.0f;
    [SerializeField] private float _comboDist = 1.5f;
    [SerializeField] private float _attack;
    [SerializeField] private float _maxHp;
    [Header("데미지 텍스트 y축 위치")]
    [SerializeField] private Vector3 _offset;

    private int _hashCombo;
    private int _hashMagic;
    private int _hashPotion;
    private int _hashAttack;
    private int _hashESlash;
    private int _hashDead;
    private int _hashMove;
    private int _hashRush;
    private int _hashGroggy;
    private float _hp;
    private float _dir;
    private float _speed = 6.0f;
    private float _rushSpeed = 12.0f;
    private float _coolTime;
    private bool _useUltimate = false;
    private bool _usePotion = false;
    private Vector3 _originSlashPos;
    private Vector3 _originRushPos;
    private Vector3 _originMagicPos;
    private Vector3 _originUltimatePos;

    public float AttackDamage => _attack;
    public float MaxHp => _maxHp;
    public event Action<float> HpChange;

    private void Awake()
    {
        _hashCombo = Animator.StringToHash(_paramCombo);
        _hashMagic = Animator.StringToHash(_paramMagic);
        _hashPotion = Animator.StringToHash(_paramPotion);
        _hashAttack = Animator.StringToHash(_paramAttack);
        _hashESlash = Animator.StringToHash(_paramESlash);
        _hashDead = Animator.StringToHash(_paramDead);
        _hashMove = Animator.StringToHash(_paramMove);
        _hashRush = Animator.StringToHash(_paramRush);
        _hashGroggy = Animator.StringToHash(_paramGroggy);
        _target = Player.Instance.transform;
    }

    private void Start()
    {
        _dir = -1;
        _hp = _maxHp;
        _originSlashPos = _slash.transform.localPosition;
        _originRushPos = _rush.transform.localPosition;
        _originMagicPos = _eBallFirePoint.localPosition;
        _originUltimatePos = _ultimate.transform.localPosition;
        ChangeState(EBState.Idle);
    }

    private void Update()
    {
        if (_state == EBState.Dead)
        {
            return;
        }

        switch (_state)
        {
            case EBState.Idle:
                Idle();
                return;
            case EBState.Chase:
                Chase();
                break;
            case EBState.Active:
                return;
        }

        if (_target == null)
        {
            return;
        }

        if (Vector2.Distance(_target.position, transform.position) <= _comboDist)
        {
            _animator.SetBool(_hashMove, false);
            ChangeState(EBState.Active);
            _animator.SetTrigger(_hashAttack);
        }
    }

    private void FixedUpdate()
    {
        if (_state == EBState.Chase)
        {
            _rb.velocity = new Vector2(_dir * _speed, _rb.velocity.y);
        }
    }

    void Idle()
    {
        _coolTime -= Time.deltaTime;
        if (_coolTime <= 0)
        {
            _coolTime = 0;
            _state = EBState.Active;
            Active();
        }
    }

    void Chase()
    {
        if (_target == null)
        {
            ChangeState(EBState.Idle);
            return;
        }

        _dir = Mathf.Sign(_target.position.x - transform.position.x);
        _renderer.flipX = _dir < 0;
    }

    void Active()
    {
        if (_target == null)
        {
            ChangeState(EBState.Idle);
            return;
        }

        _dir = Mathf.Sign(_target.position.x - transform.position.x);
        _renderer.flipX = _dir < 0;
        
        // 필살기
        if (_hp / _maxHp <= 0.3f && !_useUltimate)
        {
            _animator.SetTrigger(_hashESlash);
            _useUltimate = true;
            return;
        }

        // 콤보공격
        if (Vector2.Distance(_target.position, transform.position) <= _comboDist)
        {
            _animator.SetTrigger(_hashCombo);
            return;
        }

        float attack;
        float rush;
        float magic;
        float rand = UnityEngine.Random.value;

        if (_hp / _maxHp <= 0.6f && !_usePotion)
        {
            attack = 0.45f;
            rush = 0.3f;
            magic = 0.2f;
        }
        else
        {
            attack = 0.5f;
            rush = 0.3f;
            magic = 0.2f;
        }

        // 기본 공격
        if (rand < attack)
        {
            ChangeState(EBState.Chase);
            _animator.SetBool(_hashMove, true);
        }
        // 러쉬
        else if (rand < attack + rush)
        {
            ChangeState(EBState.Active);
            AttackPosChange(EType.Rush);
            _animator.SetBool(_hashRush, true);
            _rush.enabled = true;
            _rb.velocity = new Vector2(_rushSpeed * _dir, _rb.velocity.y);
        }
        // 마법
        else if (rand < attack + rush + magic)
        {
            ChangeState(EBState.Active);
            _animator.SetTrigger(_hashMagic);
        }
        // 포션
        else
        {
            ChangeState(EBState.Active);
            _usePotion = true;
            _animator.SetTrigger(_hashPotion);
        }
    }

    void ChangeState(EBState type)
    {
        _state = type;
        _rb.velocity = Vector2.zero;

        if (_state == EBState.Idle)
        {
            _coolTime = _activeCool;
        }
    }

    public void Damaged(float damage)
    {
        if (_state == EBState.Dead)
        {
            return;
        }

        _hp -= damage;
        DamageTextManager.Instance.ShowDamage(damage, transform.position + _offset);
        HpChange?.Invoke(_hp);

        if (_hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _animator.SetBool(_hashDead, true);
        ChangeState(EBState.Dead);
        _rb.simulated = false;
    }

    void SlashStart()
    {
        AttackPosChange(EType.Slash);
        _slash.enabled = true;
    }

    void SlashEnd()
    {
        _slash.enabled = false;
    }

    void MagicFire()
    {
        AttackPosChange(EType.Magic);
        _eBall.transform.localPosition = _eBallFirePoint.localPosition;
        _eBall.Fire(_target);
    }

    void UltimateCharge()
    {
        _effect.ChargeStart();
    }

    void UltimateSlash()
    {
        AttackPosChange(EType.Ultimate);
        _effect.ChargeEnd();
        _ultimate.Fire(_dir);
    }

    void Groggy()
    {
        StartCoroutine(Co_Groggy());
    }

    public void RushEnd()
    {
        _rush.enabled = false;
        _animator.SetBool(_hashRush, false);
        ActiveEnd();
    }

    void UsePotion()
    {
        _hp += _maxHp / 2;
        HpChange?.Invoke(_hp);
        ActiveEnd();
    }

    void AttackPosChange(EType type)
    {
        Vector3 pos;
        switch (type)
        {
            case EType.Slash:
                pos = _originSlashPos;
                pos.x *= _renderer.flipX ? -1 : 1;
                _slash.transform.localPosition = pos;
                break;
            case EType.Rush:
                pos = _originRushPos;
                pos.x *= _renderer.flipX ? -1 : 1;
                _rush.transform.localPosition = pos;
                break;
            case EType.Magic:
                pos = _originMagicPos;
                pos.x *= _renderer.flipX ? -1 : 1;
                _eBallFirePoint.localPosition = pos;
                break;
            case EType.Ultimate:
                pos = _originUltimatePos;
                pos.x *= _renderer.flipX ? -1 : 1;
                _ultimate.transform.localPosition = pos;
                break;
        }
    }

    void ActiveEnd()
    {
        ChangeState(EBState.Idle);
    }

    void GameClear()
    {
        GameManager.Instance.StopGame(true);
        _clearPanel.SetActive(true);
    }

    IEnumerator Co_Groggy()
    {
        _animator.SetBool(_hashGroggy, true);
        yield return new WaitForSeconds(4.0f);
        _animator.SetBool(_hashGroggy, false);
        ActiveEnd();
    }
}
