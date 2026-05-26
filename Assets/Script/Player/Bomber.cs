using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Skul
{
    [SerializeField] private BoxCollider2D _skillCollider;
    [SerializeField] private float _bombTimerMax = 10;
    private float _bombTimer;

    protected override void OnEnable()
    {
        base.OnEnable();
        _bombTimer = _bombTimerMax;
    }

    protected override void Update()
    {
        base.Update();
        _bombTimer -= Time.deltaTime;
        if (_bombTimer <= 0)
        {
            _bombTimer = _bombTimerMax;
            _controller.SkillReady(true);
            _controller.Skill();
        }
    }

    public override void Attack()
    {
        LockEnd();
    }

    public override void Skill()
    {
        base.Skill();
    }

    public void SkillStart()
    {
        _skillCollider.enabled = true;
    }

    public void SkillEnd()
    {
        _skillCollider.enabled = false;
    }

    public void ExplodeEnd()
    {
        LockEnd();
        Player.Instance.SkulChange(0);
    }
}
