using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    // 태클 만들어야함
    [SerializeField] private BoxCollider2D _attackCollider;
    [SerializeField] private TankTackle _tackle;
    [SerializeField] private float _tackleMovePower;
    [SerializeField] private string _paramTackle = "tTackle";
    [SerializeField] private float _originTackleCool = 8f;

    private int _hashTackle;
    private Vector3 _originAttackPos;
    private float _tackleCool;

    protected override void Awake()
    {
        base.Awake();
        _hashTackle = Animator.StringToHash(_paramTackle);
    }

    protected override void Start()
    {
        base.Start();
        _originAttackPos = _attackCollider.transform.localPosition;
    }

    protected override void Update()
    {
        if (_tackleCool > 0)
        {
            _tackleCool -= Time.deltaTime;
            if (_tackleCool <= 0)
            {
                _tackleCool = 0;
            }
        }
        base.Update();
        if (_state != EEState.Chase)
        {
            return;
        }

        if (_target == null)
        {
            return;
        }

        if (_coolTime > 0)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, _target.position);

        if (distance <= _attackDist)
        {
            Attack();
        }

        else if (_tackleCool <= 0)
        {
            Tackle();
        }        
    }

    protected override void Attack()
    {
        if (_coolTime > 0)
        {
            return;
        }
        _coolTime = _attackCool;
        ChangeState(EEState.Attack);
        _animator.SetTrigger(_hashAttack);
    }

    private void Tackle()
    {
        if (_coolTime > 0)
        {
            return;
        }
        _tackleCool = _originTackleCool;
        _coolTime = _attackCool;
        ChangeState(EEState.Attack);
        _animator.SetTrigger(_hashTackle);
    }

    protected override void Die()
    {
        base.Die();
        _attackCollider.enabled = false;
        _tackle.gameObject.SetActive(false);
    }

    public void AttackStart()
    {
        Vector3 pos = _originAttackPos;
        pos.x *= _dir;
        _attackCollider.transform.localPosition = pos;
        _attackCollider.enabled = true;
        SoundManager.Instance.PlaySfx(ESfxType.Tank_Attack);
    }

    public void AttackEnd()
    {
        _attackCollider.enabled = false;
        ChangeState(EEState.CoolDown);
    }

    public void TackleStart()
    {
        _rb.velocity = new Vector2(_dir * _tackleMovePower, 0f);
        _tackle.TackleStart(_renderer.flipX);
        SoundManager.Instance.PlaySfx(ESfxType.Tank_Tackle);
        AttackStart();
    }

    public void TackleEnd()
    {
        _rb.velocity = Vector2.zero;
        AttackEnd();
    }
}
