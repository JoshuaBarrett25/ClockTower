using System;
using UnityEngine;
using UnityEngine.InputSystem;


public enum LookDirection
{
    Left = 0,
    Right = 1
}

public class PlayerController : MonoBehaviour
{
    public Player player;

    [SerializeField] private Collider2D _feetCollider2D;
    [SerializeField] private Collider2D _bodyCollider2D;

    private Rigidbody2D _rigid2D;

    private Vector2 _calculatedMoveVelocity;
    private LookDirection _lookDirection = LookDirection.Right;
    
    //Self Respawn
    private float selfRespawnCooldown = 4f;
    private bool _isSelfRespawnOnCooldown = false;
    
    //Status
    public float verticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;
    
    //Jump
    private float _timePastPoint;
    private float _apexPoint;
    private bool _isPastApexThreshold;
    
    //buffer
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;
    
    ///Jump extensions
    //Coyote
    private float _coyoteTimer;

    //Collision
    private bool _isGrounded;
    private bool _hitHead;
    
    

    public void Awake()
    {
        player = GetComponent<Player>();
        _rigid2D = GetComponent<Rigidbody2D>();
        _lookDirection = LookDirection.Right;
    }

    public void Update()
    {
        CountTimers();
        JumpChecks();
    }
    
    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();
        if (_isGrounded)
        {
            Movement(InputManager.moveInput, player.playerData.groundAcceleration, player.playerData.groundDeceleration);   
        }

        if (!_isGrounded)
        {
            Movement(InputManager.moveInput, player.playerData.airAcceleration, player.playerData.airDeceleration);
        }
    }

    public void SelfRespawn(InputAction.CallbackContext context)
    {
        if (context.performed && !_isSelfRespawnOnCooldown)
        {
            player.Respawn();
        }
    }

    private void Movement(Vector2 moveInput, float acceleration, float deceleration)
    {
        if (moveInput != Vector2.zero)
        {
            DirectionUpdate(moveInput);
            Vector2 targetVelocity = new Vector2(moveInput.x, 0) * player.playerData.maxSpeed;
            _calculatedMoveVelocity = Vector2.Lerp(_calculatedMoveVelocity, targetVelocity, acceleration * Time.deltaTime);
            _rigid2D.linearVelocity = new Vector2(_calculatedMoveVelocity.x, _rigid2D.linearVelocity.y);
        }

        else
        {
            _calculatedMoveVelocity = Vector2.Lerp(_calculatedMoveVelocity, Vector2.zero, deceleration * Time.deltaTime);
            _rigid2D.linearVelocity = _calculatedMoveVelocity;
        }
    }

    private void DirectionUpdate(Vector2 moveInput)
    {
        if (_lookDirection == LookDirection.Right && moveInput.x < 0)
        {
            TurnCharacterBody(LookDirection.Left);
        }
        
        else if (_lookDirection == LookDirection.Left && moveInput.x > 0)
        {
            TurnCharacterBody(LookDirection.Right);
        }
    }

    private void TurnCharacterBody(LookDirection newLookDirection)
    {
        _lookDirection = newLookDirection;
        switch (newLookDirection)
        {
            case LookDirection.Left:
                player.sprite.flipX = true;
                break;
            case LookDirection.Right:
                player.sprite.flipX = false;
                break;
        }
    }


    ///<summary>
    /// Jump
    /// </summary>>

    private void JumpChecks()
    {
        if (InputManager.isJumpPressed)
        {
            _jumpBufferTimer = player.playerData.jumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }

        if (InputManager.isJumpReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }
            if (_isJumping && verticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = player.playerData.timeForUpwardsCancel;
                    verticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = verticalVelocity;
                }
            }
        }

        if (_jumpBufferTimer > 0f && !_isJumping && (!_isGrounded || _coyoteTimer > 0f))
        {
            BeginJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = verticalVelocity;
            }
        }
        
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < player.playerData.jumpsAllowed)
        {
            _isFastFalling = false;
            BeginJump(1);
        }
        
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < player.playerData.jumpsAllowed - 1)
        {
            BeginJump(2);
            _isFastFalling = false;
        }

        if ((_isJumping || _isFalling) && _isGrounded && verticalVelocity <= 0f)
        {
            _isJumping = false;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            
            verticalVelocity = Physics2D.gravity.y;
        }
    }

    private void BeginJump(int jumpUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }
        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += jumpUsed;
        verticalVelocity = player.playerData.initialJumpVelocity;
    }

    private void Jump()
    {
        if (_isJumping)
        {
            if (_hitHead)
            {
                _isFastFalling = true;
            }

            if (verticalVelocity >= 0f)
            {
                _apexPoint = Mathf.InverseLerp(player.playerData.initialJumpVelocity, 0f, verticalVelocity);

                if (_apexPoint > player.playerData.apexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastPoint = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastPoint += Time.fixedDeltaTime;
                        if (_timePastPoint < player.playerData.apexHangTime)
                        {
                            verticalVelocity = 0f;
                        }
                        else
                        {
                            verticalVelocity = -0.01f;
                        }
                    }
                }
                else
                {
                    verticalVelocity += player.playerData.gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }
            
            else if (!_isFastFalling)
            {
                verticalVelocity += player.playerData.gravity * player.playerData.gravityOnRelease * Time.fixedDeltaTime;
            }
        
            else if (verticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        if (_isFastFalling)
        {
            if (_fastFallTime >= player.playerData.timeForUpwardsCancel)
            {
                verticalVelocity += player.playerData.gravity * player.playerData.gravityOnRelease * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < player.playerData.timeForUpwardsCancel)
            {
                verticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / player.playerData.timeForUpwardsCancel));
            }
            _fastFallTime += Time.deltaTime;
        }

        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
            
            verticalVelocity += player.playerData.gravity * Time.fixedDeltaTime;
        }

        verticalVelocity = Mathf.Clamp(verticalVelocity, -player.playerData.maxFallSpeed, 50f);

        _rigid2D.linearVelocity = new Vector2(_rigid2D.linearVelocity.x, verticalVelocity);
    }
    
    /// <summary>
    /// Collision Checks
    /// </summary>
    /// <returns></returns>
    ///

    private void CollisionChecks()
    {
        IsGrounded();
        HitHead();
    }
    
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider2D.bounds.center.x, _feetCollider2D.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetCollider2D.bounds.size.x, player.playerData.groundCheckDistance);

        RaycastHit2D groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, player.playerData.groundCheckDistance, player.playerData.groundLayer);
        _isGrounded = groundHit.collider;
    }

    public void HitHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider2D.bounds.center.x, _bodyCollider2D.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetCollider2D.bounds.size.x * player.playerData.headWidth, player.playerData.groundCheckDistance);

        RaycastHit2D headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, player.playerData.headCheckDistance, player.playerData.groundLayer);
        _hitHead = headHit.collider;
    }

    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;

        if (!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else
        {
            _coyoteTimer = player.playerData.coyoteTime;
        }
    }
}