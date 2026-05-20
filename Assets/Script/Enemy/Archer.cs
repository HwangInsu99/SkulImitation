using System.Collections;
using UnityEngine;

public class Archer : Enemy
{
    [SerializeField] private EnemyProjectile _arrow;
    [SerializeField] private Transform _firePoint;
    private Vector3 _originFirePos;

    protected override void Start()
    {
        base.Start();
        _originFirePos = _firePoint.localPosition;
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
    }

    public void AttackEnd()
    {
        Vector3 pos = _originFirePos;
        pos.x *= _dir;
        _firePoint.localPosition = pos;
        EnemyProjectile arrow = Instantiate<EnemyProjectile>(_arrow, _firePoint.position, Quaternion.identity);
        arrow.SetProjectile(_attack, _dir);
        ChangeState(EEState.CoolDown);
    }
}
