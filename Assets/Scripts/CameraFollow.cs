using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; 
    public float shiftAmount = 2f;  // How much to move the camera down
    public float smoothSpeed = 0.5f;

    private Camera mainCamera;  
    private Vector3 targetLastFramePosition;
    private float screenTopEdge;
    private float screenBottomEdge;
    private bool cameraShiftTriggered = false; // Add this flag to prevent multiple shifts

    void Start()
    {
        mainCamera = Camera.main;
        CalculateScreenEdges();
        targetLastFramePosition = target.transform.position; // Initialize
    }

    void LateUpdate()
    {
        if (target != null)
        {
            CheckForOffscreenMovement();
            targetLastFramePosition = target.transform.position;
        }

        // Move camera shifting logic here
        if (cameraShiftTriggered) 
        {
            ShiftCameraDown();
            cameraShiftTriggered = false; // Reset the flag
        }
    }

    void CheckForOffscreenMovement()
    {
        // Check if the target moved off the top of the screen
        if (target.transform.position.y > screenTopEdge && targetLastFramePosition.y <= screenTopEdge) 
        {
            ShiftCameraDown();
        }
        // Check if the target moved off the bottom of the screen
        else if (target.transform.position.y < screenBottomEdge && targetLastFramePosition.y >= screenBottomEdge)
        {
            ShiftCameraDown();
        }
    }

    void ShiftCameraDown()
    {
        Vector3 desiredPosition = transform.position + Vector3.down * shiftAmount;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        CalculateScreenEdges(); // Recalculate edges after the camera moves
    }

    void CalculateScreenEdges()
    {
        // Using ViewportToWorldPoint to get screen edges in world coordinates 
        screenTopEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y; 
        screenBottomEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
    }
}