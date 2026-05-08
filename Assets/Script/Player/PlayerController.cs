using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    [System.Flags]
    public enum EState
    {
        None = 0,
        Dash = 1 << 0,
        Attack = 1 << 1,
        Air = 1 << 2,
    }

    [SerializeField] private Player _player;
    [SerializeField] private SpriteRenderer _playerRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private string _paramSpeedX = "aSpeedX";
    [SerializeField] private string _paramSpeedY = "aSpeedY";
    [SerializeField] private string _paramDash = "bDash";
    [SerializeField] private string _paramAttack = "tAttack";
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private string _groundLayerString;

    private int _hashSpeedX;
    private int _hashSpeedY;
    private int _hashDash;
    private int _hashAttack;
    private float _moveX;
    private float _moveY;
    private float _jumpPower = 8f;
    private bool _canJump = true;
    private bool _canDash = true;
    private bool _dashBuffered = false;
    private float _originGravity;
    private EState _state;
    private Coroutine _dashCo;
    private WaitForSeconds _dashTime = new WaitForSeconds(0.2f);
    private WaitForSeconds _dashCool = new WaitForSeconds(1.0f);

    private void Awake()
    {
        _hashSpeedX = Animator.StringToHash(_paramSpeedX);
        _hashSpeedY = Animator.StringToHash(_paramSpeedY);
        _hashDash = Animator.StringToHash(_paramDash);
        _hashAttack = Animator.StringToHash(_paramAttack);
        _groundLayer = LayerMask.GetMask(_groundLayerString);
    }

    private void Start()
    {
        _originGravity = _rb.gravityScale;
    }

    void Update()
    {
        GroundCheck();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        if ((_state & EState.Attack) != 0 || (_state & EState.Dash) != 0)
        {
            return;
        }
        _moveX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (_moveX != 0)
        {
            _playerRenderer.flipX = _moveX < 0;
        }

        _animator.SetFloat(_hashSpeedY, _rb.velocity.y);
        if ((_state & EState.Air) != 0)
        {
            _animator.SetFloat(_hashSpeedX, 0);
            return;
        }
        _animator.SetFloat(_hashSpeedX, Mathf.Abs(_rb.velocity.x));
    }

    private void FixedUpdate()
    {
        if ((_state & EState.Attack) != 0 || (_state & EState.Dash) != 0)
        {
            return;
        }
        PlayerMove();
    }

    void PlayerMove()
    {
        Vector2 velocity = _rb.velocity;
        velocity.x = _moveX * _player.Speed;

        _rb.velocity = velocity;
    }

    void Jump()
    {
        if (!_canJump)
        {
            return;
        }
        if ((_state & EState.Air) != 0)
        {
            _canJump = false;
            Vector2 velocity = _rb.velocity;
            velocity.y = 0;
            _rb.velocity = velocity;
        }
        _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
    }

    void Dash()
    {
        if (!_canDash)
        {
            return;
        }

        if (_dashCo != null)
        {
            _dashBuffered = true;
            _canDash = false;
            return;
        }
        _dashCo = StartCoroutine(Co_Dash());
    }

    IEnumerator Co_Dash()
    {
        _state |= EState.Dash;
        _animator.SetBool(_hashDash, true);
        _animator.SetFloat(_hashSpeedX, 0);
        _animator.SetFloat(_hashSpeedY, 0);
        _rb.gravityScale = 0f;
        float dir = _playerRenderer.flipX ? -1 : 1;
        _rb.velocity = new Vector2(dir * _player.DashSpeed, 0f);
        yield return _dashTime;
        if (_dashBuffered)
        {
            _dashBuffered = false;
            _dashCo = StartCoroutine(Co_Dash());

            yield break;
        }
        _canDash = false;
        _animator.SetBool(_hashDash, false);
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = _originGravity;
        _state &= ~EState.Dash;
        yield return _dashCool;
        _canDash = true;
        _dashCo = null;
    }

    void GroundCheck()
    {
        if (IsGrounded())
        {
            _state &= ~EState.Air;
            _canJump = true;
        }
        else
        {
            _state |= EState.Air;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f, _groundLayer);
    }
}