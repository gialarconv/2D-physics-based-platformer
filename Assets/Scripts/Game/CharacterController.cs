using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterController : MonoBehaviour
{

    [Header("Character")]
    [SerializeField] private Transform _model;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private ParticleSystem _dustParticle;
    [SerializeField] private ParticleSystem _jumpParticle;

    [Header("Physics Detection")]
    [SerializeField] private Vector2 _groundCheckPosition;
    [SerializeField] private Vector2 _groundCheckRadius;
    [SerializeField] private Vector2 _wallCheckRadius;
    [SerializeField] private Vector2 _lefttOffset;
    [SerializeField] private Vector2 _rightOffset;
    [SerializeField] private LayerMask _groundMask;

    [Header("Wall Grab")]
    [SerializeField] private float _slideSpeed = 2.5f;

    private InputManager _inputManager;
    private Rigidbody2D _rigidBody;
    private bool _isGrounded;
    private float _movementDirection;
    private float _dirX = 1f;
    private bool _isFacingRight = true;
    private bool _hitAnyWall = false;
    private bool _hitLeftWall = false;
    private bool _hitRightWall = false;

    private bool _stopMovement;
    private bool _startMovement;

    private void OnEnable()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogWarning("InputManager doesn't found!");
            return;
        }
        InputManager.Instance.OnStartTouch += Jump;
    }
    private void OnDisable()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogWarning("InputManager is not created!");
            return;
        }
        InputManager.Instance.OnStartTouch -= Jump;
    }
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movementDirection = _dirX;

        this.DoAfterFrames(3, () => _startMovement = true);
    }
    private void FixedUpdate()
    {
        if (!_startMovement)
            return;

        //check ground detection with Physics
        CheckGround();

        //auto movement
        AutoMovement();

        //check if the player is stick in the wall and change the slide speed
        WallStick();
        //check wich wall the player touched with Physics
        CheckWalls();

        // enable/disable dust particles
        DustParticleBehaviour();
    }
    private void LateUpdate()
    {
        FlipCharacterModel();
    }
    private void AutoMovement()
    {
        if (!_stopMovement)
        {
            _rigidBody.velocity = new Vector2(_movementDirection * _moveSpeed, _rigidBody.velocity.y);
        }
    }
    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapBox((Vector2)transform.position + _groundCheckPosition, _groundCheckRadius, 0f, _groundMask);
    }
    private void Jump()
    {
        //only jump
        if (_isGrounded && !_hitAnyWall)
        {
            _rigidBody.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            _jumpParticle.Play();
            _isGrounded = false;
        }
        //jump and change direction
        else if (!_isGrounded && _hitAnyWall || _isGrounded && _hitAnyWall)
        {
            _rigidBody.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            _isGrounded = false;

            _movementDirection *= -_dirX;
            _stopMovement = false;
            if (_isGrounded)
            {
                _jumpParticle.Play();
            }
        }
    }
    private void CheckWalls()
    {
        var position = transform.position;

        _hitAnyWall = Physics2D.OverlapBox((Vector2)position + _lefttOffset, _wallCheckRadius, 0f, _groundMask)
                || Physics2D.OverlapBox((Vector2)position + _rightOffset, _wallCheckRadius, 0f, _groundMask);

        _hitLeftWall = Physics2D.OverlapBox((Vector2)position + _lefttOffset, _wallCheckRadius, 0f, _groundMask);
        _hitRightWall = Physics2D.OverlapBox((Vector2)position + _rightOffset, _wallCheckRadius, 0f, _groundMask);

        _stopMovement = _hitAnyWall ? true : false;
    }

    private void WallStick()
    {
        if (_hitAnyWall && _rigidBody.velocity.y <= 0f && _stopMovement)
        {
            _rigidBody.velocity = new Vector2(_movementDirection * _moveSpeed, -_slideSpeed);
        }
    }

    #region Model and FX
    private void FlipCharacterModel()
    {
        if (_isFacingRight && _movementDirection < 0f || !_isFacingRight && _movementDirection > 0f)
        {
            _isFacingRight = !_isFacingRight;

            Vector3 scale = _model.transform.localScale;
            scale.x *= -_dirX;
            _model.transform.localScale = scale;
        }
    }
    private void DustParticleBehaviour()
    {
        float playerVelocityMag = _rigidBody.velocity.sqrMagnitude;
        if (_dustParticle.isPlaying && playerVelocityMag == 0f)
            _dustParticle.Stop();
        else if (!_dustParticle.isPlaying && playerVelocityMag > 0f)
            _dustParticle.Play();
    }
    #endregion

    #region GUI 
    private void OnDrawGizmos()
    {
        var position = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawCube((Vector2)position + _groundCheckPosition, _groundCheckRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube((Vector2)position + _lefttOffset, _wallCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawCube((Vector2)position + _rightOffset, _wallCheckRadius);
    }
    #endregion
}