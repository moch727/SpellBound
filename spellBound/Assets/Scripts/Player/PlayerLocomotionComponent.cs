using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject cam;
    private PlayerScript playerScript;
    private Rigidbody rb;

    private PlayerInput playerInput;

    private Vector2 horizontalMovementInput;

    [SerializeField] private float velocity = 2f;
    [SerializeField] private float acceleration = 5f;

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
        rotate();

    }

    private void handleInput()
    {
        horizontalMovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void calculateDisplacement()
    {
        if (horizontalMovementInput.magnitude > 0.1f)
        {
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
    
    private void rotate()
    {
        transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
    }
}
