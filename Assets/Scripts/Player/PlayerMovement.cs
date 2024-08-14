using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : NetworkBehaviour
{
    private NetworkVariable<float> rigWeight = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    [SerializeField] private float speed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform orientation;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private MultiAimConstraint rig;

    private Rigidbody _rb;
    private AnimationController _animationController;
    private bool _grounded;
    private float _xInput;
    private float _yInput;
    private readonly float _animInterpolX = 0f;
    private readonly float _animInterpolY = 0f;

    public bool IsActioning = false;

    void Start()
    {
        if (IsLocalPlayer)
        {
            mainCamera.gameObject.SetActive(true);
        }
        
        rigWeight.OnValueChanged += (oldval, newval) =>
        {
            rig.weight = newval;
        };

        if (!IsOwner) return;
        _animationController = GetComponent<AnimationController>();
        _animationController.Setup(animator, _animInterpolY, _animInterpolX);
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        AimTargetFollow();
        
        if (!IsOwner) return;
        
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,mainCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        
        rigWeight.Value = rig.weight;

        SpeedControl();
        
        Move();
        
        Grounded();
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

        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) speed = 1.5f;

        if (IsActioning && Input.GetKey(KeyCode.E))
        {
            animator.Play("Actioning");
            return;
        }
        else
        {
            animator.Play("New State");
            if (_xInput != 0 || _yInput != 0) 
            { 
                if (_xInput < 0)
                {
                    speed = Mathf.Lerp(speed, 1, Time.deltaTime * 3);
                    _animationController.SneakBackward();
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
                    {
                        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) _yInput = 0;
                        
                        if (Input.GetKey(KeyCode.W))
                        {
                            speed = Mathf.Lerp(speed, 3, Time.deltaTime * 3);
                            _animationController.Run();
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
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
        }

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
