using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform orientation;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Camera mainCamera;

    private Rigidbody _rb;
    private AnimationController _animationController;
    private bool _grounded;
    private float _xInput;
    private float _yInput;
    private readonly float _animInterpolX = 0f;
    private readonly float _animInterpolY = 0f;

    public bool isActioning;

    void Start()
    {
        _animationController = GetComponent<AnimationController>();
        _animationController.Setup(animator, _animInterpolY, _animInterpolX);
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,mainCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        
        SpeedControl();
        
        Move();
        
        Grounded();
        
        AimTargetFollow();
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Move()
    {
        _xInput = Input.GetAxisRaw("Vertical");
        _yInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift)) speed = 1;
        
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) speed = 1.5f;

        if (_xInput < 0) speed = 1;

        if (_xInput != 0 || _yInput != 0)
        {
            if (_xInput < 0)
            {
                speed = Mathf.Lerp(speed, 1, Time.deltaTime * 3);
                _animationController.SneakBackward();
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) _yInput = 0;
                    
                    if (Input.GetKey(KeyCode.W))
                    {
                        speed = Mathf.Lerp(speed, 3, Time.deltaTime * 3);
                        _animationController.Run();
                    }
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        speed = Mathf.Lerp(speed, 1, Time.deltaTime * 3);
                        _animationController.SneakRight();
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        speed = Mathf.Lerp(speed, 1, Time.deltaTime * 3);
                        _animationController.SneakLeft();
                    }
                    else
                    {
                        speed = Mathf.Lerp(speed, 1, Time.deltaTime * 3);
                        _animationController.Sneak();
                    }
                }
                else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        speed = Mathf.Lerp(speed, 2, Time.deltaTime * 3);
                        _animationController.WalkRight();
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        speed = Mathf.Lerp(speed, 2, Time.deltaTime * 3);
                        _animationController.WalkLeft();
                    }
                    else
                    {
                        speed = Mathf.Lerp(speed, 2, Time.deltaTime * 3);
                        _animationController.Walk();
                    }
                }
            }
        }
        else _animationController.Idle();
        
        Vector3 moveDirection = orientation.forward * _xInput + _yInput * orientation.right;
        
        _rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

    private void Grounded()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (_grounded)
            _rb.linearDamping = groundDrag;
        else
            _rb.linearDamping = 0;
    }

    private void AimTargetFollow()
    {
        Ray desiredTargetRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
        Vector3 desiredTargetPos = desiredTargetRay.origin + desiredTargetRay.direction * 0.7f;
        aimTarget.position = desiredTargetPos;
    }
}
