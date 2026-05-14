using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Skul _skul;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private string _groundLayerString;

    private float _moveX;
    private float _jumpPower = 8f;
    private bool _canJump = true;
    private bool _canDash = true;
    private bool _dashBuffered = false;
    private float _originGravity;
    private EState _state;
    private Coroutine _dashCo;
    private WaitForSeconds _dashTime = new WaitForSeconds(0.3f);
    private WaitForSeconds _dashCool = new WaitForSeconds(1.0f);


    private void Awake()
    {
        _groundLayer = LayerMask.GetMask(_groundLayerString);
        if (_player == null)
        {
            _player = GetComponent<Player>();
        }
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
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
            _skul.Fliped(_moveX < 0);
        }

        _skul.YSpeed(_rb.velocity.y);
        if ((_state & EState.Air) != 0)
        {
            _skul.XSpeed(0);
            return;
        }
        _skul.XSpeed(Mathf.Abs(_rb.velocity.x));
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
        _skul.Dash(true);
        _skul.XSpeed(0);
        _skul.YSpeed(0);
        _rb.gravityScale = 0f;
        float dir = _skul.Flip ? -1 : 1;
        _rb.velocity = new Vector2(dir * _player.DashSpeed, 0f);
        yield return _dashTime;
        if (_dashBuffered)
        {
            _dashBuffered = false;
            _dashCo = StartCoroutine(Co_Dash());

            yield break;
        }
        _skul.Dash(false);
        _skul.DashEnd();
        _canDash = false;
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

    public void SetSkul(Skul target)
    {
        _skul = target;
    }
}