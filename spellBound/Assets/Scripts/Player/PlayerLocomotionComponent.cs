using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionComponent : MonoBehaviour
{
    private PlayerScript playerScript;
    private Rigidbody rb;

    private PlayerInput playerInput;

    public Vector2 horizontalMovementInput;

    [SerializeField] private float velocity = 4f;
    [SerializeField] private float acceleration = 7f;

    [SerializeField] float maxSlopeAngle;
    private RaycastHit hit;

    public bool camRotation = true;

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        playerInput = GetComponent<PlayerInput>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (camRotation) //camRotation
        {
            faceForward();
            //rotate();
        }

        //faceForward();
        if (playerScript.currentAction == PlayerScript.Action.None) //!playerScript.inAction
        {
            handleInput();
            calculateDisplacement();
            if (OnSlope())
            {
                rb.linearVelocity = getSlopeDirection();
            }
            rb.useGravity = !OnSlope();
        }

        //faceForward();
        //Debug.Log(rb.linearVelocity);

    }

    private void handleInput()
    {
        horizontalMovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void calculateDisplacement()
    {
        if (horizontalMovementInput.magnitude > 0.1f) // && !playerScript.inAction
        {
            faceForward();
            Vector3 targetVelocityX = transform.right * horizontalMovementInput.x * velocity;
            Vector3 targetVelocityY = transform.forward * horizontalMovementInput.y * velocity;

            Vector3 total = targetVelocityX + targetVelocityY;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, total, acceleration * Time.deltaTime);
        }
        else
        {
            Vector3 startingVelocity = rb.linearVelocity;
            rb.linearVelocity = Vector3.Lerp(startingVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }
    }
    
    public void faceForward()
    {
        transform.eulerAngles = new Vector3(0, playerScript.cam.transform.eulerAngles.y, 0);
    }

    public void Dodge()
    {
        float magnitude = 4f;
        if (OnSlope()) rb.useGravity = true;
        rb.AddForce(rb.linearVelocity.normalized * magnitude, ForceMode.Impulse);
    }
    public void rotate()
    {
        if(playerScript.currentAction != PlayerScript.Action.Interact)
        {
            transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"));
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.World); //space world prevents rotation in z axis
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 2f))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            return angle != 0 && angle <= maxSlopeAngle;
        }
        return false;
    }

    private Vector3 getSlopeDirection()
    {
        return Vector3.ProjectOnPlane(rb.linearVelocity, hit.normal);
    }
}
