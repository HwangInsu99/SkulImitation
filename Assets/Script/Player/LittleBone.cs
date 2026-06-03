using UnityEngine;

public class LittleBone : Skul
{
    [SerializeField] private BoxCollider2D _attackCollider;
    [SerializeField] private BoxCollider2D _jumpAtkCollider;
    [SerializeField] private Transform _headFire;
    [SerializeField] private LittleBoneHead _head;
    [SerializeField] private string _paramHead = "tGetHead";

    private Vector3 _originAttackPos;
    private Vector3 _originFirePos;
    private int _hashHead;

    protected override void Awake()
    {
        base.Awake();
        _hashHead = Animator.StringToHash(_paramHead);
    }

    private void Start()
    {
        _originAttackPos = _attackCollider.transform.localPosition;
        _originFirePos = _headFire.transform.localPosition;
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Skill()
    {
        base.Skill();
        AttackPosChange();
        _head.transform.SetParent(null);
        _head.transform.position = _headFire.transform.position;
        _head.gameObject.SetActive(true);
        _head.SetHead(_renderer.flipX ? -1 : 1, _skillCoolTime, Player.Instance.Attack);
    }

    public void AttackStart()
    {
        AttackPosChange();
        _attackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        _attackCollider.enabled = false;
        if (_attackBuffered && _attackCombo < _maxCombo-1)
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
        pos = _originFirePos;
        pos.x *= _renderer.flipX ? -1 : 1;
        _headFire.transform .localPosition = pos;
    }

    public override void CollectProjectile()
    {
        _head.transform.SetParent(transform);
        _head.gameObject.SetActive(false);
        _animator.SetTrigger(_hashHead);
        _skillCool = 0;
        _controller.SkillReady(true);
    }

    public override void ChangeSkul()
    {
        if (_skillCool > 0)
        {
            CollectProjectile();
        }
    }
}
