using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public InputActionAsset inputActions;

    public float tileSize = 1f;
    private Vector3 targetPosition; 
    private bool moveTriggered = false; 
    private float moveSpeed = 5f; // Add missing variable declaration and assign a value

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!moveTriggered)
        {
            CheckForInput();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition) 
        {
            moveTriggered = false;
        }
    }

    void CheckForInput()
    {
        if (inputActions != null) 
        {
            float horizontalInput = inputActions.FindActionMap("Player").FindAction("Horizontal").ReadValue<float>();

            // Directly use horizontalInput for movement
            if (horizontalInput != 0) // No need for the absolute value check in this case
            {
                int direction = (int)horizontalInput; // -1 for left, 1 for right
                targetPosition = transform.position + new Vector3(direction * tileSize, -tileSize, 0); 
                moveTriggered = true;
            }
        }
    }

    void OnEnable()
    {
        if (inputActions != null) // Ensure it's not null
        {
            inputActions.Enable();
        }
    }

    void OnDisable()
    {
        if (inputActions != null) // Ensure it's not null
        {
            inputActions.Disable();
        }
    }
}
