using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolf : Skul
{
    [SerializeField] private BoxCollider2D _attackCollider;
    [SerializeField] private BoxCollider2D _jumpAtkCollider;
    [SerializeField] private BoxCollider2D _skillCollider;

    private Vector3 _originAttackPos;

    private void Start()
    {
        _originAttackPos = _attackCollider.transform.localPosition;
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Skill()
    {
        base.Skill();
        AttackPosChange();
    }

    public void AttackStart()
    {
        AttackPosChange();
        _attackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        _attackCollider.enabled = false;
        if (_attackBuffered && _attackCombo < _maxCombo - 1)
        {
            _attackCombo++;
            _animator.SetTrigger(_hashAttack);
            _attackBuffered = false;
        }
        else
        {
            _attackCombo = 0;
            _attackBuffered = false;
        }
    }

    public void SkillStart()
    {
        _skillCollider.enabled = true;
    }

    public void SkillEnd()
    {
        _skillCollider.enabled = false;
    }

    public void JumpAtkStart()
    {
        _jumpAtkCollider.enabled = true;
    }

    public void JumpAtkEnd()
    {
        _jumpAtkCollider.enabled = false;
    }

    void AttackPosChange()
    {
        Vector3 pos = _originAttackPos;
        pos.x *= _renderer.flipX ? -1 : 1;
        _attackCollider.transform.localPosition = pos;
    }
}
