using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skul : MonoBehaviour
{

    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected string _paramSpeedX = "aSpeedX";
    [SerializeField] protected string _paramSpeedY = "aSpeedY";
    [SerializeField] protected string _paramDash = "bDash";
    [SerializeField] protected string _paramDashEnd = "tDashEnd";
    [SerializeField] protected string _paramAttack = "tAttack";

    protected int _hashSpeedX;
    protected int _hashSpeedY;
    protected int _hashDash;
    protected int _hashDashEnd;
    protected int _hashAttack;

    public bool Flip => _renderer.flipX;

    protected void Awake()
    {
        _hashSpeedX = Animator.StringToHash(_paramSpeedX);
        _hashSpeedY = Animator.StringToHash(_paramSpeedY);
        _hashDash = Animator.StringToHash(_paramDash);
        _hashDashEnd = Animator.StringToHash(_paramDashEnd);
        _hashAttack = Animator.StringToHash(_paramAttack);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void YSpeed(float speed)
    {
        _animator.SetFloat(_hashSpeedY, speed);
    }

    public void XSpeed(float speed)
    {      
        _animator.SetFloat(_hashSpeedX, speed);
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

    public void Attack()
    {
        _animator.SetTrigger(_hashAttack);
    }

    // 스컬 변경시 값 유지용 다른거 추가해서 함수이름 바꿀 예정
    public void SkulChange(bool flip)
    {
        _renderer.flipX = flip;
    }
}
