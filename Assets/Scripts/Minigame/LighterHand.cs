using UnityEngine;
using UnityEngine.InputSystem;

public class LighterHand : MonoBehaviour
{
    [SerializeField] private float accel = 0.25f;
    [SerializeField] private float friction = 0.97f;
    private Vector2 velocity;

    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.WorldToScreenPoint(transform.position).z));
        Vector2 targetPos = new Vector2(worldPos.x, worldPos.y);

        ApplyVelocity(targetPos);

        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void ApplyVelocity(Vector2 targetPos)
    {
        Vector2 dir = targetPos - (Vector2)transform.position;

        float alignment = Vector2.Dot(velocity.normalized, dir.normalized);
        float velFactor = accel * Mathf.Max(alignment, 0.5f);

        velocity += dir * velFactor;
        velocity *= friction;
    }
}