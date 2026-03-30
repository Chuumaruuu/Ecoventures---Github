using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPanning : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float swipeSensitivity = 0.15f;
    [SerializeField] private float maxRotation = 60f;

    [Header("Optional (Editor Testing)")]
    [SerializeField] private bool enableMouseInput = true;

    private float currentYRotation = 0f;

    private Vector2 lastPosition;
    private bool isDragging = false;

    void Update()
    {
        HandleTouchInput();
        HandleMouseInput(); // for testing in editor
    }

    void HandleTouchInput()
    {
        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        // Touch start
        if (touch.press.wasPressedThisFrame)
        {
            lastPosition = touch.position.ReadValue();
            isDragging = true;
        }

        // Touch move
        if (isDragging && touch.press.isPressed)
        {
            Vector2 currentPosition = touch.position.ReadValue();
            float deltaX = currentPosition.x - lastPosition.x;

            RotateCamera(deltaX);

            lastPosition = currentPosition;
        }

        // Touch end
        if (touch.press.wasReleasedThisFrame)
        {
            isDragging = false;
        }
    }

    void HandleMouseInput()
    {
        if (!enableMouseInput)
            return;

        if (Mouse.current == null)
            return;

        // Mouse down
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            lastPosition = Mouse.current.position.ReadValue();
            isDragging = true;
        }

        // Mouse drag
        if (isDragging && Mouse.current.leftButton.isPressed)
        {
            Vector2 currentPosition = Mouse.current.position.ReadValue();
            float deltaX = currentPosition.x - lastPosition.x;

            RotateCamera(deltaX);

            lastPosition = currentPosition;
        }

        // Mouse up
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }
    }

    void RotateCamera(float deltaX)
    {
        // Apply sensitivity (NO deltaTime!)
        float rotationAmount = deltaX * swipeSensitivity;

        currentYRotation += rotationAmount;

        // Clamp rotation to prevent full spin
        currentYRotation = Mathf.Clamp(currentYRotation, -maxRotation, maxRotation);

        // Apply rotation locally (important!)
        transform.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
    }
}