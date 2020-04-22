using UnityEngine;

public sealed class MoveCharacter2D : MonoBehaviour
{
    private Rigidbody2D _controllerRigidbody;
    private AnimatorMove _animatorMove;

    [SerializeField] private float _acceleration = 0.0f;
    [SerializeField] private float _maxSpeed = 0.0f;
    [SerializeField] private float _jumpForce = 0.0f;

    private Inputs _inputs;
    
    private Vector2 _movementInput;
    private float _moveHorizontal = 0.0f;
    private float _flipLeft = 1.0f;
    private float _flipRight = -1.0f;

    private Vector2 _velocity;
    private float _horizontalSpeedNormalized;

    private bool _jumpInput;
    private bool _isJumping;
    private bool _isFalling;

    private void Start()
    {
        _animatorMove = GetComponent<AnimatorMove>();
        _controllerRigidbody = GetComponent<Rigidbody2D>();
        _inputs = GetComponent<Inputs>();
    }

    private void Update()
    {
        _moveHorizontal = 0.0f;

        if (Input.GetKey(_inputs.MoveRightButton))
        {
            _moveHorizontal = 1.0f;
            Flip(_flipRight);
        }
        else if (Input.GetKey(_inputs.MoveLeftButton))
        {
            _moveHorizontal = -1.0f;
            Flip(_flipLeft);
        }

        _movementInput.Set(_moveHorizontal, 0.0f);

        if (!_isJumping && Input.GetKeyDown(_inputs.JumpButton))
        {
            _jumpInput = true;
        }
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
        UpdateJump();
    }

    private void UpdateVelocity()
    {
        _velocity = _controllerRigidbody.velocity;

        _velocity += _movementInput * (_acceleration * Time.fixedDeltaTime);

        _movementInput = Vector2.zero;

        _velocity.x = Mathf.Clamp(_velocity.x, -_maxSpeed, _maxSpeed);

        _controllerRigidbody.velocity = _velocity;

        _horizontalSpeedNormalized = Mathf.Abs(_velocity.x) / _maxSpeed;

        _animatorMove.Move(_horizontalSpeedNormalized);

        // Play audio
    }

    private void Flip(float value)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = value;
        transform.localScale = localScale;
    }

    private void UpdateJump()
    {
        if (_isJumping && _controllerRigidbody.velocity.y < 0)
        {
            _isFalling = true;
        }

        if (_jumpInput)
        {
            _controllerRigidbody.AddForce(new Vector2(0.0f, _jumpForce), ForceMode2D.Impulse);

            _animatorMove.Jump();

            _jumpInput = false;

            _isJumping = true;

            // Play audio
        }
        else if (_isJumping && _isFalling)
        {
            _isJumping = false;
            _isFalling = false;

            // Play audio
        }
    }
}
