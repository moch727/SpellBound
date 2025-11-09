using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerScript playerScript;

    private PlayerInput playerInput;
    private InputAction movementAction;
    private Vector2 horizontalMovementInput;

    private float speed = 2f;
    private float xVelocity = 0;
    private float zVelocity = 0;
    private float sprintValue;

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        playerInput = GetComponent<PlayerInput>();

        movementAction = playerInput.actions["Move"];
    }

    // Update is called once per frame
    void Update()
    {
        handleInput();


    }

    private void handleInput()
    {
        if (movementAction != null)
        {
            horizontalMovementInput = movementAction.ReadValue<Vector2>();
        }
    }

    public Vector2 getHorizontalMovementInput()
    {
        return horizontalMovementInput;
    }
}
