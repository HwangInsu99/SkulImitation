using UnityEngine;

public class LittleBone : Skul
{
    [SerializeField] private BoxCollider2D _attackCollider;
    private Vector3 _originAttackPos;

    private void Start()
    {
        _originAttackPos = _attackCollider.transform.localPosition;
    }

    public override void Attack()
    {
        base.Attack();
    }

    public void AttackStart()
    {
        Vector3 pos = _originAttackPos;
        pos.x *= _renderer.flipX ? -1 : 1;
        _attackCollider.transform.localPosition = pos;
        _attackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        _attackCollider.enabled = false;
    }
}
