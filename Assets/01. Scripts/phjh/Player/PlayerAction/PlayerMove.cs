using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    InputReader input;

    Rigidbody rb;

    float _currentSpeed = 4f;

    Vector3 _inputDirection;
    Vector3 _movementVelocity;

    public bool CanMove { get; set; } = true;

    [SerializeField]
    Animator animator;

    private readonly int animationDirx = Animator.StringToHash("dirx");

    private void OnEnable()
    {
        input.MovementEvent += GetMoveDirection;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetMoveDirection(Vector2 dir)
    {
        _inputDirection.x = dir.x;
        _inputDirection.z = dir.y;
        //애니메이션 처리
        animator.SetFloat(animationDirx, dir.x);
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = _inputDirection * _currentSpeed;
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }


    private void Move()
    {
        rb.velocity = _movementVelocity;
    }

    private void FixedUpdate()
    {

        if (CanMove)
            CalculatePlayerMovement();
        else
            StopImmediately();
        Move();
    }


}
