using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public Transform pivotPoint; // Reference to the pivot point transform
    private float rotationSpeed = 5f; // Adjust this value to control the rotation speed
    private float rotationOffset = 315f; // Adjust this value to control the rotation offset
    private float followSpeed = 80f;
    public float mouseSensitivity = 1f; // Adjust this value to control the mouse sensitivity

    Rigidbody2D rb;


    void Start()
    {
        Cursor.visible = false; // Hide the cursor
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - rb.position);
        float speed = 10f; // Adjust speed as needed

        rb.velocity = direction.normalized * speed;
        Rotate();
    }

    void Move()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure z position is 0 (since it's a 2D game)

        // Calculate target position
        Vector3 targetPosition = Vector3.Lerp(pivotPoint.position, mousePosition, followSpeed * mouseSensitivity * Time.deltaTime);

        // Calculate acceleration based on the difference between current and target positions
        Vector3 acceleration = (targetPosition - pivotPoint.position) * 1000f;

        // Apply acceleration to current velocity
        pivotPoint.position += acceleration * Time.deltaTime;
    }

    void Rotate()
    {
        // Calculate direction from pivot point to mouse position
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivotPoint.position;

        // Calculate the angle between current direction and the right direction (1,0)
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Add rotation offset
        targetAngle += rotationOffset;

        // Smooth the rotation towards the target angle using adjusted rotation speed
        float currentAngle = Mathf.LerpAngle(pivotPoint.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

        // Apply the smoothed rotation to the pivot point
        pivotPoint.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void LockedToLevel()
    {
        // Get screen bounds in world space
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Clamp pivot point position within screen bounds
        Vector3 clampedPosition = pivotPoint.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
        pivotPoint.position = clampedPosition;
    }
}
