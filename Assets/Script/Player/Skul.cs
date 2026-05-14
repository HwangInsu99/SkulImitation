using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skul : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _paramSpeedX = "aSpeedX";
    [SerializeField] private string _paramSpeedY = "aSpeedY";
    [SerializeField] private string _paramDash = "bDash";
    [SerializeField] private string _paramDashEnd = "tDashEnd";
    [SerializeField] private string _paramAttack = "tAttack";

    private int _hashSpeedX;
    private int _hashSpeedY;
    private int _hashDash;
    private int _hashDashEnd;
    private int _hashAttack;

    public bool Flip => _renderer.flipX;

    private void Awake()
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
