using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public Vector2 centerOfMassOffset = Vector2.zero; // Variable to set the offset of the center of mass in local space

    private Rigidbody2D rb;
    private TargetJoint2D targetJoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetJoint = GetComponent<TargetJoint2D>();
        Cursor.visible = false; // Hide the cursor

        // Set the center of mass offset
        rb.centerOfMass += centerOfMassOffset;
    }

    void Update()
    {
        // Set the target position of the TargetJoint2D component to the mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetJoint.target = mousePosition;
    }
}
