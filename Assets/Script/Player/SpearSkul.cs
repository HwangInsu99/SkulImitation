using UnityEngine;

public class SpearSkul : Skul
{
    [SerializeField] private BoxCollider2D _attackCollider;
    [SerializeField] private BoxCollider2D _jumpAtkCollider;
    [SerializeField] private MeleeAttack _melee;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _skillDamage;
    [SerializeField] private float _skillMovePower;

    private Vector3 _originAttackPos;

    private void Start()
    {
        _originAttackPos = _attackCollider.transform.localPosition;
    }

    public override void Attack()
    {
        base.Attack();
        DamageChagnge(_attackDamage);
    }

    public override void Skill()
    {
        base.Skill();
        AttackPosChange();
        DamageChagnge(_skillDamage);
    }

    public void AttackStart()
    {
        AttackPosChange();
        _attackCollider.enabled = true;
        SoundManager.Instance.PlaySfx(ESfxType.Sting);
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
        float dir = _renderer.flipX ? -1 : 1;
        _controller.Rb.velocity = new Vector2(dir * _skillMovePower, 0f);
        _attackCollider.enabled = true;
        SoundManager.Instance.PlaySfx(ESfxType.Dash);
        SoundManager.Instance.PlaySfx(ESfxType.Sting);
    }

    public void SkillEnd()
    {
        _controller.Rb.velocity = Vector2.zero;
        _attackCollider.enabled = false;
    }

    public void JumpAtkStart()
    {
        _jumpAtkCollider.enabled = true;
        SoundManager.Instance.PlaySfx(ESfxType.Sting);
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

    void DamageChagnge(float mul)
    {
        _melee.DamageMul(mul);
    }
}
