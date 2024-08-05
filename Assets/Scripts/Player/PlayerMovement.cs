using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform orientation;

    private Rigidbody _rb;
    private bool _grounded;

    public enum PlayerStates
    {
        Idle,
        Running,
        Walking,
        Sneaking,
        Actioning
    }

    private PlayerStates _state;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        SpeedControl();
        float xInput = Input.GetAxisRaw("Vertical");
        float yInput = Input.GetAxisRaw("Horizontal");
        
        Vector3 moveDirection = orientation.forward * xInput + yInput * orientation.right;
        
        _rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

        _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (_grounded)
            _rb.linearDamping = groundDrag;
        else
            _rb.linearDamping = 0;
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
}
