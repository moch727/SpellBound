using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerScript playerScript;
    private Rigidbody rb;

    private PlayerInput playerInput;

    private Vector2 horizontalMovementInput;

    [SerializeField] private float velocity = 2f;
    [SerializeField] private float acceleration = 5f;

    [SerializeField] private const float SHAKERATE = 0.5f;
    [SerializeField] private const float SHAKEMAGNITUDE = 10f;
    private float shakeRate = SHAKERATE;
    [SerializeField] private float shakeMagnitude = 10f;
    private bool touchingGround = true;

    //private float sprintValue;

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        playerInput = GetComponent<PlayerInput>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        handleInput();

        calculateDisplacement();

    }

    private void handleInput()
    {
        horizontalMovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void calculateDisplacement()
    {
        if (horizontalMovementInput.magnitude > 0.1f)
        {

            if (touchingGround) //screen shake effect
            {
                shakeRate -= Time.fixedDeltaTime;
                if (shakeRate < 0)
                {
                    shakeMagnitude = SHAKEMAGNITUDE;
                    shakeRate = SHAKERATE;
                }
            }
            else
            {
                shakeRate = SHAKERATE;
                shakeMagnitude = 0;
            }

            Vector3 targetVelocity = new Vector3(horizontalMovementInput.x * velocity, shakeMagnitude, horizontalMovementInput.y * velocity);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            shakeRate = SHAKERATE;
            Vector3 startingVelocity = rb.linearVelocity;
            rb.linearVelocity = Vector3.Lerp(startingVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }
    }
    public Vector2 getHorizontalMovementInput()
    {
        return horizontalMovementInput;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            touchingGround = false;
        }
    }
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            touchingGround = true;
        }
    }
}
