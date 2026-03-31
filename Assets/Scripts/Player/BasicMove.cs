using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMove : MonoBehaviour
{
    const byte MAX_SPEED = 128; // waow
    [SerializeField] public float skinWidth = 0.02f;
    [SerializeField] public float moveSpeed = 7f;
    [SerializeField] public float accel = 2f;
    [SerializeField] public float friction = 0.25f;
    [NonSerialized] public BoxCollider col;
    public GameObject model;
    private Vector2 moveInput;
    [NonSerialized] public Vector3 wishDir;
    [NonSerialized] public Vector3 velocity;
    private Vector3 wishVel;
    private int planeCount = 0;
    MoveHands moveHands;
    void Start()
    {
        col = GetComponent<BoxCollider>();
        moveHands = GetComponentInChildren<MoveHands>();
    }

    void Update()
    {
        // get move direction
        wishDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        Accelerate();
        ApplyFriction();

        velocity = MoveWithCollisions(velocity);

        Quaternion targetRotation = Quaternion.LookRotation(moveHands.direction);
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.deltaTime * 10f);

        //Debug.Log($"velocity: {velocity.magnitude}");
    }

    private void Accelerate()
    {
        wishVel = wishDir * MAX_SPEED;
        Vector3 accelDir = wishVel - velocity;
        accelDir.Normalize();

        if (wishDir.magnitude > 0)
        {
            velocity += accelDir * accel;
        }
    }

    private void ApplyFriction()
    {
        float mag = velocity.magnitude;
        float diff = mag - moveSpeed;
        if (diff > 0 || wishDir.magnitude <= 0)
        {
            if (accel >= diff && wishDir.magnitude > 0)
            {
                velocity.Normalize();
                velocity *= moveSpeed;
            }
            else
            {
                float deccel = wishDir.magnitude > 0 ? accel : 0;
                velocity.Normalize();
                float newMag = mag - (friction + deccel);
                if (newMag < 0) newMag = 0;
                velocity *= newMag;
            }
        }
    }

    Vector3 MoveWithCollisions(Vector3 velocity)
    {
        Vector3 position = col.transform.position;
        Vector3 wishVel = velocity * Time.deltaTime;

        Vector3[] planes = new Vector3[5];
        planeCount = 0;

        for (int bump = 0; bump < 4; bump++)
        {
            if (wishVel.magnitude < 0.001f)
                break;

            if (!Physics.BoxCast(position, col.bounds.extents, wishVel.normalized,
                out RaycastHit hit, Quaternion.identity, wishVel.magnitude + skinWidth))
            {
                position += wishVel;
                break;
            }

            // this is the collision
            Rigidbody rb = hit.collider.attachedRigidbody;
            float dist = Mathf.Max(hit.distance - skinWidth, 0f);
            Vector3 colVel = Vector3.zero;
            if (rb == null)
            {
                colVel = wishVel.normalized * dist;
                position += colVel;
            }
            else
            {
                if (rb.isKinematic)
                {
                    position += wishVel; // move fully through
                    break;
                }
                else
                {
                    rb.AddForce(wishVel * 10, ForceMode.Impulse);
                    rb.angularVelocity += wishVel.normalized;
                }
            }

            // Store plane
            if (planeCount < planes.Length)
                planes[planeCount++] = hit.normal;

            Vector3 newVel = wishVel - colVel;

            for (int i = 0; i < planeCount; i++)
            {
                Vector3 normal = planes[i];

                // actually clipping the velocity
                float velIntoPlane = Vector3.Dot(newVel, normal);
                if (velIntoPlane < 0f)
                {
                    if (rb == null)
                    {
                        newVel -= velIntoPlane * normal;
                    }
                    else
                    {
                        newVel -= velIntoPlane * normal * 0.1f;
                    }
                }

                //Debug.Log($"colNormal: {normal} vel: {newVel} vel: {velocity}");
            }

            wishVel = newVel;
        }

        col.transform.position = position;

        return wishVel / Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
