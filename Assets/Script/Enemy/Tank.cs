using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    // 태클 만들어야함
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
        StartCoroutine(Co_Attack());
    }

    IEnumerator Co_Attack()
    {
        yield return null;
        float animTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        float prepTime = 0.2f;
        yield return new WaitForSeconds(prepTime);
        Vector3 pos = _originAttackPos;
        pos.x *= _dir;
        _attackCollider.transform.localPosition = pos;
        _attackCollider.enabled = true;
        yield return new WaitForSeconds(Mathf.Max(0, animTime - prepTime));
        _attackCollider.enabled = false;
        ChangeState(EEState.CoolDown);
    }

    protected override void Die()
    {
        base.Die();
        StopAllCoroutines();
        _attackCollider.enabled = false;
    }
}
