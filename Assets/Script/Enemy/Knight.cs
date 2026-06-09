using System.Collections;
using UnityEngine;

public class Knight : Enemy
{
    [SerializeField] private BoxCollider2D _attackCollider;
    private Vector3 _originAttackPos;

    protected override void Start()
    {
        base.Start();
        _originAttackPos = _attackCollider.transform.localPosition;
    }

    protected override void Update()
    {
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

    protected override void Die()
    {
        base.Die();
        _attackCollider.enabled = false;
    }

    public void AttackStart()
    {
        Vector3 pos = _originAttackPos;
        pos.x *= _dir;
        _attackCollider.transform.localPosition = pos;
        _attackCollider.enabled = true;
        SoundManager.Instance.PlaySfx(ESfxType.Knight_Attack);
    }

    public void AttackEnd()
    {
        _attackCollider.enabled = false;
        ChangeState(EEState.CoolDown);
    }
}
