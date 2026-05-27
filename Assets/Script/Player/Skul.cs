using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skul : MonoBehaviour
{

    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected string _paramSpeedY = "aSpeedY";
    [SerializeField] protected string _paramDash = "bDash";
    [SerializeField] protected string _paramDashEnd = "tDashEnd";
    [SerializeField] protected string _paramAttack = "tAttack";
    [SerializeField] protected string _paramWalk = "bWalk";
    [SerializeField] protected string _paramSkill = "tSkill";
    [SerializeField] protected PlayerController _controller;
    [SerializeField] protected int _maxCombo = 2;
    [SerializeField] protected float _skillCoolTime;

    protected int _hashSpeedY;
    protected int _hashDash;
    protected int _hashDashEnd;
    protected int _hashAttack;
    protected int _hashWalk;
    protected int _hashSkill;
    protected int _attackCombo;
    protected bool _attackBuffered;
    protected float _skillCool;
    protected float _changeTime;

    public bool Flip => _renderer.flipX;

    protected virtual void Awake()
    {
        _hashSpeedY = Animator.StringToHash(_paramSpeedY);
        _hashDash = Animator.StringToHash(_paramDash);
        _hashDashEnd = Animator.StringToHash(_paramDashEnd);
        _hashAttack = Animator.StringToHash(_paramAttack);
        _hashWalk = Animator.StringToHash(_paramWalk);
        _hashSkill = Animator.StringToHash(_paramSkill);
        if (_controller == null)
        {
            _controller = GetComponentInParent<PlayerController>();
        }
    }

    protected virtual void Update()
    {
        if (_skillCool > 0)
        {
            _skillCool -= Time.deltaTime;

            if (_skillCool <= 0)
            {
                _skillCool = 0;
                _controller.SkillReady(true);
            }
        }

    }

    protected virtual void OnEnable()
    {
        if (_changeTime > 0)
        {
            ReduceCool(Time.time);
        }
        else if (_skillCool <= 0)
        {
            _controller.SkillReady(true);
        }
    }

    protected virtual void OnDisable()
    {
        _changeTime = Time.time;
    }

    public void YSpeed(float speed)
    {
        _animator.SetFloat(_hashSpeedY, speed);
    }

    public void Walk(bool walk)
    {        
        _animator.SetBool(_hashWalk, walk);
    }

    public void Fliped(bool flip)
    {
        _renderer.flipX = flip;
    }

    public void Dash(bool dash)
    {
        _animator.SetBool(_hashDash, dash);
    }

    public void DashEnd()
    {
        _animator.SetTrigger(_hashDashEnd);
    }

    public virtual void Attack()
    {
        _animator.SetTrigger(_hashAttack);
    }

    public virtual void Combo()
    {
        if (!_attackBuffered)
        {
            _attackBuffered = true;
        }
    }
    public virtual void Skill()
    {
        _skillCool = _skillCoolTime;
        _animator.SetTrigger(_hashSkill);
    }

    // 스컬 변경시 값 유지용 다른거 추가해서 함수이름 바꿀 예정
    public virtual void SkulFlip(bool flip)
    {
        _renderer.flipX = flip;
    }

    public virtual void ChangeSkul() { }

    public void LockEnd()
    {
        if (_attackCombo != 0)
        {
            _attackCombo = 0;
        }
        _controller.LockEnd();
    }

    void ReduceCool(float currentTime)
    {        
        _skillCool -= currentTime - _changeTime;
        if(_skillCool < 0)
        {
            _skillCool = 0;
            _controller.SkillReady(true);
        }
        else
        {
            _controller.SkillReady(false);
        }
    }
}
