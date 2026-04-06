using UnityEngine;

public class WobblyHand : MonoBehaviour
{
    [SerializeField] private float accel = 0.1f;
    [SerializeField] private float friction = 0.985f;
    [SerializeField] public float gameLength = 10f;
    [SerializeField] private Collider fire;
    [SerializeField] private SceneLoader sl;
    private Liquid liquid;
    private float timer;
    private Vector2 velocity;
    private Vector2 targetPos;

    void Start()
    {
        liquid = GetComponentInChildren<Liquid>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= gameLength)
        {
            EndGame();
            return;
        }

        if (Random.value < 0.05f)
        {
            targetPos = Random.insideUnitCircle * 1.33f;
        }

        ApplyVelocity(targetPos);

        transform.position += (Vector3)velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, -velocity.x * 5f);
    }

    private void EndGame()
    {
        sl.Loadscene("SampleScene");
        Debug.Log("game ended. leave");
    }

    private void ApplyVelocity(Vector2 targetPos)
    {
        Vector2 dir = targetPos - (Vector2)transform.position;

        float alignment = Vector2.Dot(velocity.normalized, dir.normalized);
        float velFactor = accel * Mathf.Max(alignment, 0.5f);

        velocity += dir * velFactor;
        velocity *= friction;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider == fire)
        {
            float endTime = gameLength * 0.75f;
            float drainRate = 0.99f / endTime; // per second

            liquid.liquidLevel -= drainRate * Time.deltaTime;
        }
    }
}