using System.Collections;
using UnityEngine;

public class Archer : Enemy
{
    // 화살 날려야함
    [SerializeField] private GameObject _arrow;

    protected override void Start()
    {
        base.Start();
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
        StartCoroutine(Co_Attack());
    }

    IEnumerator Co_Attack()
    {
        yield return null;
        float animTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animTime);
        ChangeState(EEState.CoolDown);
    }

    protected override void Die()
    {
        base.Die();
        StopAllCoroutines();
    }
}
