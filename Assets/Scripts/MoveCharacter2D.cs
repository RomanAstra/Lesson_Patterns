using UnityEngine;

public sealed class MoveCharacter2D : MonoBehaviour
{
    private Rigidbody2D _controllerRigidbody;
    private AnimatorMove _animatorMove;

    [SerializeField] private KeyCode _moveRightButton = KeyCode.D;
    [SerializeField] private KeyCode _moveLeftButton = KeyCode.A;
    [SerializeField] private KeyCode _jumpButton = KeyCode.Space;

    [SerializeField] private float _acceleration = 0.0f;
    [SerializeField] private float _maxSpeed = 0.0f;
    [SerializeField] private float _jumpForce = 0.0f;
    
    private Vector2 _movementInput;
    private float _moveHorizontal = 0.0f;

    private bool _jumpInput;
    private bool _isJumping;
    private bool _isFalling;

    private void Start()
    {
        _animatorMove = GetComponent<AnimatorMove>();
        _controllerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(_moveRightButton))
        {
            _moveHorizontal = 1.0f;
            Flip(-1.0f);
        }
        else if (Input.GetKey(_moveLeftButton))
        {
            _moveHorizontal = -1.0f;
            Flip(1.0f);
        }
        else
        {
            _moveHorizontal = 0.0f;
        }

        _movementInput.Set(_moveHorizontal, 0.0f);

        if (!_isJumping && Input.GetKeyDown(_jumpButton))
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
        var velocity = _controllerRigidbody.velocity;

        velocity += _movementInput * (_acceleration * Time.fixedDeltaTime);

        _movementInput = Vector2.zero;

        velocity.x = Mathf.Clamp(velocity.x, -_maxSpeed, _maxSpeed);

        _controllerRigidbody.velocity = velocity;

        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) / _maxSpeed;
        _animatorMove.Move(horizontalSpeedNormalized);

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
