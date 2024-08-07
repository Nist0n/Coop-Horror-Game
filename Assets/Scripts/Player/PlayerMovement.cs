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

    private Rigidbody _rb;
    private bool _grounded;
    private float _xInput;
    private float _yInput;
    private bool _isStateComplete;
    private float _animInterpolX = 0f;
    private float _animInterpolY = 0f;

    public bool isActioning;

    private enum PlayerStates
    {
        Idle,
        Running,
        Walking,
        Sneaking,
        Actioning,
        SneakingBackward
    }

    private PlayerStates _state;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        
        SpeedControl();
        
        Move();
        
        Grounded();

        if (_isStateComplete)
        {
            SelectState();
        }
        UpdateState();
        
        Ray desiredTargetRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
        Vector3 desiredTargetPos = desiredTargetRay.origin + desiredTargetRay.direction * 0.7f;
        aimTarget.position = desiredTargetPos;
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

        Vector3 moveDirection = orientation.forward * _xInput + _yInput * orientation.right;

        if (Input.GetKey(KeyCode.LeftShift)) speed = 1;
        
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) speed = 1.5f;

        if (_xInput < 0) speed = 1;

        if (_xInput != 0 || _yInput != 0)
        {
            if (_xInput < 0)
            {
                speed = Mathf.Lerp(speed, 1, Time.deltaTime * 3);
                SneakBackward();
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    speed = Mathf.Lerp(speed, 3, Time.deltaTime * 3);
                    Run();
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = Mathf.Lerp(speed, 1, Time.deltaTime * 3);
                    Sneak();
                }
                else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    speed = Mathf.Lerp(speed, 2, Time.deltaTime * 3);
                    Walk();
                }
            }
        }
        else Idle();
        
        _rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

        Debug.Log($"x = {_xInput}");
        Debug.Log($"y = {_yInput}");
    }

    private void Run()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, -1f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, 1f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    private void Idle()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 0, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, 0, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    private void Walk()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 1, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, 0.5f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    private void Sneak()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, -1f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, -1f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    private void SneakBackward()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 1f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, -0.5f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }

    private void Grounded()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (_grounded)
            _rb.linearDamping = groundDrag;
        else
            _rb.linearDamping = 0;
    }

    private void UpdateState()
    {
        switch (_state)
        {
            case PlayerStates.Idle:
                UpdateIdle();
                break;
            case PlayerStates.Running:
                UpdateRunning();
                break;
            case PlayerStates.Actioning:
                UpdateActioning();
                break;
            case PlayerStates.Walking:
                UpdateWalking();
                break;
            case PlayerStates.Sneaking:
                UpdateSneaking();
                break;
            case PlayerStates.SneakingBackward:
                UpdateSneakingBackward();
                break;
        }
    }

    private void SelectState()
    {
        _isStateComplete = false;

        if (_xInput == 0 && _yInput == 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            if (isActioning)
            {
                _state = PlayerStates.Actioning;
                StartActioning();
            }
            else
            {
                _state = PlayerStates.Idle;
                StartIdle();
            }
        }
        else
        {
            if (_xInput < 0)
            {
                _state = PlayerStates.SneakingBackward;
                StartSneakingBackward();
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    _state = PlayerStates.Running;
                    StartRunning();
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    _state = PlayerStates.Sneaking;
                    StartSneaking();
                }
                else
                {
                    _state = PlayerStates.Walking;
                    StartWalking();
                }
            }
        }
    }

    private void UpdateIdle()
    {
        if (_xInput != 0 || _yInput != 0 || Input.GetKey(KeyCode.LeftShift))
        {
            _isStateComplete = true;
        }
    }
    
    private void UpdateRunning()
    {
        if ((_xInput == 0 && _yInput == 0) || !Input.GetKey(KeyCode.LeftControl) || _xInput < 0)
        {
            _isStateComplete = true;
        }
    }
    
    private void UpdateSneaking()
    {
        if (!Input.GetKey(KeyCode.LeftShift) || _xInput < 0)
        {
            _isStateComplete = true;
        }
    }
    
    private void UpdateSneakingBackward()
    {
        if (_xInput >= 0 || !Input.GetKey(KeyCode.LeftShift))
        {
            _isStateComplete = true;
        }
    }
    
    private void UpdateWalking()
    {
        if ((_xInput == 0 && _yInput == 0) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift) || _xInput < 0)
        {
            _isStateComplete = true;
        }
    }
    
    private void UpdateActioning()
    {
        if ((_xInput == 0 && _yInput == 0) && !isActioning || _xInput < 0)
        {
            _isStateComplete = true;
        }
    }

    private void StartIdle()
    {
        //animator.Play("Idle");
    }
    
    private void StartRunning()
    {
        //animator.Play("Running");
    }
    
    private void StartSneaking()
    {
        //animator.Play("Sneaking");
    }
    
    private void StartWalking()
    {
        //animator.Play("Walking");
    }
    
    private void StartActioning()
    {
        //animator.Play("Actioning");
    }
    
    private void StartSneakingBackward()
    {
        //animator.Play("SneakingBackward");
    }
}
